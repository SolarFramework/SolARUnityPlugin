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
using SolAR.Api.Input.Devices;
using SolAR.Api.Loop;
using SolAR.Api.Reloc;
using SolAR.Api.Slam;
using SolAR.Api.Solver.Map;
using SolAR.Api.Solver.Pose;
using SolAR.Api.Storage;
using SolAR.Core;
using SolAR.Datastructure;
using XPCF.Api;
using Keyframe = SolAR.Datastructure.Keyframe;

namespace SolAR.Expert.Samples
{
    public class SlamSample : AbstractSample
    {
        public SlamSample(IComponentManager xpcfComponentManager) : base(xpcfComponentManager)
        {
            // declare and create components
            LOG_INFO("Start creating components");
            LOG_INFO("Resolving camera ");
            camera = Resolve<ICamera>();
            LOG_INFO("Resolving point cloud manager");
            pointCloudManager = Resolve<IPointCloudManager>();
            LOG_INFO("Resolving key frames manager");
            keyframesManager = Resolve<IKeyframesManager>();
            LOG_INFO("Resolving covisibility graph");
            covisibilityGraph = Resolve<ICovisibilityGraph>();
            LOG_INFO("Resolving key frame retriever");
            keyframeRetriever = Resolve<IKeyframeRetriever>();
            LOG_INFO("Resolving key mapper");
            mapper = Resolve<IMapper>();
            LOG_INFO("Resolving key points detector");
            keypointsDetector = Resolve<IKeypointDetector>();
            LOG_INFO("Resolving descriptor extractor");
            descriptorExtractor = Resolve<IDescriptorsExtractor>();
            LOG_INFO("Resolving image viewer");
            imageViewer = Resolve<IImageViewer>();
            LOG_INFO("Resolving viewer3D points");
            viewer3DPoints = Resolve<I3DPointsViewer>();
            LOG_INFO("Resolving loop detector");
            loopDetector = Resolve<ILoopClosureDetector>();
            LOG_INFO("Resolving loop corrector");
            loopCorrector = Resolve<ILoopCorrector>();
            LOG_INFO("Resolving 3D overlay");
            overlay3D = Resolve<I3DOverlay>();
            LOG_INFO("Resolving Fiducial marker pose");
            fiducialMarkerPoseEstimator = Resolve<IFiducialMarkerPose>();
            LOG_INFO("Resolving bundle adjustment");
            bundler = Resolve<IBundler>();
            LOG_INFO("Resolving bootstrapper");
            bootstrapper = Resolve<IBootstrapper>();
            LOG_INFO("Resolving tracking");
            tracking = Resolve<ITracking>();
            LOG_INFO("Resolving mapping");
            mapping = Resolve<IMapping>();
            LOG_INFO("Loaded all components");
        }

        Matrix3x3f calibration; Vector5f distortion;
        public override void SetCameraParameters(Matrix3x3f calibration, Vector5f distortion)
        {
            this.calibration = calibration;
            this.distortion = distortion;
            // initialize pose estimation with the camera intrinsic parameters (please refer to the use of intrinsic parameters file)
            overlay3D.setCameraParameters(calibration, distortion);
            loopDetector.setCameraParameters(calibration, distortion);
            loopCorrector.setCameraParameters(calibration, distortion);
            fiducialMarkerPoseEstimator.setCameraParameters(calibration, distortion);
            bootstrapper.setCameraParameters(calibration, distortion);
            tracking.setCameraParameters(calibration, distortion);
            mapping.setCameraParameters(calibration, distortion);
            LOG_DEBUG("Intrincic parameters : \n {}", calibration);
            // get properties
            minWeightNeighbor = mapping.BindTo<IConfigurable>().getProperty("minWeightNeighbor").getFloatingValue();
            reprojErrorThreshold = mapper.BindTo<IConfigurable>().getProperty("reprojErrorThreshold").getFloatingValue();

            // Load map from file
            Keyframe keyframe2 = SharedPtr.Alloc<Keyframe>();
            if (mapper.loadFromFile() == FrameworkReturnCode._SUCCESS)
            {
                LOG_INFO("Load map done!");
                keyframesManager.getKeyframe(0, keyframe2);
            }
            else
            {
                LOG_INFO("Initialization from scratch");
                bool bootstrapOk = false;
                while (!bootstrapOk)
                {
                    Image image = SharedPtr.Alloc<Image>(), view = SharedPtr.Alloc<Image>();
                    camera.getNextImage(image);
                    Transform3Df pose = Transform3Df.Identity();
                    fiducialMarkerPoseEstimator.estimate(image, pose);
                    if (bootstrapper.process(image, view, pose) == FrameworkReturnCode._SUCCESS)
                    {
                        double bundleReprojError = bundler.bundleAdjustment(calibration, distortion);
                        bootstrapOk = true;
                    }
                    //if (!pose.isApprox(Transform3Df.Identity()))
                    //    overlay3D.draw(pose, view);
                    if (imageViewer.display(view) == FrameworkReturnCode._STOP)
                        return;
                }
                keyframesManager.getKeyframe(1, keyframe2);
            }

            LOG_INFO("Number of initial point cloud: {0}", pointCloudManager.getNbPoints());
            LOG_INFO("Number of initial keyframes: {0}", keyframesManager.getNbKeyframes());

            // Prepare for tracking
            tracking.updateReferenceKeyframe(keyframe2);

            // init display point cloud
            fnDisplay(new Transform3DfList() { keyframe2.getPose() });
        }

