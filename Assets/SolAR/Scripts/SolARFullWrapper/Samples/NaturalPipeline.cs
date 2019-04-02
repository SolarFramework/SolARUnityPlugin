/**
 * @copyright Copyright (c) 2017 B-com http://www.b-com.com/
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Linq;
using SolAR.Api.Display;
using SolAR.Api.Features;
using SolAR.Api.Geom;
using SolAR.Api.Input.Files;
using SolAR.Api.Solver.Pose;
using SolAR.Core;
using SolAR.Datastructure;
using UniRx;
using XPCF.Api;

namespace SolAR.Samples
{
    public class NaturalPipeline : AbstractPipeline
    {
        public NaturalPipeline(IComponentManager xpcfComponentManager) : base(xpcfComponentManager)
        {
            imageViewerKeypoints = Create<IImageViewer>("SolARImageViewerOpencv", "keypoints");
            imageViewerResult = Create<IImageViewer>("SolARImageViewerOpencv");
            marker = Create<IMarker2DNaturalImage>("SolARMarker2DNaturalImageOpencv");
            kpDetector = Create<IKeypointDetector>("SolARKeypointDetectorOpencv");
            matcher = Create<IDescriptorMatcher>("SolARDescriptorMatcherKNNOpencv");
            basicMatchesFilter = Create<IMatchesFilter>("SolARBasicMatchesFilter");
            geomMatchesFilter = Create<IMatchesFilter>("SolARGeometricMatchesFilterOpencv");
            homographyEstimation = Create<I2DTransformFinder>("SolARHomographyEstimationOpencv");
            homographyValidation = Create<IHomographyValidation>("SolARHomographyValidation");
            keypointsReindexer = Create<IKeypointsReIndexer>("SolARKeypointsReIndexer");
            poseEstimation = Create<I3DTransformFinderFrom2D3D>("SolARPoseEstimationPnpOpencv");
            //poseEstimation = Create<I3DTransformFinderFrom2D3D>("SolARPoseEstimationPnpEPFL");
            overlay2DComponent = Create<I2DOverlay>("SolAR2DOverlayOpencv");
            img_mapper = Create<IImage2WorldMapper>("SolARImage2WorldMapper4Marker2D");
            transform2D = Create<I2DTransform>("SolAR2DTransform");
            descriptorExtractor = Create<IDescriptorsExtractor>("SolARDescriptorsExtractorAKAZE2Opencv");

            /* in dynamic mode, we need to check that components are well created*/
            /* this is needed in dynamic mode */
            if (new object[] { imageViewerKeypoints, imageViewerResult, marker, kpDetector, descriptorExtractor, matcher, basicMatchesFilter, geomMatchesFilter, homographyEstimation, homographyValidation, keypointsReindexer, poseEstimation, overlay2DComponent, img_mapper, transform2D }.Contains(null))
            {
                LOG_ERROR("One or more component creations have failed");
                return;
            }
            LOG_INFO("All components have been created");

            // Declare data structures used to exchange information between components
            refImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            //kpImageCam = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            refDescriptors = SharedPtr.Alloc<DescriptorBuffer>().AddTo(subscriptions);
            camDescriptors = SharedPtr.Alloc<DescriptorBuffer>().AddTo(subscriptions);
            matches = new DescriptorMatchVector().AddTo(subscriptions);

            Hm = new Transform2Df().AddTo(subscriptions);
            // where to store detected keypoints in ref image and camera image
            refKeypoints = new KeypointList().AddTo(subscriptions);
            camKeypoints = new KeypointList().AddTo(subscriptions);

            // load marker
            LOG_INFO("LOAD MARKER IMAGE ");
            marker.loadMarker().Check();
            marker.getImage(refImage).Check();

            // detect keypoints in reference image
            LOG_INFO("DETECT MARKER KEYPOINTS ");
            kpDetector.detect(refImage, refKeypoints);

            // extract descriptors in reference image
            LOG_INFO("EXTRACT MARKER DESCRIPTORS ");
            descriptorExtractor.extract(refImage, refKeypoints, refDescriptors);
            LOG_INFO("EXTRACT MARKER DESCRIPTORS COMPUTED");

            // initialize image mapper with the reference image size and marker size
            var img_mapper_config = img_mapper.BindTo<IConfigurable>().AddTo(subscriptions);
            var refSize = refImage.getSize();
            var mkSize = marker.getSize();
            img_mapper_config.getProperty("digitalWidth").setIntegerValue((int)refSize.width);
            img_mapper_config.getProperty("digitalHeight").setIntegerValue((int)refSize.height);
            img_mapper_config.getProperty("worldWidth").setFloatingValue(mkSize.width);
            img_mapper_config.getProperty("worldHeight").setFloatingValue(mkSize.height);

            // vector of 4 corners in the marker
            refImgCorners = new Point2DfList();
            float w = refImage.getWidth(), h = refImage.getHeight();
            Point2Df corner0 = new Point2Df(0, 0);
            Point2Df corner1 = new Point2Df(w, 0);
            Point2Df corner2 = new Point2Df(w, h);
            Point2Df corner3 = new Point2Df(0, h);
            refImgCorners.Add(corner0);
            refImgCorners.Add(corner1);
            refImgCorners.Add(corner2);
            refImgCorners.Add(corner3);
        }

        public override void SetCameraParameters(Matrix3x3f intrinsics, Vector5f distorsion)
        {
            // initialize pose estimation
            poseEstimation.setCameraParameters(intrinsics, distorsion);
        }

        public override Sizef GetMarkerSize()
        {
            return new Sizef
            {
                width = marker.getWidth(),
                height = marker.getHeight()
            };
        }

        public override FrameworkReturnCode Proceed(Image camImage, Transform3Df pose)
        {
            //var matchesImage = new Image(refImage.getWidth() + camImage.getWidth(), refImage.getHeight(), refImage.getImageLayout(), refImage.getPixelOrder(), refImage.getDataType());
            //var matchImage = matchesImage;

            // detect keypoints in camera image
            kpDetector.detect(camImage, camKeypoints);
            // Not working, C2664 : cannot convert argument 1 from std::vector<boost_shared_ptr<Keypoint>> to std::vector<boost_shared_ptr<Point2Df>> !
            /* you can either draw keypoints */
            //kpDetector.drawKeypoints(camImage, camKeypoints, kpImageCam);

            /* extract descriptors in camera image*/

            descriptorExtractor.extract(camImage, camKeypoints, camDescriptors);

            /*compute matches between reference image and camera image*/
            matcher.match(refDescriptors, camDescriptors, matches);

            /* filter matches to remove redundancy and check geometric validity */
            basicMatchesFilter.filter(matches, matches, refKeypoints, camKeypoints);
            geomMatchesFilter.filter(matches, matches, refKeypoints, camKeypoints);

            /* we declare here the Solar datastucture we will need for homography*/
            var ref2Dpoints = new Point2DfList();
            var cam2Dpoints = new Point2DfList();
            //Point2Df point;
            var ref3Dpoints = new Point3DfList();
            //var output2Dpoints = new Point2DfList();
            var markerCornersinCamImage = new Point2DfList();
            var markerCornersinWorld = new Point3DfList();

            /*we consider that, if we have less than 10 matches (arbitrarily), we can't compute homography for the current frame */

            if (matches.Count > 10)
            {
                // reindex the keypoints with established correspondence after the matching
                keypointsReindexer.reindex(refKeypoints, camKeypoints, matches, ref2Dpoints, cam2Dpoints).Check();

                // mapping to 3D points
                img_mapper.map(ref2Dpoints, ref3Dpoints).Check();

                var res = homographyEstimation.find(ref2Dpoints, cam2Dpoints, Hm);
                //test if a meaningful matrix has been obtained
                if (res == Api.Solver.Pose.RetCode.TRANSFORM2D_ESTIMATION_OK)
                {
                    //poseEstimation.poseFromHomography(Hm, pose, objectCorners, sceneCorners);
                    // vector of 2D corners in camera image
                    transform2D.transform(refImgCorners, Hm, markerCornersinCamImage).Check();
                    // draw circles on corners in camera image
                    overlay2DComponent.drawCircles(markerCornersinCamImage, camImage); //DEBUG

                    /* we verify is the estimated homography is valid*/
                    if (homographyValidation.isValid(refImgCorners, markerCornersinCamImage))
                    {
                        // from the homography we create 4 points at the corners of the reference image
                        // map corners in 3D world coordinates
                        img_mapper.map(refImgCorners, markerCornersinWorld).Check();

                        // pose from solvePNP using 4 points.
                        /* The pose could also be estimated from all the points used to estimate the homography */
                        poseEstimation.estimate(markerCornersinCamImage, markerCornersinWorld, pose).Check();

                        return FrameworkReturnCode._SUCCESS;
                    }
                    else /* when homography is not valid*/
                        LOG_INFO("Wrong homography for this frame");
                }
            }
            return FrameworkReturnCode._ERROR_;
        }

        // structures
        readonly Image refImage;
        //readonly Image kpImageCam;
        readonly DescriptorBuffer refDescriptors;
        readonly DescriptorBuffer camDescriptors;
        readonly DescriptorMatchVector matches;
        readonly Transform2Df Hm;
        readonly KeypointList refKeypoints;
        readonly KeypointList camKeypoints;

        // components
        readonly IImageViewer imageViewerKeypoints;
        readonly IImageViewer imageViewerResult;
        readonly IMarker2DNaturalImage marker;
        readonly IKeypointDetector kpDetector;
        readonly IDescriptorMatcher matcher;
        readonly IMatchesFilter basicMatchesFilter;
        readonly IMatchesFilter geomMatchesFilter;
        readonly I2DTransformFinder homographyEstimation;
        readonly IHomographyValidation homographyValidation;
        readonly IKeypointsReIndexer keypointsReindexer;
        readonly I3DTransformFinderFrom2D3D poseEstimation;
        //readonly I3DTransformFinderFrom2D3D poseEstimation;
        readonly I2DOverlay overlay2DComponent;
        readonly IImage2WorldMapper img_mapper;
        readonly I2DTransform transform2D;
        readonly IDescriptorsExtractor descriptorExtractor;

        readonly Point2DfList refImgCorners;
    }
}
