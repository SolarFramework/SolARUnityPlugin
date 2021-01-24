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

//#define NDEBUG

using SolAR.Api.Display;
using SolAR.Api.Features;
using SolAR.Api.Geom;
using SolAR.Api.Input.Devices;
using SolAR.Api.Input.Files;
using SolAR.Api.Solver.Pose;
using SolAR.Api.Tracking;
using SolAR.Core;
using SolAR.Datastructure;
using UniRx;
using XPCF.Api;

namespace SolAR.Expert.Samples
{
    public class NaturalSample : AbstractSample
    {
        const int updateTrackedPointThreshold = 300;

        public NaturalSample(IComponentManager xpcfComponentManager) : base(xpcfComponentManager)
        {
            // declare and create components
            LOG_INFO("Start creating components");

            //camera = Resolve<ICamera>();
            marker = Resolve<IMarker2DNaturalImage>();
            kpDetector = Resolve<IKeypointDetector>();
            kpDetectorRegion = Resolve<IKeypointDetectorRegion>();
            descriptorExtractor = Resolve<IDescriptorsExtractor>();
            matcher = Resolve<IDescriptorMatcher>();
            basicMatchesFilter = Resolve<IMatchesFilter>();
            geomMatchesFilter = Resolve<IMatchesFilter>();
            keypointsReindexer = Resolve<IKeypointsReIndexer>();
            poseEstimationPlanar = Resolve<I3DTransformSACFinderFrom2D3D>();
            img_mapper = Resolve<IImage2WorldMapper>();
            opticalFlowEstimator = Resolve<IOpticalFlowEstimator>();
            projection = Resolve<IProject>();
            unprojection = Resolve<IUnproject>();
            overlay2DComponent = Resolve<I2DOverlay>();
            overlay3DComponent = Resolve<I3DOverlay>();
            imageViewerKeypoints = Resolve<IImageViewer>("keypoints");
            imageViewerResult = Resolve<IImageViewer>();

            // Declare data structures used to exchange information between components
            refImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            //camImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            previousCamImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            kpImageCam = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            refKeypoints = new KeypointArray().AddTo(subscriptions);
            refDescriptors = SharedPtr.Alloc<DescriptorBuffer>().AddTo(subscriptions);
            //camDescriptors = SharedPtr.Alloc<DescriptorBuffer>().AddTo(subscriptions);
            //matches = new DescriptorMatchVector().AddTo(subscriptions);
            markerWorldCorners = new Point3DfArray().AddTo(subscriptions);

            Init();
        }

        readonly Point2DfArray projectedMarkerCorners = new Point2DfArray();
        readonly UIntVector inliers = new UIntVector();
        readonly Point2DfArray imagePoints_track = new Point2DfArray();
        readonly Point3DfArray worldPoints_track = new Point3DfArray();
        //Transform3Df pose;
        bool valid_pose = false;
        bool isTrack = false;
        bool needNewTrackedPoints = false;

        void Init()
        {
            // load marker
            LOG_INFO("LOAD MARKER IMAGE ");
            marker.loadMarker().Check();
            marker.getWorldCorners(markerWorldCorners).Check();

            marker.getImage(refImage).Check();

            // detect keypoints in reference image
            kpDetector.detect(refImage, refKeypoints);

            // extract descriptors in reference image
            descriptorExtractor.extract(refImage, refKeypoints, refDescriptors);

#if !NDEBUG
            // display keypoints in reference image
            // copy reference image
            var kpImage = refImage.copy();
            // draw circles on keypoints

            overlay2DComponent.drawCircles(refKeypoints, kpImage);
            // displays the image with circles in an imageviewer
            imageViewerKeypoints.display(kpImage);
#endif

            // initialize image mapper with the reference image size and marker size
            var img_mapper_config = img_mapper.BindTo<IConfigurable>().AddTo(subscriptions);
            var refSize = refImage.getSize();
            var mkSize = marker.getSize();
            img_mapper_config.getProperty("digitalWidth").setIntegerValue((int)refSize.width);
            img_mapper_config.getProperty("digitalHeight").setIntegerValue((int)refSize.height);
            img_mapper_config.getProperty("worldWidth").setFloatingValue(mkSize.width);
            img_mapper_config.getProperty("worldHeight").setFloatingValue(mkSize.height);
        }

