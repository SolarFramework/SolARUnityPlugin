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
using SolAR.Api.Solver.Map;
using SolAR.Api.Reloc;
using System.Collections.Generic;
using System.Threading;

namespace SolAR.Samples
{
    public class SlamPipeline : AbstractPipeline
    {
        public SlamPipeline(IComponentManager xpcfComponentManager) : base(xpcfComponentManager)
        {
            //            keypointsDetector = Create<IKeypointDetector>("SolARKeypointDetectorOpencv");
            //            descriptorExtractor = Create<IDescriptorsExtractor>("SolARDescriptorsExtractorAKAZE2Opencv");
            //            matcher = Create<IDescriptorMatcher>("SolARDescriptorMatcherKNNOpencv");
            //            poseFinderFrom2D2D = Create<I3DTransformFinderFrom2D2D>("SolARPoseFinderFrom2D2DOpencv");
            //            triangulator = Create<ITriangulator>("SolARSVDTriangulationOpencv");
            //            matchesFilter = Create<IMatchesFilter>("SolARGeometricMatchesFilterOpencv");
            //            pnpRansac = Create<I3DTransformSACFinderFrom2D3D>("SolARPoseEstimationSACPnpOpencv");
            //            pnp = Create<I3DTransformFinderFrom2D3D>("SolARPoseEstimationPnpOpencv");
            //            corr2D3DFinder = Create<I2D3DCorrespondencesFinder>("SolAR2D3DCorrespondencesFinderOpencv");
            //            mapFilter = Create<IMapFilter>("SolARMapFilter");
            //            mapper = Create<IMapper>("SolARMapper");
            //            keyframeSelector = Create<IKeyframeSelector>("SolARKeyframeSelector");
            //            matchesOverlay = Create<IMatchesOverlay>("SolARMatchesOverlayOpencv");
            //            matchesOverlayBlue = Create<IMatchesOverlay>("SolARMatchesOverlayOpencv");
            //            matchesOverlayRed = Create<IMatchesOverlay>("SolARPoseFinderFrom2D2DOpencv");
            //            imageViewer = Create<IImageViewer>("SolARImageViewerOpencv");
            //            viewer3DPoints = Create<I3DPointsViewer>("SolAR3DPointsViewerOpengl");
            //            kfRetriever = Create<IKeyframeRetriever>("SolARKeyframeRetrieverFBOW");
            //            projector = Create<IProject>("SolARProjectOpencv");
            //            bundler = Create<IBundler>("SolARBundlerCeres");

            //            binaryMarker = Create<IMarker2DSquaredBinary>("SolARMarker2DSquaredBinaryOpencv");
            //            imageFilterBinary = Create<IImageFilter>("SolARImageFilterBinaryOpencv");
            //            imageConvertor = Create<IImageConvertor>("SolARImageConvertorOpencv");
            //            contoursExtractor = Create<IContoursExtractor>("SolARContoursExtractorOpencv");
            //            contoursFilter = Create<IContoursFilter>("SolARContoursFilterBinaryMarkerOpencv");
            //            perspectiveController = Create<IPerspectiveController>("SolARPerspectiveControllerOpencv");
            //            patternDescriptorExtractor = Create<IDescriptorsExtractorSBPattern>("SolARDescriptorsExtractorSBPatternOpencv");
            //            patternMatcher = Create<IDescriptorMatcher>("SolARDescriptorMatcherRadiusOpencv");
            //            patternReIndexer = Create<ISBPatternReIndexer>("SolARSBPatternReIndexer");
            //            img2worldMapper = Create<IImage2WorldMapper>("SolARImage2WorldMapper4Marker2D");
            //            overlay3D = Create<I3DOverlay>("SolAR3DOverlayBoxOpencv");
            //            overlay2D = Create<I2DOverlay>("SolAR2DOverlayOpencv");

            //            /* in dynamic mode, we need to check that components are well created*/
            //            /* this is needed in dynamic mode */
            //            if (new object[] { keypointsDetector, descriptorExtractor, matcher, triangulator , matchesFilter , pnpRansac , pnp,
            //                                corr2D3DFinder, mapFilter, mapper, keyframeSelector, matchesOverlay, matchesOverlayBlue,
            //                                matchesOverlayRed,imageViewer, viewer3DPoints, kfRetriever, projector, bundler, binaryMarker,
            //                                imageFilterBinary, imageConvertor,contoursExtractor, contoursFilter, perspectiveController,
            //                                patternDescriptorExtractor, patternMatcher, patternReIndexer, img2worldMapper,
            //                                overlay3D, overlay2D}.Contains(null))
            //            {
            //                LOG_ERROR("One or more component creations have failed");
            //                return;
            //            }
            //            LOG_INFO("All components have been created");

            //            view2 = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            //            view = SharedPtr.Alloc<Image>().AddTo(subscriptions);

            //            //keyframe1 = SharedPtr.Alloc<Datastructure.Keyframe>().AddTo(subscriptions);
            //            //keyframe2 = SharedPtr.Alloc<Datastructure.Keyframe>().AddTo(subscriptions);
            //            keypointsView1 = new KeypointArray().AddTo(subscriptions);
            //            keypointsView2 = new KeypointArray().AddTo(subscriptions);
            //            keypointsView = new KeypointArray().AddTo(subscriptions);

            //            descriptorsView1 = SharedPtr.Alloc<DescriptorBuffer>().AddTo(subscriptions);
            //            descriptorsView2 = SharedPtr.Alloc<DescriptorBuffer>().AddTo(subscriptions);
            //            descriptorsView = SharedPtr.Alloc<DescriptorBuffer>().AddTo(subscriptions);

            //            matches = new DescriptorMatchVector().AddTo(subscriptions);

            //            poseFrame1 = new Transform3Df().AddTo(subscriptions);
            //            poseFrame2 = new Transform3Df().AddTo(subscriptions);
            //            newFramePose = new Transform3Df().AddTo(subscriptions);
            //            lastPose = new Transform3Df().AddTo(subscriptions);

            //            cloud = new CloudPointVector().AddTo(subscriptions);
            //            filteredCloud = new CloudPointVector().AddTo(subscriptions);

            //            keyframePoses = new Transform3DfList().AddTo(subscriptions);
            //            framePoses = new Transform3DfList().AddTo(subscriptions);

            //            newFrame = SharedPtr.Alloc<Frame>().AddTo(subscriptions);
            //            frameToTrack = SharedPtr.Alloc<Frame>().AddTo(subscriptions);

            //            referenceKeyframe = SharedPtr.Alloc<Datastructure.Keyframe>().AddTo(subscriptions);
            //            updatedRefKf = SharedPtr.Alloc<Datastructure.Keyframe>().AddTo(subscriptions);

            //            imageMatches = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            //            imageMatches2 = SharedPtr.Alloc<Image>().AddTo(subscriptions);

            //            map = SharedPtr.Alloc<Map>().AddTo(subscriptions);
            //            localMap = new CloudPointVector().AddTo(subscriptions);

            //            idxLocalMap = new UIntVector().AddTo(subscriptions);
            //            isLostTrack = false;
            //            bundling = false;
            //            windowIdxBundling = new IntVector().AddTo(subscriptions);
            //            markerPatternDescriptor = SharedPtr.Alloc<DescriptorBuffer>().AddTo(subscriptions);
        }

