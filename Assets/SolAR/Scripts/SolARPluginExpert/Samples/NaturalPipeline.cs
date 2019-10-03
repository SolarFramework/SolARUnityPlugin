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
using SolAR.Api.Tracking;
using SolAR.Api.Input.Files;
using SolAR.Api.Solver.Pose;
using SolAR.Core;
using SolAR.Datastructure;
using UniRx;
using XPCF.Api;
using SolAR.Api.Image;
using SolAR.Api.Sink;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using SolAR.Api.Input.Devices;

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
            kpDetectorRegion = Create<IKeypointDetectorRegion>("SolARKeypointDetectorRegionOpencv");
            descriptorExtractor = Create<IDescriptorsExtractor>("SolARDescriptorsExtractorAKAZE2Opencv");
            matcher = Create<IDescriptorMatcher>("SolARDescriptorMatcherKNNOpencv");
            geomMatchesFilter = Create<IMatchesFilter>("SolARGeometricMatchesFilterOpencv");
            poseEstimationPlanar = Create<I3DTransformSACFinderFrom2D3D>("SolARPoseEstimationPlanarPointsOpencv");
            opticalFlow = Create<IOpticalFlowEstimator>("SolAROpticalFlowPyrLKOpencv");
            projection = Create<IProject>("SolARProjectOpencv");
            unprojection = Create<IUnproject>("SolARUnprojectPlanarPointsOpencv");
            img_mapper = Create<IImage2WorldMapper>("SolARImage2WorldMapper4Marker2D");
            basicMatchesFilter = Create<IMatchesFilter>("SolARBasicMatchesFilter");
            keypointsReindexer = Create<IKeypointsReIndexer>("SolARKeypointsReIndexer");
            overlay3DComponent = Create<I3DOverlay>("SolAR3DOverlayOpencv");
            /* in dynamic mode, we need to check that components are well created*/
            /* this is needed in dynamic mode */
            if (new object[] { imageViewerKeypoints, imageViewerResult, marker, kpDetector , kpDetectorRegion , descriptorExtractor , matcher,
                                geomMatchesFilter, poseEstimationPlanar, opticalFlow, projection, unprojection, img_mapper,
                                basicMatchesFilter,keypointsReindexer, overlay3DComponent }.Contains(null))
            {
                LOG_ERROR("One or more component creations have failed");
                return;
            }
            LOG_INFO("All components have been created");

            // Declare data structures used to exchange information between components
            refImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            previousCamImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);

            //kpImageCam = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            refDescriptors = SharedPtr.Alloc<DescriptorBuffer>().AddTo(subscriptions);
            camDescriptors = SharedPtr.Alloc<DescriptorBuffer>().AddTo(subscriptions);
            matches = new DescriptorMatchVector().AddTo(subscriptions);

            // where to store detected keypoints in ref image and camera image
            refKeypoints = new KeypointArray().AddTo(subscriptions);
            camKeypoints = new KeypointArray().AddTo(subscriptions);

            markerWorldCorners = new Point3DfArray().AddTo(subscriptions);

            // load marker
            marker.loadMarker().Check();
            marker.getWorldCorners(markerWorldCorners).Check();
            marker.getImage(refImage).Check();

            // detect keypoints in reference image
            kpDetector.detect(refImage, refKeypoints);

            // extract descriptors in reference image
            descriptorExtractor.extract(refImage, refKeypoints, refDescriptors);

           

            // initialize image mapper with the reference image size and marker size
            var img_mapper_config = img_mapper.BindTo<IConfigurable>().AddTo(subscriptions);
            var refSize = refImage.getSize();
            var mkSize = marker.getSize();
            img_mapper_config.getProperty("digitalWidth").setIntegerValue((int)refSize.width);
            img_mapper_config.getProperty("digitalHeight").setIntegerValue((int)refSize.height);
            img_mapper_config.getProperty("worldWidth").setFloatingValue(mkSize.width);
            img_mapper_config.getProperty("worldHeight").setFloatingValue(mkSize.height);

            // vector of 4 corners in the marker
            refImgCorners = new Point2DfArray();
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
            poseEstimationPlanar.setCameraParameters(intrinsics, distorsion);
        }

        public override Sizef GetMarkerSize()
        {
            return new Sizef
            {
                width = marker.getWidth(),
                height = marker.getHeight()
            };
        }

        public override FrameworkReturnCode Proceed(Image camImage, Transform3Df pose, ICamera camera)
        {
            // initialize overlay 3D component with the camera intrinsec parameters (please refeer to the use of intrinsec parameters file)
            overlay3DComponent.setCameraParameters(camera.getIntrinsicsParameters(), camera.getDistorsionParameters());

            // initialize pose estimation based on planar points with the camera intrinsec parameters (please refeer to the use of intrinsec parameters file)
            poseEstimationPlanar.setCameraParameters(camera.getIntrinsicsParameters(), camera.getDistorsionParameters());

            // initialize projection component with the camera intrinsec parameters (please refeer to the use of intrinsec parameters file)
            projection.setCameraParameters(camera.getIntrinsicsParameters(), camera.getDistorsionParameters());

            // initialize unprojection component with the camera intrinsec parameters (please refeer to the use of intrinsec parameters file)
            unprojection.setCameraParameters(camera.getIntrinsicsParameters(), camera.getDistorsionParameters());
            if (!isTrack)
            {
                kpDetector.detect(camImage, camKeypoints);
                descriptorExtractor.extract(camImage, camKeypoints, camDescriptors);
                matcher.match(refDescriptors, camDescriptors, matches);
                basicMatchesFilter.filter(matches, matches, refKeypoints, camKeypoints);
                geomMatchesFilter.filter(matches, matches, refKeypoints, camKeypoints);

                var ref2Dpoints = new Point2DfArray();
                var cam2Dpoints = new Point2DfArray();
                var ref3Dpoints = new Point3DfArray();

                if (matches.Count > 10)
                {
                    keypointsReindexer.reindex(refKeypoints, camKeypoints, matches, ref2Dpoints, cam2Dpoints).Check();
                    img_mapper.map(ref2Dpoints, ref3Dpoints).Check();
                    if (poseEstimationPlanar.estimate(cam2Dpoints, ref3Dpoints, imagePoints_inliers , worldPoints_inliers, pose) != FrameworkReturnCode._SUCCESS)
                    {
                        valid_pose = false;
                        //LOG_DEBUG("Wrong homography for this frame");
                    }
                    else
                    {
                        isTrack = true;
                        needNewTrackedPoints = true;
                        valid_pose = true;
                        previousCamImage = camImage.copy();
                        //LOG_INFO("Start tracking", pose.matrix());
                    }
                }
            }
            else
            {
                // initialize points to track
                if (needNewTrackedPoints)
                {
                    imagePoints_track.Clear();
                    worldPoints_track.Clear();
                    KeypointArray newKeypoints = new KeypointArray();
                    // Get the projection of the corner of the marker in the current image
                    projection.project(markerWorldCorners, projectedMarkerCorners, pose);

                    // Detect the keypoints within the contours of the marker defined by the projected corners
                    kpDetectorRegion.detect(previousCamImage, projectedMarkerCorners, newKeypoints);

                    if (newKeypoints.Count > updateTrackedPointThreshold)
                    {
                        foreach(var keypoint in newKeypoints)
                            //imagePoints_track.push_back(xpcf::utils::make_shared<Point2Df>(keypoint->getX(), keypoint->getY()));
                              imagePoints_track.Add(new Point2Df(keypoint.getX(), keypoint.getY()));
                        // get back the 3D positions of the detected keypoints in world space
                        unprojection.unproject(imagePoints_track, worldPoints_track, pose);
                        //LOG_DEBUG("Reinitialize points to track");
                    }
                    else
                    {
                        isTrack = false;
                        //LOG_DEBUG("Cannot reinitialize points to track");
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
                    opticalFlow.estimate(previousCamImage, camImage, imagePoints_track, trackedPoints, status, err);

                    for (int i = 0; i < status.Count; i++)
                    {
                        if (status[i] == 1)
                        {
                            pts2D.Add(trackedPoints[i]);
                            pts3D.Add(worldPoints_track[i]);
                        }
                    }
                    // calculate camera pose
                    // Estimate the pose from the 2D-3D planar correspondence
                    if (poseEstimationPlanar.estimate(pts2D, pts3D, imagePoints_track, worldPoints_track, pose) != FrameworkReturnCode._SUCCESS)
                    {
                        isTrack = false;
                        valid_pose = false;
                        needNewTrackedPoints = false;
                        //LOG_INFO("Tracking lost");
                    }
                    else
                    {
                        valid_pose = true;
                        previousCamImage = camImage.copy();
                        if (worldPoints_track.Count < updateTrackedPointThreshold)
                            needNewTrackedPoints = true;
                    }
                }
                //else
                    //LOG_INFO("Tracking lost");
            }

            if (valid_pose)
            {
                // We draw a box on the place of the recognized natural marker
                overlay3DComponent.draw(pose, camImage);
            }
            //if (imageViewerResult.display(camImage) == FrameworkReturnCode._STOP) return FrameworkReturnCode._STOP;
            return FrameworkReturnCode._SUCCESS;
        }

        int updateTrackedPointThreshold = 300;

        Point2DfArray projectedMarkerCorners = new Point2DfArray();
        Point2DfArray imagePoints_inliers = new Point2DfArray();
        Point3DfArray worldPoints_inliers = new Point3DfArray();
        Point2DfArray imagePoints_track = new Point2DfArray();
        Point3DfArray worldPoints_track = new Point3DfArray();
        bool isTrack = false;
        bool valid_pose = false;
        bool needNewTrackedPoints = false;

        // structures
        readonly Image refImage;
        Image previousCamImage;
        //readonly Image kpImageCam;
        readonly DescriptorBuffer refDescriptors;
        readonly DescriptorBuffer camDescriptors;
        readonly DescriptorMatchVector matches;
        readonly KeypointArray refKeypoints;
        readonly KeypointArray camKeypoints;

        readonly Point3DfArray markerWorldCorners;

        // components
        readonly IImageViewer imageViewerKeypoints;
        readonly IImageViewer imageViewerResult;
        readonly IMarker2DNaturalImage marker;
        readonly IKeypointDetector kpDetector;
        readonly IKeypointDetectorRegion kpDetectorRegion;
        readonly IDescriptorMatcher matcher;
        readonly IMatchesFilter basicMatchesFilter;
        readonly IMatchesFilter geomMatchesFilter;
        readonly IKeypointsReIndexer keypointsReindexer;
        readonly I3DOverlay overlay3DComponent;
        readonly IImage2WorldMapper img_mapper;
        readonly IDescriptorsExtractor descriptorExtractor;
        readonly I3DTransformSACFinderFrom2D3D poseEstimationPlanar;
        readonly IOpticalFlowEstimator opticalFlow;
        readonly IProject projection;
        readonly IUnproject unprojection;
        readonly Point2DfArray refImgCorners;
    }
}