        public override void SetCameraParameters(Matrix3x3f intrinsics, Vector5f distortion)
        {
            // initialize overlay 3D component with the camera intrinsec parameters (please refeer to the use of intrinsec parameters file)
            overlay3DComponent.setCameraParameters(intrinsics, distortion);

            // initialize pose estimation based on planar points with the camera intrinsec parameters (please refeer to the use of intrinsec parameters file)
            poseEstimationPlanar.setCameraParameters(intrinsics, distortion);

            // initialize projection component with the camera intrinsec parameters (please refeer to the use of intrinsec parameters file)
            projection.setCameraParameters(intrinsics, distortion);

            // initialize unprojection component with the camera intrinsec parameters (please refeer to the use of intrinsec parameters file)
            unprojection.setCameraParameters(intrinsics, distortion);
        }

        public override FrameworkReturnCode Proceed(Image camImage, Transform3Df pose, ICamera camera)
        {
            /* we declare here the Solar datastucture we will need for homography*/
            var camKeypoints = new KeypointArray(); // where to store detected keypoints in ref image and camera image
            var camDescriptors = new DescriptorBuffer();
            var matches = new DescriptorMatchVector();
            var refMatched2Dpoints = new Point2DfArray();
            var camMatched2Dpoints = new Point2DfArray();
            var ref3Dpoints = new Point3DfArray();

            // Detection mode
            if (!isTrack) // We estimate the pose by matching marker planar keypoints and current image keypoints and by estimating the pose based on planar points
            {
                //detect natural marker from features points
                // detect keypoints in camera image
                kpDetector.detect(camImage, camKeypoints);

#if !NDEBUG
                kpImageCam = camImage.copy();
                //overlay2DComponent->drawCircles(camKeypoints, kpImageCam);
#endif

                /* extract descriptors in camera image*/
                descriptorExtractor.extract(camImage, camKeypoints, camDescriptors);

                /*compute matches between reference image and camera image*/
                matcher.match(refDescriptors, camDescriptors, matches);

                /* filter matches to remove redundancy and check geometric validity */
                basicMatchesFilter.filter(matches, matches, refKeypoints, camKeypoints);
                geomMatchesFilter.filter(matches, matches, refKeypoints, camKeypoints);

                /*we consider that, if we have more than 10 matches (arbitrarily), we can compute homography for the current frame */
                if (matches.Count > 10)
                {
                    // reindex the keypoints with established correspondence after the matching
                    keypointsReindexer.reindex(refKeypoints, camKeypoints, matches, refMatched2Dpoints, camMatched2Dpoints).Check();

                    // mapping to 3D points
                    img_mapper.map(refMatched2Dpoints, ref3Dpoints).Check();

                    // Estimate the pose from the 2D-3D planar correspondence
                    if (poseEstimationPlanar.estimate(camMatched2Dpoints, ref3Dpoints, inliers, pose) != FrameworkReturnCode._SUCCESS)
                    {
                        valid_pose = false;
                        LOG_DEBUG("Wrong homography for this frame");
                    }
                    else
                    {
#if TRACKING
                        isTrack = true;
                        needNewTrackedPoints = true;
#endif
                        valid_pose = true;
                        previousCamImage = camImage.copy();
                        LOG_INFO("Start tracking", pose);
                    }
                }
            }
            else // We track planar keypoints and we estimate the pose based on a homography
            {
                // initialize points to track
                if (needNewTrackedPoints)
                {
                    imagePoints_track.Clear();
                    worldPoints_track.Clear();
                    KeypointArray newKeypoints = new KeypointArray();
                    // Get the projection of the corner of the marker in the current image
                    projection.project(markerWorldCorners, projectedMarkerCorners, pose);
#if !NDEBUG
                    overlay2DComponent.drawContour(projectedMarkerCorners, kpImageCam);
#endif
                    // Detect the keypoints within the contours of the marker defined by the projected corners
                    kpDetectorRegion.detect(previousCamImage, projectedMarkerCorners, newKeypoints);

                    if (newKeypoints.Count > updateTrackedPointThreshold)
                    {
                        foreach (var keypoint in newKeypoints)
                            //imagePoints_track.push_back(xpcf::utils::make_shared<Point2Df>(keypoint->getX(), keypoint->getY()));
                            imagePoints_track.Add(new Point2Df(keypoint.getX(), keypoint.getY()));

                        // get back the 3D positions of the detected keypoints in world space
                        unprojection.unproject(imagePoints_track, worldPoints_track, pose);
                        LOG_DEBUG("Reinitialize points to track");
                    }
                    else
                    {
                        isTrack = false;
                        LOG_DEBUG("Cannot reinitialize points to track");
                    }
                    needNewTrackedPoints = false;
                }

                // Tracking mode
                if (isTrack)
                {
                    Point2DfArray trackedPoints = new Point2DfArray();
                    Point2DfArray pts2D = new Point2DfArray();
                    Point3DfArray pts3D = new Point3DfArray();
                    UCharList status = new UCharList();
                    FloatList err = new FloatList();

                    // tracking 2D-2D
                    opticalFlowEstimator.estimate(previousCamImage, camImage, imagePoints_track, trackedPoints, status, err);

                    for (int i = 0; i < status.Count; i++)
                    {
                        if (status[i] == 1)
                        {
                            pts2D.Add(trackedPoints[i]);
                            pts3D.Add(worldPoints_track[i]);
                        }
                    }

#if !NDEBUG
                    kpImageCam = camImage.copy();
                    overlay2DComponent.drawCircles(pts2D, kpImageCam);
#endif

                    // calculate camera pose
                    // Estimate the pose from the 2D-3D planar correspondence
                    if (poseEstimationPlanar.estimate(pts2D, pts3D, inliers, pose) != FrameworkReturnCode._SUCCESS)
                    {
                        isTrack = false;
                        valid_pose = false;
                        needNewTrackedPoints = false;
                        LOG_INFO("Tracking lost");
                    }
                    else
                    {
                        valid_pose = true;
                        imagePoints_track.Clear();
                        worldPoints_track.Clear();
                        foreach (int index in inliers)
                        {
                            imagePoints_track.Add(pts2D[index]);
                            worldPoints_track.Add(pts3D[index]);
                        }
                        previousCamImage = camImage.copy();
                        if (worldPoints_track.Count < updateTrackedPointThreshold)
                            needNewTrackedPoints = true;
                    }
                }
                else
                    LOG_INFO("Tracking lost");
            }

            //draw a cube if the pose if valid
            if (valid_pose)
            {
                // We draw a box on the place of the recognized natural marker
#if !NDEBUG
                overlay3DComponent.draw(pose, kpImageCam);
#else
                overlay3DComponent.draw(pose, camImage);
#endif
            }
#if !NDEBUG
            if (imageViewerResult.display(kpImageCam).Check() == FrameworkReturnCode._STOP)
#else
            if (imageViewerResult.display(camImage).Check() == FrameworkReturnCode._STOP)
#endif
                return FrameworkReturnCode._SUCCESS;

            return FrameworkReturnCode._ERROR_;
        }