        public override void SetCameraParameters(Matrix3x3f intrinsics, Vector5f distorsion)
        {
            //            pnp.setCameraParameters(intrinsics, distorsion);
            //            pnpRansac.setCameraParameters(intrinsics, distorsion);
            //            poseFinderFrom2D2D.setCameraParameters(intrinsics, distorsion);
            //            triangulator.setCameraParameters(intrinsics, distorsion);
            //            projector.setCameraParameters(intrinsics, distorsion);
            //            overlay3D.setCameraParameters(intrinsics, distorsion);
        }
        public override Sizef GetMarkerSize()
        {
            return new Sizef();
            //return binaryMarker.getSize();
        }


        public override FrameworkReturnCode Proceed(Image camImage, Transform3Df pose, ICamera camera)
        {
            //            Action<IntVector, double> localBundleAdjuster = (IntVector framesIdxToBundle, double reprojError) =>
            //            {
            //                Transform3DfList correctedPoses = new Transform3DfList();
            //                CloudPointVector correctedCloud = new CloudPointVector();
            //                CameraParameters correctedCamera = new CameraParameters();
            //                reprojError = bundler.solve(mapper.getKeyframes(),
            //                                             mapper.getGlobalMap().getPointCloud(),
            //                                             camera.getIntrinsicsParameters(),
            //                                             camera.getDistorsionParameters(),
            //                                             framesIdxToBundle,
            //                                             correctedPoses,
            //                                             correctedCloud,
            //                                             correctedCamera.intrinsic,
            //                                             correctedCamera.distorsion);
            //                mapper.update(correctedPoses, correctedCloud);
            //            };
            //            binaryMarker.loadMarker().Check();
            //            patternDescriptorExtractor.extract(binaryMarker.getPattern(), markerPatternDescriptor);
            //            var patternSize = binaryMarker.getPattern().getSize();
            //            var binaryMarkerSize = binaryMarker.getSize();
            //            overlay3D.BindTo<IConfigurable>().getProperty("size").setFloatingValue(binaryMarkerSize.width, 0);
            //            overlay3D.BindTo<IConfigurable>().getProperty("size").setFloatingValue(binaryMarkerSize.height, 1);
            //            overlay3D.BindTo<IConfigurable>().getProperty("size").setFloatingValue(binaryMarkerSize.height / 2.0f, 2);

            //            patternDescriptorExtractor.BindTo<IConfigurable>().getProperty("patternSize").setIntegerValue(patternSize);
            //            patternReIndexer.BindTo<IConfigurable>().getProperty("sbPatternSize").setIntegerValue(patternSize);
            //            img2worldMapper.BindTo<IConfigurable>().getProperty("digitalWidth").setIntegerValue(patternSize);
            //            img2worldMapper.BindTo<IConfigurable>().getProperty("digitalHeight").setIntegerValue(patternSize);
            //            img2worldMapper.BindTo<IConfigurable>().getProperty("worldWidth").setFloatingValue(binaryMarkerSize.width);
            //            img2worldMapper.BindTo<IConfigurable>().getProperty("worldHeight").setFloatingValue(binaryMarkerSize.height);

            //            Func<Image, Transform3Df, bool> detectFiducialMarker = (Image image, Transform3Df localpose) =>
            //            {
            //                Image greyImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            //                Image binaryImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);

            //                Contour2DfArray contours = new Contour2DfArray().AddTo(subscriptions);
            //                Contour2DfArray filtered_contours = new Contour2DfArray().AddTo(subscriptions);
            //                ImageList patches = new ImageList().AddTo(subscriptions);
            //                Contour2DfArray recognizedContours = new Contour2DfArray().AddTo(subscriptions);
            //                DescriptorBuffer recognizedPatternsDescriptors = new DescriptorBuffer().AddTo(subscriptions);
            //                DescriptorBuffer markerPatternDescriptor = new DescriptorBuffer().AddTo(subscriptions);
            //                DescriptorMatchVector patternMatches = new DescriptorMatchVector().AddTo(subscriptions);
            //                Point2DfArray pattern2DPoints = new Point2DfArray().AddTo(subscriptions);
            //                Point2DfArray img2DPoints = new Point2DfArray().AddTo(subscriptions);
            //                Point3DfArray pattern3DPoints = new Point3DfArray().AddTo(subscriptions);

            //                bool marker_found = false;
            //                // Convert Image from RGB to grey
            //                imageConvertor.convert(image, greyImage, Image.ImageLayout.LAYOUT_GREY).Check();

            //                // Convert Image from grey to black and white
            //                imageFilterBinary.filter(greyImage, binaryImage);
            //                // Extract contours from binary image
            //                contoursExtractor.extract(binaryImage, contours);
            //                // Filter 4 edges contours to find those candidate for marker contours
            //                contoursFilter.filter(contours, filtered_contours);
            //                // Create one warpped and cropped image by contour
            //                perspectiveController.correct(binaryImage, filtered_contours, patches);
            //                // test if this last image is really a squared binary marker, and if it is the case, extract its descriptor
            //                if (patternDescriptorExtractor.extract(patches, filtered_contours, recognizedPatternsDescriptors, recognizedContours) != FrameworkReturnCode._ERROR_)
            //                {
            //                    // From extracted squared binary pattern, match the one corresponding to the squared binary marker
            //                    if (patternMatcher.match(markerPatternDescriptor, recognizedPatternsDescriptors, patternMatches) == IDescriptorMatcher.RetCode.DESCRIPTORS_MATCHER_OK)
            //                    {
            //                        // Reindex the pattern to create two vector of points, the first one corresponding to marker corner, the second one corresponding to the poitsn of the contour
            //                        patternReIndexer.reindex(recognizedContours, patternMatches, pattern2DPoints, img2DPoints);
            //                        // Compute the 3D position of each corner of the marker
            //                        img2worldMapper.map(pattern2DPoints, pattern3DPoints);
            //                        // Compute the pose of the camera using a Perspective n Points algorithm using only the 4 corners of the marker
            //                        if (pnp.estimate(img2DPoints, pattern3DPoints, pose) == FrameworkReturnCode._SUCCESS)
            //                        {
            //                            marker_found = true;
            //                        }
            //                    }
            //                }
            //                return marker_found;
            //            };

            //            if (detectFiducialMarker(camImage, poseFrame1))
            //            {
            //                keypointsDetector.detect(camImage, keypointsView1);
            //                descriptorExtractor.extract(camImage, keypointsView1, descriptorsView1);
            //                keyframe1 = new Datastructure.Keyframe(keypointsView1, descriptorsView1, camImage, poseFrame1);
            //                CloudPointVector tmpCP = new CloudPointVector();
            //                DescriptorMatchVector tmpDMV1 = new DescriptorMatchVector();
            //                DescriptorMatchVector tmpDMV2 = new DescriptorMatchVector();
            //                mapper.update(map, keyframe1, tmpCP, tmpDMV1, tmpDMV2);
            //                keyframePoses.Add(poseFrame1); // used for display
            //                kfRetriever.addKeyframe(keyframe1); // add keyframe for reloc
            //            }

            //            bool bootstrapOk = false;
            //            while (!bootstrapOk)
            //            {
            //                if (camera.getNextImage(view2) == FrameworkReturnCode._ERROR_)
            //                    break;

            //                if (!detectFiducialMarker(view2, poseFrame2))
            //                {
            //                    if (imageViewer.display(view2) == FrameworkReturnCode._STOP)
            //                        return FrameworkReturnCode._ERROR_;
            //                    continue;
            //                }
            //                float disTwoKeyframes = (float)Math.Sqrt(Math.Pow(poseFrame1.translation().coeff(0, 0) - poseFrame2.translation().coeff(0, 0), 2.0f) + Math.Pow(poseFrame1.translation().coeff(1, 0) - poseFrame2.translation().coeff(1, 0), 2.0f) +
            //                    Math.Pow(poseFrame1.translation().coeff(2, 0) - poseFrame2.translation().coeff(2, 0), 2.0f));

            //                if (disTwoKeyframes < 0.1)
            //                {
            //                    if (imageViewer.display(view2) == FrameworkReturnCode._STOP)
            //                        return FrameworkReturnCode._STOP;
            //                    continue;
            //                }

            //                keypointsDetector.detect(view2, keypointsView2);
            //                descriptorExtractor.extract(view2, keypointsView2, descriptorsView2);
            //                Frame frame2 = new Frame(keypointsView2, descriptorsView2, view2, keyframe1);
            //                matcher.match(descriptorsView1, descriptorsView2, matches);
            //                int nbOriginalMatches = matches.Count;
            //                matchesFilter.filter(matches, matches, keypointsView1, keypointsView2);

            //                matchesOverlay.draw(view2, imageMatches, keypointsView1, keypointsView2, matches);
            //                if (imageViewer.display(imageMatches) == FrameworkReturnCode._STOP)
            //                    return FrameworkReturnCode._STOP;

            //                if (keyframeSelector.select(frame2, matches))
            //                {
            //                    frame2.setPose(poseFrame2);
            //                    // Triangulate
            //                    keyframe2 = new Datastructure.Keyframe(frame2);
            //                    triangulator.triangulate(keyframe2, matches, cloud);
            //                    //double reproj_error = triangulator->triangulate(keypointsView1, keypointsView2, matches, std::make_pair(0, 1), poseFrame1, poseFrame2, cloud);
            //                    mapFilter.filter(poseFrame1, poseFrame2, cloud, filteredCloud);
            //                    keyframePoses.Add(poseFrame2); // used for display
            //                    DescriptorMatchVector tmpDMV = new DescriptorMatchVector();
            //                    mapper.update(map, keyframe2, filteredCloud, matches, tmpDMV);
            //                    kfRetriever.addKeyframe(keyframe2); // add keyframe for reloc
            //                    if (bundling)
            //                    {
            //                        IntVector firstIdxKFs = new IntVector() { 0, 1 };
            //                        localBundleAdjuster(firstIdxKFs, bundleReprojError);
            //                    }
            //                    bootstrapOk = true;
            //                }
            //            }
            //            Action<Datastructure.Keyframe, CloudPointVector, UIntVector, Datastructure.Keyframe, Frame> updateData =
            //            (Datastructure.Keyframe refKf, CloudPointVector localMap, UIntVector idxLocalMap, Datastructure.Keyframe referenceKeyframe, Frame frameToTrack) =>
            //            {
            //                referenceKeyframe = refKf;
            //                frameToTrack = new Frame(referenceKeyframe);
            //                frameToTrack.setReferenceKeyframe(referenceKeyframe);
            //                idxLocalMap.Clear();
            //                localMap.Clear();
            //                mapper.getLocalMapIndex(referenceKeyframe, idxLocalMap);
            //                foreach (var it in idxLocalMap)
            //                    localMap.Add(mapper.getGlobalMap().getAPoint((int)it));
            //            };

            //            // check need to make a new keyframe based on all existed keyframes
            //            Func<Frame, bool> checkNeedNewKfWithAllKfs = (Frame newFrame)
            //             =>
            //            {
            //                KeyframeList ret_keyframes = new KeyframeList();
            //                if (kfRetriever.retrieve(newFrame, ret_keyframes) == FrameworkReturnCode._SUCCESS)
            //                {
            //                    if (ret_keyframes[0].m_idx != referenceKeyframe.m_idx)
            //                    {
            //                        updatedRefKf = ret_keyframes[0];
            //                        return true;
            //                    }
            //                    return false;
            //                }
            //                else
            //                    return false;
            //            };

            //            // checkDisparityDistance
            //            Func<Frame, bool> checkDisparityDistance = (Frame newFrame)
            //             =>
            //            {
            //                CloudPointVector cloudPoint = map.getPointCloud();
            //                KeypointArray refKeypoints = referenceKeyframe.getKeypoints();
            //                MapIntInt refMapVisibility = referenceKeyframe.getVisibleMapPoints();
            //                CloudPointVector cpRef = new CloudPointVector();
            //                Point2DfArray projected2DPts = new Point2DfArray();
            //                Point2DfArray ref2DPts = new Point2DfArray();

            //                foreach (var it in refMapVisibility)
            //                {
            //                    cpRef.Add(cloudPoint[(int)it.Value]);
            //                    ref2DPts.Add(new Point2Df(refKeypoints[(int)it.Key].getX(), refKeypoints[(int)it.Key].getY()));
            //                }
            //                projector.project(cpRef, projected2DPts, newFrame.getPose());

            //                uint imageWidth = newFrame.getView().getWidth();
            //                double totalMatchesDist = 0.0;
            //                for (int i = 0; i < projected2DPts.Count; i++)
            //                {
            //                    Point2Df pt1 = ref2DPts[i];
            //                    Point2Df pt2 = projected2DPts[i];

            //                    totalMatchesDist += Math.Sqrt(((pt1.getX() - pt2.getX()) * (pt1.getX() - pt2.getX())) + ((pt1.getY() - pt2.getY()) * (pt1.getY() - pt2.getY()))) / imageWidth;
            //                }
            //                double meanMatchesDist = totalMatchesDist / projected2DPts.Count;
            //                return (meanMatchesDist > 0.07);
            //            };

            //            Action<Datastructure.Keyframe> updateAssociateCloudPoint = (Datastructure.Keyframe newKf) =>
            //            {
            //                MapIntInt newkf_mapVisibility = newKf.getVisibleMapPoints();
            //                MapIntInt kfCounter = new MapIntInt();
            //                foreach (var it in newkf_mapVisibility)
            //                {
            //                    CloudPoint cp = map.getAPoint((int)it.Value);
            //                    // calculate the number of connections to other keyframes
            //                    MapIntInt cpKfVisibility = cp.getVisibility();
            //                    foreach (var it_kf in cpKfVisibility)
            //                        kfCounter[it_kf.Value]++;
            //                    ///// update descriptor of cp: des_cp = ((des_cp * cp.getVisibility().size()) + des_buf) / (cp.getVisibility().size() + 1)
            //                    //// TO DO
            //                    cp.visibilityAddKeypoint((uint)newKf.m_idx, it.Key);
            //                }

            //                foreach (var it in kfCounter)
            //                    if ((it.Key != newKf.m_idx) && (it.Value > 20))
            //                    {
            //                        newKf.addNeighborKeyframe(it.Key, it.Value);
            //                    }
            //            };

            //            Action<Datastructure.Keyframe, IntVector, List<Tuple<uint, int, uint>>, CloudPointVector> findMatchesAndTriangulation =
            //                                           (Datastructure.Keyframe newKf, IntVector idxBestNeighborKfs,
            //                                           List<Tuple<uint, int, uint>> infoMatches, CloudPointVector cloudPoint) =>
            //            {
            //                MapIntInt newKf_mapVisibility = newKf.getVisibleMapPoints();
            //                DescriptorBuffer newKf_des = newKf.getDescriptors();
            //                KeypointArray newKf_kp = newKf.getKeypoints();
            //                Transform3Df newKf_pose = newKf.getPose();

            //                bool[] checkMatches = new bool[newKf_kp.Capacity];

            //                for (uint i = 0; i < newKf_kp.Count; ++i)
            //                    if (newKf_mapVisibility[i] != newKf_mapVisibility.Count)
            //                    {
            //                        checkMatches[i] = true;
            //                    }

            //                for (int i = 0; i < idxBestNeighborKfs.Count; ++i)
            //                {
            //                    IntVector newKf_indexKeypoints = new IntVector();
            //                    for (int j = 0; j < checkMatches.Length; ++j)
            //                        if (!checkMatches[j])
            //                            newKf_indexKeypoints.Add(j);

            //                    // get neighbor keyframe i
            //                    Datastructure.Keyframe tmpKf = mapper.getKeyframe(idxBestNeighborKfs[i]);
            //                    Transform3Df tmpPose = tmpKf.getPose();

            //                    // check distance between two keyframes
            //                    double distPose = Math.Sqrt(Math.Pow(newKf_pose.translation().coeff(0, 0) - tmpPose.translation().coeff(0, 0), 2.0f) + Math.Pow(newKf_pose.translation().coeff(0, 1)
            //                        - tmpPose.translation().coeff(0, 1), 2.0f) + Math.Pow(newKf_pose.translation().coeff(0, 2) - tmpPose.translation().coeff(0, 2), 2.0f));
            //                    if (distPose < 0.05)
            //                        continue;

            //                    // Matching based on BoW
            //                    DescriptorMatchVector tmpMatches = new DescriptorMatchVector();
            //                    DescriptorMatchVector goodMatches = new DescriptorMatchVector();

            //                    kfRetriever.match(newKf_indexKeypoints, newKf_des, idxBestNeighborKfs[i], tmpMatches);

            //                    // matches filter based epipolar lines
            //                    matchesFilter.filter(tmpMatches, tmpMatches, newKf_kp, tmpKf.getKeypoints(), newKf.getPose(), tmpKf.getPose(), camera.getIntrinsicsParameters());

            //                    // find info to triangulate				
            //                    List<Tuple<uint, int, uint>> tmpInfoMatches = new List<Tuple<uint, int, uint>>();
            //                    MapIntInt tmpMapVisibility = tmpKf.getVisibleMapPoints();
            //                    for (int j = 0; j < tmpMatches.Capacity; ++j)
            //                    {
            //                        uint idx_newKf = tmpMatches[j].getIndexInDescriptorA();
            //                        uint idx_tmpKf = tmpMatches[j].getIndexInDescriptorB();
            //                        if ((!checkMatches[idx_newKf]) && !(tmpMapVisibility.Keys.Contains(idx_tmpKf)))
            //                        {
            //                            tmpInfoMatches.Add(new Tuple<uint, int, uint>(idx_newKf, idxBestNeighborKfs[i], idx_tmpKf));
            //                            goodMatches.Add(tmpMatches[j]);
            //                        }
            //                    }

            //                    // triangulation
            //                    CloudPointVector tmpCloudPoint = new CloudPointVector();
            //                    CloudPointVector tmpFilteredCloudPoint = new CloudPointVector();

            //                    IntVector indexFiltered = new IntVector();
            //                    if (goodMatches.Count > 0)
            //                        triangulator.triangulate(newKf_kp, tmpKf.getKeypoints(), newKf_des, tmpKf.getDescriptors(), goodMatches,
            //                            new PairUIntUInt((uint)newKf.m_idx, (uint)idxBestNeighborKfs[i]), newKf.getPose(), tmpKf.getPose(), tmpCloudPoint);

            //                    // filter cloud points
            //                    if (tmpCloudPoint.Count > 0)
            //                        mapFilter.filter(newKf.getPose(), tmpKf.getPose(), tmpCloudPoint, tmpFilteredCloudPoint, indexFiltered);
            //                    for (int o = 0; o < indexFiltered.Count; ++o)
            //                    {
            //                        checkMatches[(tmpInfoMatches[indexFiltered[o]]).Item1] = true;
            //                        infoMatches.Add(tmpInfoMatches[indexFiltered[o]]);
            //                        cloudPoint.Add(tmpFilteredCloudPoint[o]);
            //                    }
            //                }
            //            };

            //            Action<Datastructure.Keyframe, UIntVector, List<Tuple<uint, int, uint>>, CloudPointVector> fuseCloudPoint =
            //                (Datastructure.Keyframe newKeyframe, UIntVector idxNeigborKfs, List<Tuple<uint, int, uint>> infoMatches, CloudPointVector newCloudPoint)
            //                =>
            //                {
            //                    bool[] checkMatches = new bool[newCloudPoint.Capacity];
            //                    for (int i = 0; i < checkMatches.Length; i++)
            //                        checkMatches[i] = true;

            //                    DescriptorBufferList desNewCloudPoint = new DescriptorBufferList();

            //                    foreach (var it_cp in newCloudPoint)
            //                    {
            //                        desNewCloudPoint.Add(it_cp.getDescriptor());
            //                    }

            //                    for (int i = 0; i < idxNeigborKfs.Count; ++i)
            //                    {
            //                        // get a neighbor
            //                        Datastructure.Keyframe neighborKf = mapper.getKeyframe((int)idxNeigborKfs[i]);
            //                        MapIntInt mapVisibilitiesNeighbor = neighborKf.getVisibleMapPoints();

            //                        //  projection points
            //                        Point2DfArray projected2DPts = new Point2DfArray();
            //                        projector.project(newCloudPoint, projected2DPts, neighborKf.getPose());

            //                        DescriptorMatchVector allMatches = new DescriptorMatchVector();
            //                        matcher.matchInRegion(projected2DPts, desNewCloudPoint, neighborKf, allMatches, 5.0f);

            //                        for (int j = 0; j < allMatches.Count; ++j)
            //                        {
            //                            int idxNewCloudPoint = (int)allMatches[j].getIndexInDescriptorA();
            //                            int idxKpNeighbor = (int)allMatches[j].getIndexInDescriptorB();
            //                            if (!checkMatches[idxNewCloudPoint])
            //                                continue;
            //                            Tuple<uint, int, uint> infoMatch = infoMatches[idxNewCloudPoint];

            //                            // check this cloud point is created from the same neighbor keyframe
            //                            if (infoMatch.Item2 == idxNeigborKfs[i])
            //                                continue;

            //                            // check if have a cloud point in the neighbor keyframe is coincide with this cloud point.
            //                            bool it_cp = mapVisibilitiesNeighbor.ContainsKey((uint)idxKpNeighbor);
            //                            if (it_cp)
            //                            {
            //                                // fuse
            //                                CloudPoint old_cp = map.getAPoint((int)mapVisibilitiesNeighbor[(uint)idxKpNeighbor]);
            //                                old_cp.visibilityAddKeypoint((uint)newKeyframe.m_idx, infoMatch.Item1);
            //                                old_cp.visibilityAddKeypoint((uint)infoMatch.Item2, infoMatch.Item3);

            //                                newKeyframe.addVisibleMapPoint(infoMatch.Item1, mapVisibilitiesNeighbor[(uint)idxKpNeighbor]);
            //                                mapper.getKeyframe(infoMatch.Item2).addVisibleMapPoint(infoMatch.Item3, mapVisibilitiesNeighbor[(uint)idxKpNeighbor]);

            //                                checkMatches[idxNewCloudPoint] = false;
            //                            }
            //                        }
            //                    }
            //                    List<Tuple<uint, int, uint>> tmpInfoMatches = new List<Tuple<uint, int, uint>>();
            //                    CloudPointVector tmpNewCloudPoint = new CloudPointVector();
            //                    for (int i = 0; i < checkMatches.Length; ++i)
            //                        if (checkMatches[i])
            //                        {
            //                            tmpInfoMatches.Add(infoMatches[i]);
            //                            tmpNewCloudPoint.Add(newCloudPoint[i]);
            //                        }
            //                    infoMatches = tmpInfoMatches;
            //                    newCloudPoint = tmpNewCloudPoint;
            //                };

            //            Func<Frame, Frame> processNewKeyframe = (Frame newFrame) =>
            //            {
            //                // create a new keyframe from the current frame
            //                Datastructure.Keyframe newKeyframe = new Datastructure.Keyframe(newFrame);
            //                // Add to BOW retrieval			
            //                kfRetriever.addKeyframe(newKeyframe);
            //                // Update keypoint visibility, descriptor in cloud point and connections between new keyframe with other keyframes
            //                updateAssociateCloudPoint(newKeyframe);
            //                // get best neighbor keyframes
            //                UIntVector idxBestNeighborKfs = newKeyframe.getBestNeighborKeyframes(4);
            //                IntVector tmp_idxBestNeighborKfs = new IntVector();
            //                for (int i = 0; i < idxBestNeighborKfs.Count; ++i)
            //                    tmp_idxBestNeighborKfs[i] = (int)idxBestNeighborKfs[i];
            //                // find matches between unmatching keypoints in the new keyframe and the best neighboring keyframes
            //                List<Tuple<uint, int, uint>> infoMatches = new List<Tuple<uint, int, uint>>(); // first: index of kp in newKf, second: index of Kf, third: index of kp in Kf.
            //                CloudPointVector newCloudPoint = new CloudPointVector();
            //                findMatchesAndTriangulation(newKeyframe, tmp_idxBestNeighborKfs, infoMatches, newCloudPoint);
            //                if (newCloudPoint.Count > 0)
            //                {
            //                    // fuse duplicate points
            //                    UIntVector idxNeigborKfs = newKeyframe.getBestNeighborKeyframes(10);
            //                    fuseCloudPoint(newKeyframe, idxNeigborKfs, infoMatches, newCloudPoint);
            //                }
            //                // mapper update
            //                mapper.update(map, newKeyframe, newCloudPoint, infoMatches);
            //                return newKeyframe;
            //            };

            //            // Prepare for tracking
            //            lastPose = poseFrame2;
            //            updateData(keyframe2, localMap, idxLocalMap, referenceKeyframe, frameToTrack);

            //            // Get current image
            //            camera.getNextImage(view);
            //            keypointsDetector.detect(view, keypointsView);
            //            descriptorExtractor.extract(view, keypointsView, descriptorsView);
            //            newFrame = new Frame(keypointsView, descriptorsView, view, referenceKeyframe);
            //            // match current keypoints with the keypoints of the Keyframe
            //            matcher.match(frameToTrack.getDescriptors(), descriptorsView, matches);
            //            matchesFilter.filter(matches, matches, frameToTrack.getKeypoints(), keypointsView);

            //            Point2DfArray pt2d = new Point2DfArray();
            //            Point3DfArray pt3d = new Point3DfArray();
            //            CloudPointVector foundPoints = new CloudPointVector();
            //            DescriptorMatchVector foundMatches = new DescriptorMatchVector();
            //            DescriptorMatchVector remainingMatches = new DescriptorMatchVector();
            //            corr2D3DFinder.find(frameToTrack, newFrame, matches, map, pt3d, pt2d, foundMatches, remainingMatches);
            //            // display matches
            //            imageMatches = view.copy();

            //            Point2DfArray imagePoints_inliers = new Point2DfArray();
            //            Point3DfArray worldPoints_inliers = new Point3DfArray();
            //            if (pnpRansac.estimate(pt2d, pt3d, imagePoints_inliers, worldPoints_inliers, newFramePose, lastPose) == FrameworkReturnCode._SUCCESS)
            //            {
            //                // Set the pose of the new frame
            //                newFrame.setPose(newFramePose);

            //                // refine pose and update map visibility of frame
            //                {
            //                    // get all keypoints of the new frame
            //                    KeypointArray keypoints = newFrame.getKeypoints();

            //                    //  projection points
            //                    Point2DfArray projected2DPts = new Point2DfArray();
            //                    projector.project(localMap, projected2DPts, newFrame.getPose());

            //                    DescriptorBufferList desAllLocalMap = new DescriptorBufferList();
            //                    foreach (var it_cp in localMap)
            //                    {
            //                        desAllLocalMap.Add(it_cp.getDescriptor());
            //                    }

            //                    // matches feature in region
            //                    DescriptorMatchVector allMatches = new DescriptorMatchVector();
            //                    matcher.matchInRegion(projected2DPts, desAllLocalMap, newFrame, allMatches, 5.0f);

            //                    Point2DfArray _pt2d = new Point2DfArray();
            //                    Point3DfArray _pt3d = new Point3DfArray();
            //                    MapIntInt newMapVisibility = new MapIntInt();


            //                    foreach (var it_match in allMatches)
            //                    {
            //                        int idx_2d = (int) it_match.getIndexInDescriptorB();
            //                        int idx_3d = (int) it_match.getIndexInDescriptorA();
            //                        _pt2d.Add(new Point2Df(keypoints[idx_2d].getX(), keypoints[idx_2d].getY()));
            //                        _pt3d.Add(new Point3Df(localMap[idx_3d].getX(), localMap[idx_3d].getY(), localMap[idx_3d].getZ()));
            //                        newMapVisibility[(uint)idx_2d] = idxLocalMap[idx_3d];
            //                    }

            //                    // pnp optimization
            //                    Transform3Df refinedPose = new Transform3Df();
            //                    pnp.estimate(pt2d, pt3d, refinedPose, newFrame.getPose());
            //                    newFrame.setPose(refinedPose);

            //                    // update map visibility of current frame
            //                    newFrame.addVisibleMapPoints(newMapVisibility);
            //                    overlay2D.drawCircles(pt2d, imageMatches);
            //                    overlay3D.draw(refinedPose, imageMatches);
            //                }
            //                lastPose = newFrame.getPose();

            //                // check need new keyframe
            //                if (checkDisparityDistance(newFrame))
            //                {
            //                    if (checkNeedNewKfWithAllKfs(newFrame))
            //                    {
            //                        updateData(updatedRefKf, localMap, idxLocalMap, referenceKeyframe, frameToTrack);
            //                    }
            //                    else
            //                    {
            //                        Datastructure.Keyframe newKeyframe = new Datastructure.Keyframe(processNewKeyframe(newFrame));
            //                        if (bundling)
            //                        {
            //                            // get current keyframe idx
            //                            int currentIdxKF = mapper.getKeyframes()[mapper.getKeyframes().Capacity - 1].m_idx;
            //                            // get keyfram connected graph
            //                            UIntVector bestIdx = mapper.getKeyframes()[currentIdxKF].getBestNeighborKeyframes(2);
            //                            // define 2 best keyframes + current keyframe
            //                            windowIdxBundling = new IntVector() {(int)(bestIdx[0]),(int)(bestIdx[1]),currentIdxKF };
            //                            //	windowIdxBundling = { currentIdxKF - 1, currentIdxKF }; // temporal sliding window
            //                            localBundleAdjuster(windowIdxBundling, bundleReprojError);
            //                        }
            //                        // update data
            //                        updateData(newKeyframe, localMap, idxLocalMap, referenceKeyframe, frameToTrack);
            //                        // add keyframe pose to display
            //                        keyframePoses.Add(newKeyframe.getPose());
            //                    }
            //                }
            //                else
            //                {
            //                    // update frame to track
            //                    frameToTrack = newFrame;
            //                }

            //                framePoses.Add(newFrame.getPose()); // used for display

            //                isLostTrack = false;    // tracking is good

            //            }
            //            else
            //            {
            //                isLostTrack = true;     // lost tracking
            //                                        // reloc
            //                KeyframeList ret_keyframes = new KeyframeList();
            //                if (kfRetriever.retrieve(newFrame, ret_keyframes) == FrameworkReturnCode._SUCCESS)
            //                {
            //                    // update data
            //                    updateData(ret_keyframes[0], localMap, idxLocalMap, referenceKeyframe, frameToTrack);
            //                    lastPose = referenceKeyframe.getPose();
            //                }
            //            }

            //            // display matches and a cube on the fiducial marker
            //            if (imageViewer.display(imageMatches) == FrameworkReturnCode._STOP)
            //                return FrameworkReturnCode._STOP;

            //            // display point cloud
            //            if (viewer3DPoints.display(map.getPointCloud(), lastPose, keyframePoses, framePoses, localMap) == FrameworkReturnCode._STOP)
            //                return FrameworkReturnCode._STOP;

                        return FrameworkReturnCode._SUCCESS;
        }