        // display point cloud function
        bool fnDisplay(Transform3DfList framePoses)
        {
            // get all keyframes and point cloud
            Transform3DfList keyframePoses = new Transform3DfList();
            KeyframeList allKeyframes = new KeyframeList();
            keyframesManager.getAllKeyframes(allKeyframes);
            foreach (var it in allKeyframes)
                keyframePoses.Add(it.getPose());
            CloudPointList pointCloud = new CloudPointList();
            pointCloudManager.getAllPoints(pointCloud);
            // display point cloud 
            if (viewer3DPoints.display(pointCloud, framePoses.Last(), keyframePoses, framePoses) == FrameworkReturnCode._STOP)
                return false;
            else
                return true;
        }

        public override FrameworkReturnCode Proceed(Image camImage, Transform3Df pose, ICamera camera)
        {
            Image view = SharedPtr.Alloc<Image>(), displayImage = SharedPtr.Alloc<Image>();
            KeypointArray keypoints = new KeypointArray();
            DescriptorBuffer descriptors = new DescriptorBuffer();
            Frame frame;
            Keyframe keyframe = new Keyframe();
            // Get current image
            if (camera.getNextImage(view) != FrameworkReturnCode._SUCCESS)
                return FrameworkReturnCode._STOP;
            // feature extraction
            keypointsDetector.detect(view, keypoints);
            LOG_DEBUG("Number of keypoints: {0}", keypoints.Count);
            if (keypoints.Count == 0)
                return FrameworkReturnCode._ERROR_;
            descriptorExtractor.extract(view, keypoints, descriptors);
            frame = new Frame(keypoints, descriptors, view);

            // tracking
            if (tracking.process(frame, displayImage) == FrameworkReturnCode._SUCCESS)
            {
                // used for display
                framePoses.Add(frame.getPose());
                // draw cube
                overlay3D.draw(frame.getPose(), displayImage);
                // mapping
                if (mapping.process(frame, keyframe) == FrameworkReturnCode._SUCCESS)
                {
                    LOG_DEBUG("New keyframe id: {}", keyframe.getId());
                    // Local bundle adjustment
                    UIntVector bestIdx = new UIntVector(), bestIdxToOptimize = new UIntVector();
                    covisibilityGraph.getNeighbors(keyframe.getId(), minWeightNeighbor, bestIdx);
                    if (bestIdx.Count < 10)
                    {
                        var tmp = bestIdxToOptimize;
                        bestIdxToOptimize = bestIdx;
                        bestIdx = tmp;
                    }
                    else
                    {
                        for (int i = 0; i < 10; ++i)
                        {
                            bestIdxToOptimize.Add(bestIdx[i]);
                        }
                    }
                    bestIdxToOptimize.Add(keyframe.getId());
                    LOG_DEBUG("Nb keyframe to local bundle: {0}", bestIdxToOptimize.Count);
                    double bundleReprojError = bundler.bundleAdjustment(calibration, distortion, bestIdxToOptimize);
                    // loop closure
                    countNewKeyframes++;
                    if (countNewKeyframes >= NB_NEWKEYFRAMES_LOOP)
                    {
                        Keyframe detectedLoopKeyframe = new Keyframe();
                        Transform3Df sim3Transform = new Transform3Df();
                        UIntPairVector duplicatedPointsIndices = new UIntPairVector();
                        if (loopDetector.detect(keyframe, detectedLoopKeyframe, sim3Transform, duplicatedPointsIndices) == FrameworkReturnCode._SUCCESS)
                        {
                            // detected loop keyframe
                            LOG_INFO("Detected loop keyframe id: {0}", detectedLoopKeyframe.getId());
                            // performs loop correction 
                            countNewKeyframes = 0;
                            loopCorrector.correct(keyframe, detectedLoopKeyframe, sim3Transform, duplicatedPointsIndices);
                            // Loop optimisation
                            bundler.bundleAdjustment(calibration, distortion);
                        }
                    }
                    // map pruning
                    mapper.pruning();
                }
                // update reference keyframe
                if (keyframe != null)
                {
                    tracking.updateReferenceKeyframe(keyframe);
                }
            }

            // display matches and a cube on the origin of coordinate system
            if (imageViewer.display(displayImage) == FrameworkReturnCode._STOP)
                return FrameworkReturnCode._STOP;

            // display point cloud
            if (!fnDisplay(framePoses))
                return FrameworkReturnCode._STOP;

            return FrameworkReturnCode._SUCCESS;
        }

        readonly ICamera camera;
        readonly IPointCloudManager pointCloudManager;
        readonly IKeyframesManager keyframesManager;
        readonly ICovisibilityGraph covisibilityGraph;
        readonly IKeyframeRetriever keyframeRetriever;
        readonly IMapper mapper;
        readonly IKeypointDetector keypointsDetector;
        readonly IDescriptorsExtractor descriptorExtractor;
        readonly IImageViewer imageViewer;
        readonly I3DPointsViewer viewer3DPoints;
        readonly ILoopClosureDetector loopDetector;
        readonly ILoopCorrector loopCorrector;
        readonly I3DOverlay overlay3D;
        readonly IFiducialMarkerPose fiducialMarkerPoseEstimator;
        readonly IBundler bundler;
        readonly IBootstrapper bootstrapper;
        readonly ITracking tracking;
        readonly IMapping mapping;

        readonly Transform3DfList framePoses = new Transform3DfList();
        float minWeightNeighbor;
        float reprojErrorThreshold;

        int countNewKeyframes;
        const int NB_NEWKEYFRAMES_LOOP = 0;
    }
}