        // components
        readonly IMarker2DNaturalImage marker;
        readonly IKeypointDetector kpDetector;
        readonly IKeypointDetectorRegion kpDetectorRegion;
        readonly IDescriptorsExtractor descriptorExtractor;
        readonly IDescriptorMatcher matcher;
        readonly IMatchesFilter basicMatchesFilter;
        readonly IMatchesFilter geomMatchesFilter;
        readonly IKeypointsReIndexer keypointsReindexer;
        readonly I3DTransformSACFinderFrom2D3D poseEstimationPlanar;
        readonly IImage2WorldMapper img_mapper;
        readonly IOpticalFlowEstimator opticalFlowEstimator;
        readonly IProject projection;
        readonly IUnproject unprojection;
        readonly I2DOverlay overlay2DComponent;
        readonly I3DOverlay overlay3DComponent;
        readonly IImageViewer imageViewerKeypoints;
        readonly IImageViewer imageViewerResult;

        // readonly IOpticalFlowEstimator opticalFlow;
        // readonly Point2DfArray refImgCorners;

        // Declare data structures used to exchange information between components
        readonly Image refImage;
        //readonly Image camImage;
        Image previousCamImage;
        Image kpImageCam;
        readonly KeypointArray refKeypoints;
        readonly DescriptorBuffer refDescriptors;
        //readonly DescriptorBuffer camDescriptors;
        //readonly DescriptorMatchVector matches;
        readonly Point3DfArray markerWorldCorners;
    }
}