        //    readonly IKeypointDetector keypointsDetector;
        //    readonly IDescriptorsExtractor descriptorExtractor;
        //    readonly IDescriptorMatcher matcher;
        //    readonly I3DTransformFinderFrom2D2D poseFinderFrom2D2D;
        //    readonly ITriangulator triangulator;
        //    readonly IMatchesFilter matchesFilter;
        //    readonly I3DTransformSACFinderFrom2D3D pnpRansac;
        //    readonly I3DTransformFinderFrom2D3D pnp;
        //    readonly I2D3DCorrespondencesFinder corr2D3DFinder;
        //    readonly IMapFilter mapFilter;
        //    readonly IMapper mapper;
        //    readonly IKeyframeSelector keyframeSelector;
        //    readonly IMatchesOverlay matchesOverlay;
        //    readonly IMatchesOverlay matchesOverlayBlue;
        //    readonly IMatchesOverlay matchesOverlayRed;
        //    readonly IImageViewer imageViewer;
        //    readonly I3DPointsViewer viewer3DPoints;
        //    readonly IKeyframeRetriever kfRetriever;
        //    readonly IProject projector;
        //    readonly IBundler bundler;
        //    readonly IMarker2DSquaredBinary binaryMarker;
        //    readonly IImageFilter imageFilterBinary;
        //    readonly IImageConvertor imageConvertor;
        //    readonly IContoursExtractor contoursExtractor;
        //    readonly IContoursFilter contoursFilter;
        //    readonly IPerspectiveController perspectiveController;
        //    readonly IDescriptorsExtractorSBPattern patternDescriptorExtractor;
        //    readonly IDescriptorMatcher patternMatcher;
        //    readonly ISBPatternReIndexer patternReIndexer;
        //    readonly IImage2WorldMapper img2worldMapper;
        //    readonly I3DOverlay overlay3D;
        //    readonly I2DOverlay overlay2D;

