//#include "xpcf/module/ModuleFactory.h"
//#include "PipelineNaturalImageMarker.h"
//#include "core/Log.h"

//XPCF_DEFINE_FACTORY_CREATE_INSTANCE(SolAR.PIPELINES.PipelineNaturalImageMarker)

#define TRACKING

namespace SolAR
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using SolAR.Api.Sink;
    using SolAR.Api.Source;
    using SolAR.Api.Tracking;
    using SolAR.Core;
    using SolAR.Datastructure;
    using UniRx;
    using UnityEngine;
    using XPCF.Api;
    using XPCF.Core;
    namespace PIPELINES
    {
#pragma warning disable IDE1006 // Styles d'affectation de noms
        public abstract class ConfigurableBase : IDisposable
        {
            readonly CompositeDisposable subscriptions = new CompositeDisposable();
            readonly IComponentManager xpcfComponentManager;

            protected ConfigurableBase(IComponentManager xpcfComponentManager)
            {
                this.xpcfComponentManager = xpcfComponentManager;
            }

            protected void LOG_ERROR(string message, params object[] objects) { Debug.LogErrorFormat(message, objects); }
            protected void LOG_INFO(string message, params object[] objects) { Debug.LogWarningFormat(message, objects); }
            protected void LOG_DEBUG(string message, params object[] objects) { Debug.LogFormat(message, objects); }
            protected void LOG_WARNING(string message, params object[] objects) { Debug.LogWarningFormat(message, objects); }

            protected void declareInterface<I>(object o) { Debug.Log(new { o }); }

            protected void declareInjectable<I>(out I injectable, string name = null)
                where I : IComponentIntrospect, IDisposable
            {
                injectable = xpcfComponentManager.Resolve<I>().AddTo(subscriptions).BindTo<I>().AddTo(subscriptions);
                //_xpcfComponents.Add(component);
            }

            protected void declareProperty(string name, object o) { Debug.Log(new { name, o }); }

            public void Dispose()
            {
                subscriptions.Dispose();
            }

            protected ConfigurableBase() { }
        }
#pragma warning restore IDE1006 // Styles d'affectation de noms
#pragma warning disable IDE1006 // Styles d'affectation de noms
        public class PipelineNaturalImageMarker : ConfigurableBase
        {
            public PipelineNaturalImageMarker(IComponentManager xpcfComponentManager) : base(/*xpcf.toUUID<PipelineNaturalImageMarker>*/)
            {
                declareInterface<Api.Pipeline.IPoseEstimationPipeline>(this);
                declareInjectable(out m_camera);
                declareInjectable(out m_kpDetector);
                declareInjectable(out m_naturalImagemarker);
                declareInjectable(out m_geomMatchesFilter);
                declareInjectable(out m_matcher);
                declareInjectable(out m_descriptorExtractor);
                declareInjectable(out m_basicMatchesFilter, "basicMatchesFilter");
                declareInjectable(out m_img_mapper);
                declareInjectable(out m_keypointsReindexer);
                declareInjectable(out m_sink);
                declareInjectable(out m_source);
                declareInjectable(out m_imageConvertorUnity, "imageConvertorUnity");
                declareInjectable(out m_kpDetectorRegion);
                declareInjectable(out m_projection);
                declareInjectable(out m_unprojection);
                declareInjectable(out m_poseEstimationPlanar);
                declareInjectable(out m_opticalFlowEstimator);

                declareProperty("updateTrackedPointThreshold", m_updateTrackedPointThreshold);
                declareProperty("detectionMatchesNumberThreshold", m_detectionMatchesNumberThreshold);

                m_initOK = false;
                m_startedOK = false;
                m_stopFlag = false;
                m_haveToBeFlip = false;
                m_taskGetCameraImages = null;
                m_taskDetection = null;
                m_taskTracking = null;
                LOG_DEBUG(" Pipeline constructor");
            }

            ~PipelineNaturalImageMarker()
            {
                if (m_taskDetection != null)
                    m_taskDetection.Abort();
                LOG_DEBUG(" Pipeline destructor");
            }

            void onInjected()
            {
                LOG_DEBUG(" Pipeline injectables injected and configured !!");
            }

            XPCFErrorCode onConfigured()
            {
                LOG_DEBUG(" Pipeline configured !!");
                return XPCFErrorCode._SUCCESS;
            }

            FrameworkReturnCode init(IComponentManager xpcfComponentManager)
            {
                // load marker
                LOG_INFO("LOAD MARKER IMAGE ");
                m_naturalImagemarker.loadMarker();
                m_naturalImagemarker.getImage(m_refImage);
                m_naturalImagemarker.getWorldCorners(m_markerWorldCorners);


                // detect keypoints in reference image
                LOG_INFO("DETECT MARKER KEYPOINTS ");
                m_kpDetector.detect(m_refImage, m_refKeypoints);

                // extract descriptors in reference image
                LOG_INFO("EXTRACT MARKER DESCRIPTORS ");
                m_descriptorExtractor.extract(m_refImage, m_refKeypoints, m_refDescriptors);
                LOG_INFO("EXTRACT MARKER DESCRIPTORS COMPUTED");

                // initialize image mapper with the reference image size and marker size

                LOG_INFO(" worldWidth : {} worldHeight : {} \n", m_naturalImagemarker.getSize().width, m_naturalImagemarker.getSize().height);


                m_img_mapper.BindTo<IConfigurable>().getProperty("digitalWidth").setIntegerValue((int)m_refImage.getSize().width);
                m_img_mapper.BindTo<IConfigurable>().getProperty("digitalHeight").setIntegerValue((int)m_refImage.getSize().height);
                m_img_mapper.BindTo<IConfigurable>().getProperty("worldWidth").setFloatingValue(m_naturalImagemarker.getSize().width);
                m_img_mapper.BindTo<IConfigurable>().getProperty("worldHeight").setFloatingValue(m_naturalImagemarker.getSize().height);


                Point2Df corner0 = new Point2Df(0, 0);
                Point2Df corner1 = new Point2Df((float)m_refImage.getWidth(), 0);
                Point2Df corner2 = new Point2Df((float)m_refImage.getWidth(), (float)m_refImage.getHeight());
                Point2Df corner3 = new Point2Df(0, (float)m_refImage.getHeight());
                m_refImgCorners.Add(corner0);
                m_refImgCorners.Add(corner1);
                m_refImgCorners.Add(corner2);
                m_refImgCorners.Add(corner3);

                //for (int i = 0; i < 4; i++)
                //    for (int j = 0; j < 4; j++)
                //        m_pose(i, j) = 0.f;

                // initialize pose estimation based on planar points with the camera intrinsec parameters (please refeer to the use of intrinsec parameters file)
                m_poseEstimationPlanar.setCameraParameters(m_camera.getIntrinsicsParameters(), m_camera.getDistortionParameters());

                // initialize projection component with the camera intrinsec parameters (please refeer to the use of intrinsec parameters file)
                m_projection.setCameraParameters(m_camera.getIntrinsicsParameters(), m_camera.getDistortionParameters());

                // initialize unprojection component with the camera intrinsec parameters (please refeer to the use of intrinsec parameters file)
                m_unprojection.setCameraParameters(m_camera.getIntrinsicsParameters(), m_camera.getDistortionParameters());

                m_isTrack = false;
                m_needNewTrackedPoints = false;

                m_initOK = true;
                return FrameworkReturnCode._SUCCESS;
            }

            CameraParameters getCameraParameters()
            {
                CameraParameters camParam = null;
                if (m_camera != null)
                {
                    camParam = m_camera.getParameters();
                }
                return camParam;
            }


            // get images from camera

            void getCameraImages()
            {

                Image view = SharedPtr.Alloc<Image>();
                if (m_stopFlag || !m_initOK || !m_startedOK)
                    return;

                if (m_haveToBeFlip)
                {
                    if (m_source.getNextImage(view) != SourceReturnCode._NEW_IMAGE)
                    {
                        m_stopFlag = true;
                        return;
                    }
                    m_imageConvertorUnity.convert(view, view, Image.ImageLayout.LAYOUT_RGB);
                }
                else if (m_camera.getNextImage(view) == FrameworkReturnCode._ERROR_LOAD_IMAGE)
                {
                    m_stopFlag = true;
                    return;
                }

                if (m_CameraImagesForDetection.Count == 0)
                    m_CameraImagesForDetection.Push(view);

                if (m_CameraImagesForTracking.Count == 0)
                    m_CameraImagesForTracking.Push(view);

                return;
            }

            bool processDetection()
            {
                if (m_stopFlag || !m_initOK || !m_startedOK)
                    return false;

                bool valid_pose = false;

                Image camImage = SharedPtr.Alloc<Image>();
                UIntVector inliers = new UIntVector();
                Transform3Df pose = new Transform3Df();
                KeypointArray camKeypoints = new KeypointArray();  // where to store detected keypoints in ref image and camera image
                DescriptorMatchVector matches = new DescriptorMatchVector();
                DescriptorBuffer camDescriptors = new DescriptorBuffer();

                if (null == (camImage = m_CameraImagesForDetection.Pop()))
                    return false;

                // detect keypoints in camera image
                m_kpDetector.detect(camImage, camKeypoints);

                /* extract descriptors in camera image*/
                m_descriptorExtractor.extract(camImage, camKeypoints, camDescriptors);

                /*compute matches between reference image and camera image*/
                m_matcher.match(m_refDescriptors, camDescriptors, matches);

                /* filter matches to remove redundancy and check geometric validity */
                m_basicMatchesFilter.filter(matches, matches, m_refKeypoints, camKeypoints);
                m_geomMatchesFilter.filter(matches, matches, m_refKeypoints, camKeypoints);

                Point2DfArray ref2Dpoints = new Point2DfArray();
                Point2DfArray cam2Dpoints = new Point2DfArray();
                Point3DfArray ref3Dpoints = new Point3DfArray();

                //List<Point2Df> markerCornersinCamImage;
                //List<Point3Df> markerCornersinWorld;


                //List<Point2Df> refMatched2Dpoints, camMatched2Dpoints;

                if (matches.Count > m_detectionMatchesNumberThreshold)
                {
                    // reindex the keypoints with established correspondence after the matching
                    m_keypointsReindexer.reindex(m_refKeypoints, camKeypoints, matches, ref2Dpoints, cam2Dpoints);

                    // mapping to 3D points
                    m_img_mapper.map(ref2Dpoints, ref3Dpoints);

                    // Estimate the pose from the 2D-3D planar correspondence
                    if (m_poseEstimationPlanar.estimate(cam2Dpoints, ref3Dpoints, inliers, pose) != FrameworkReturnCode._SUCCESS)
                    {
                        valid_pose = false;
                        LOG_DEBUG("No pose from Detection for this frame");
                    }
                    else
                    {
#if TRACKING
                        m_isTrack = true;
                        LOG_INFO("Start tracking");
#else
                        LOG_INFO("Valid pose");
#endif
                        valid_pose = true;
                    }
                }
#if TRACKING
                if (valid_pose)
                {
                    m_outBufferDetection.Push(Tuple.Create(camImage, pose, valid_pose));
                }
#else
                if (valid_pose)
                {
                    m_sink.set(pose, camImage);
                }
                else
                    m_sink.set(camImage);
#endif
                return true;

            }

            bool processTracking()
            {
                if (m_stopFlag || !m_initOK || !m_startedOK)
                    return false;

                bool valid_pose = false;

                m_needNewTrackedPoints = false;
                Image camImage;
                UIntVector inliers = new UIntVector();

                if (null == (camImage = m_CameraImagesForTracking.Pop()))
                    return false;

                Tuple<Image, Transform3Df, bool> getCameraPoseDetection;

                if (!m_isTrack)
                {
#if TRACKING
                    m_sink.set(camImage);
#endif
                    return true;
                }

                if (null == (getCameraPoseDetection = m_outBufferDetection.Pop()))
                {
                    Image detectedImage = getCameraPoseDetection.Item1;
                    Transform3Df detectedPose = getCameraPoseDetection.Item2;
                    bool isDetectedPose = getCameraPoseDetection.Item3;
                    if (isDetectedPose)
                    {
                        m_previousCamImage = detectedImage.copy();
                        m_pose = detectedPose;
                        m_needNewTrackedPoints = true;
                    }
                }


                if (m_needNewTrackedPoints)
                {
                    m_imagePoints_track.Clear();
                    m_worldPoints_track.Clear();
                    Point2DfArray projectedMarkerCorners = new Point2DfArray();
                    KeypointArray newKeypoints = new KeypointArray();
                    // Get the projection of the corner of the marker in the current image
                    m_projection.project(m_markerWorldCorners, projectedMarkerCorners, m_pose);

                    // Detect the keypoints within the contours of the marker defined by the projected corners
                    m_kpDetectorRegion.detect(m_previousCamImage, projectedMarkerCorners, newKeypoints);

                    if (newKeypoints.Count > m_updateTrackedPointThreshold)
                    {
                        foreach (var keypoint in newKeypoints)
                            m_imagePoints_track.Add(new Point2Df(keypoint.getX(), keypoint.getY()));

                        // get back the 3D positions of the detected keypoints in world space
                        m_unprojection.unproject(m_imagePoints_track, m_worldPoints_track, m_pose);
                        LOG_DEBUG("Reinitialize points to track");
                    }
                    else
                    {
                        m_isTrack = false;
                        LOG_DEBUG("Cannot reinitialize points to track");
                    }
                    m_needNewTrackedPoints = false;
                }

                if (!m_isTrack)
                {
                    LOG_INFO("Tracking lost");
                    m_sink.set(camImage);
                    return true;
                }

                Point2DfArray trackedPoints = new Point2DfArray(), pts2D = new Point2DfArray();
                Point3DfArray pts3D = new Point3DfArray();
                UCharList status = new UCharList();
                FloatList err = new FloatList();

                // tracking 2D-2D
                m_opticalFlowEstimator.estimate(m_previousCamImage, camImage, m_imagePoints_track, trackedPoints, status, err);

                for (int i = 0; i < status.Count; i++)
                {
                    if (status[i] != 0)
                    {
                        pts2D.Add(trackedPoints[i]);
                        pts3D.Add(m_worldPoints_track[i]);
                    }
                }

                // Estimate the pose from the 2D-3D planar correspondence
                if (m_poseEstimationPlanar.estimate(pts2D, pts3D, inliers, m_pose) != FrameworkReturnCode._SUCCESS)
                {
                    m_isTrack = false;
                    valid_pose = false;
                    m_needNewTrackedPoints = false;
                    LOG_INFO("Tracking lost");
                }
                else
                {
                    m_imagePoints_track.Clear();
                    m_worldPoints_track.Clear();
                    foreach (int index in inliers)
                    {
                        m_imagePoints_track.Add(pts2D[index]);
                        m_worldPoints_track.Add(pts3D[index]);
                    }
                    valid_pose = true;
                    m_previousCamImage = camImage.copy();
                    if (m_worldPoints_track.Count < m_updateTrackedPointThreshold)
                    {
                        m_needNewTrackedPoints = true;
                        LOG_DEBUG("Need new point to track");
                    }
                }

                if (valid_pose)
                {
                    m_sink.set(m_pose, camImage);
                    //        std.cout << m_pose.matrix() <<"\n";
                }
                else
                    m_sink.set(camImage);

                return true;
            }
            //////////////////////////////// ADD
            SourceReturnCode loadSourceImage(IntPtr sourceTextureHandle, int width, int height)
            {
                m_haveToBeFlip = true;
                return m_source.setInputTexture(sourceTextureHandle, width, height);
            }
            ////////////////////////////////////


            FrameworkReturnCode start(IntPtr imageDataBuffer)
            {
                if (m_initOK == false)
                {
                    LOG_WARNING("Try to start the Fiducial marker pipeline without initializing it");
                    return FrameworkReturnCode._ERROR_;
                }
                m_stopFlag = false;
                m_sink.setImageBuffer(imageDataBuffer);
                if (!m_haveToBeFlip && (m_camera.start() != FrameworkReturnCode._SUCCESS))
                {
                    LOG_ERROR("Camera cannot start");
                    return FrameworkReturnCode._ERROR_;
                }

                // create and start threads to process the images

                ThreadStart getCameraImagesThread = () =>
                {
                    while (true)
                    {
                        getCameraImages();
                        Thread.Yield();
                    }
                };
                m_taskGetCameraImages = new Thread(getCameraImagesThread);
                m_taskGetCameraImages.Start();

                ThreadStart processDetectionThread = () =>
                {
                    while (true)
                    {
                        processDetection();
                        Thread.Yield();
                    }
                };
                m_taskDetection = new Thread(processDetectionThread);
                m_taskDetection.Start();

#if TRACKING
                // create and start a thread to process the images
                ThreadStart processTrackingThread = () =>
                {
                    while (true)
                    {
                        processTracking();
                        Thread.Yield();
                    }
                };
                m_taskTracking = new Thread(processTrackingThread);
                m_taskTracking.Start();
#endif


                m_startedOK = true;
                return FrameworkReturnCode._SUCCESS;
            }

            FrameworkReturnCode stop()
            {
                m_stopFlag = true;
                if (m_taskGetCameraImages != null)
                    m_taskGetCameraImages.Abort();


                if (m_taskDetection != null)
                    m_taskDetection.Abort();
#if TRACKING
                if (m_taskTracking != null)
                    m_taskTracking.Abort();
#endif
                if (!m_initOK)
                {
                    LOG_WARNING("Try to stop a pipeline that has not been initialized");
                    return FrameworkReturnCode._ERROR_;
                }

                if (!m_startedOK)
                {
                    LOG_WARNING("Try to stop a pipeline that has not been started");
                    return FrameworkReturnCode._ERROR_;
                }

                LOG_INFO("Pipeline has stopped: \n");

                return FrameworkReturnCode._SUCCESS;
            }

            SinkReturnCode update(Transform3Df pose)
            {
                return m_sink.tryGet(pose);
            }

            readonly Api.Input.Devices.ICamera m_camera;
            readonly Api.Features.IKeypointDetector m_kpDetector;
            readonly Api.Input.Files.IMarker2DNaturalImage m_naturalImagemarker;
            readonly Api.Features.IMatchesFilter m_geomMatchesFilter;
            readonly Api.Features.IDescriptorMatcher m_matcher;
            readonly Api.Features.IDescriptorsExtractor m_descriptorExtractor;
            readonly Api.Features.IMatchesFilter m_basicMatchesFilter;
            readonly Api.Geom.IImage2WorldMapper m_img_mapper;
            readonly Api.Features.IKeypointsReIndexer m_keypointsReindexer;
            readonly Api.Sink.ISinkPoseImage m_sink;
            readonly Api.Source.ISourceImage m_source;
            readonly Api.Image.IImageConvertor m_imageConvertorUnity;
            readonly Api.Features.IKeypointDetectorRegion m_kpDetectorRegion;
            readonly Api.Geom.IProject m_projection;
            readonly Api.Geom.IUnproject m_unprojection;
            readonly Api.Solver.Pose.I3DTransformSACFinderFrom2D3D m_poseEstimationPlanar;
            readonly Api.Tracking.IOpticalFlowEstimator m_opticalFlowEstimator;

            const int m_updateTrackedPointThreshold = 3;
            const int m_detectionMatchesNumberThreshold = 5;

            bool m_initOK;
            bool m_startedOK;
            bool m_stopFlag;
            bool m_haveToBeFlip;
            Thread m_taskGetCameraImages;
            Thread m_taskDetection;
            Thread m_taskTracking;

            readonly Image m_refImage = SharedPtr.Alloc<Image>();
            readonly Point3DfArray m_markerWorldCorners = new Point3DfArray();
            readonly KeypointArray m_refKeypoints = new KeypointArray();
            readonly DescriptorBuffer m_refDescriptors = new DescriptorBuffer();
            readonly Point2DfArray m_refImgCorners = new Point2DfArray();
            Transform3Df m_pose;
            bool m_isTrack;
            bool m_needNewTrackedPoints;
            readonly Stack<Image> m_CameraImagesForDetection = new Stack<Image>();
            readonly Stack<Image> m_CameraImagesForTracking = new Stack<Image>();

            readonly Stack<Tuple<Image, Transform3Df, bool>> m_outBufferDetection = new Stack<Tuple<Image, Transform3Df, bool>>();
            Image m_previousCamImage;
            readonly Point2DfArray m_imagePoints_track = new Point2DfArray();
            readonly Point3DfArray m_worldPoints_track = new Point3DfArray();
        }
#pragma warning restore IDE1006 // Styles d'affectation de noms
    }
}