        //    readonly Image view1;
        //    readonly Image view2;
        //    readonly Image view;
        //    Datastructure.Keyframe keyframe1;
        //    Datastructure.Keyframe keyframe2;
        //    readonly KeypointArray keypointsView1;
        //    readonly KeypointArray keypointsView2;
        //    readonly Map map;
        //    readonly CloudPointVector localMap;
        //    readonly UIntVector idxLocalMap;
        //    bool isLostTrack;
        //    bool bundling;
        //    double bundleReprojError;
        //    IntVector windowIdxBundling;
        //    readonly DescriptorBuffer markerPatternDescriptor;
        //    readonly KeypointArray keypointsView;
        //    readonly DescriptorBuffer descriptorsView1;
        //    readonly DescriptorBuffer descriptorsView2;
        //    readonly DescriptorBuffer descriptorsView;
        //    readonly DescriptorMatchVector matches;
        //    readonly Transform3Df poseFrame1;
        //    readonly Transform3Df poseFrame2;
        //    readonly Transform3Df newFramePose;
        //    Transform3Df lastPose;
        //    readonly CloudPointVector cloud;
        //    readonly CloudPointVector filteredCloud;
        //    readonly Transform3DfList keyframePoses;
        //    readonly Transform3DfList framePoses;
        //    Frame newFrame;
        //    Frame frameToTrack;
        //    Datastructure.Keyframe referenceKeyframe;
        //    Datastructure.Keyframe updatedRefKf;
        //    Image imageMatches;
        //    Image imageMatches2;
    }
}
