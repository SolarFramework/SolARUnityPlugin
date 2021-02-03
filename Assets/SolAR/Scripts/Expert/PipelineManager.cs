using System;
using System.Collections.Generic;
using SolAR.Api.Input.Devices;
using SolAR.Core;
using SolAR.Datastructure;
using SolAR.Expert.Samples;
using UniRx;
using UnityEngine;
using XPCF.Api;
using XPCF.Core;

namespace SolAR.Expert
{
    public class PipelineManager : AbstractSampleMB, ISolARPipeline
    {
        [Tooltip("Library used to capture the video stream")]
        [UnityEngine.Serialization.FormerlySerializedAs("source")]
        [SerializeField] protected SOURCE videoSource = SOURCE.SolAR;
        protected enum SOURCE { SolAR, External }

        //[Tooltip("Library used to render video stream")]
        //[SerializeField] protected DISPLAY display = DISPLAY.Unity;
        //protected enum DISPLAY { None, SolAR, Unity, }

        //[Tooltip("For debug: Insert the tracked marker into the video stream")]
        //[SerializeField] protected bool mogrify;

        Image inputImage;
        Transform3Df pose;

        IComponentManager xpcfComponentManager;

        // components
        ICamera srcCamera;
        //I3DOverlay overlay3D;
        //IImageViewer imageViewer;

        // to count the average number of processed frames per seconds
        int count = 0;
        long start;
        long end;

        public enum PIPELINE
        {
            Fiducial,
            Natural,
            SLAM,
        }
        [UnityEngine.Serialization.FormerlySerializedAs("mode")]
        public PIPELINE pipeline;

        /*
        PIPELINE _mode;

        protected void Awake() { _mode = mode; }

        protected void OnValidate()
        {
            if (!Application.isPlaying) return;
            if (!isActiveAndEnabled) return;
            if (_mode != mode)
            {
                _mode = mode;

                pipeline?.Dispose();

                switch (mode)
                {
                    case PIPELINE.Debug:
                        pipeline = new DebugPipeline(xpcfComponentManager).AddTo(subscriptions);
                        break;
                    case PIPELINE.Fiducial:
                        pipeline = new FiducialPipeline(xpcfComponentManager).AddTo(subscriptions);
                        break;
                    case PIPELINE.Natural:
                        pipeline = new NaturalPipeline(xpcfComponentManager).AddTo(subscriptions);
                        break;
                }
            }
        }
        */

        protected override void OnEnable()
        {
            base.OnEnable();

            xpcfComponentManager = xpcf_api.getComponentManagerInstance();
            Disposable.Create(xpcfComponentManager.clear).AddTo(subscriptions);
            xpcfComponentManager.AddTo(subscriptions);

            if (xpcfComponentManager.load(conf.path) != XPCFErrorCode._SUCCESS)
            {
                Debug.LogErrorFormat(this, "Failed to load the configuration file {0}", conf.path);
                enabled = false;
                return;
            }

            switch (pipeline)
            {
                case PIPELINE.Fiducial:
                    manager = new FiducialSample(xpcfComponentManager);
                    break;
                case PIPELINE.Natural:
                    manager = new NaturalSample(xpcfComponentManager);
                    break;
                case PIPELINE.SLAM:
                    manager = new SlamSample(xpcfComponentManager);
                    break;
            }
            manager.AddTo(subscriptions);

            //overlay3D = xpcfComponentManager.Create("SolAR3DOverlayOpencv").BindTo<I3DOverlay>().AddTo(subscriptions);

            switch (videoSource)
            {
                case SOURCE.SolAR:
                    srcCamera = xpcfComponentManager.Resolve<ICamera>().AddTo(subscriptions).BindTo<ICamera>().AddTo(subscriptions);

                    var intrinsic = srcCamera.getIntrinsicsParameters();
                    var distortion = srcCamera.getDistortionParameters();
                    var resolution = srcCamera.getResolution();
                    manager.SetCameraParameters(intrinsic, distortion);
                    //overlay3D.setCameraParameters(intrinsic, distortion);
                    onCalibrate(resolution, intrinsic, distortion);

                    if (srcCamera.start() != FrameworkReturnCode._SUCCESS)
                    {
                        LOG_ERROR("Camera cannot start");
                        enabled = false;
                    }
                    break;
                case SOURCE.External:
                    webcam = new WebCamTexture();
                    webcam.Play();
                    if (!webcam.isPlaying)
                    {
                        LOG_ERROR("Camera cannot start");
                        enabled = false;
                    }
                    break;
            }

            /*
            switch (display)
            {
                case DISPLAY.SolAR:
                    // Set the size of the box to display according to the marker size in world unit
                    var overlay3D_sizeProp = overlay3D.BindTo<IConfigurable>().getProperty("size");
                    var size = pipeline.GetMarkerSize();
                    overlay3D_sizeProp.setFloatingValue(size.width, 0);
                    overlay3D_sizeProp.setFloatingValue(size.height, 1);
                    overlay3D_sizeProp.setFloatingValue(size.height / 2.0f, 2);

                    imageViewer = xpcfComponentManager.Create("SolARImageViewerOpencv").AddTo(subscriptions).BindTo<IImageViewer>().AddTo(subscriptions);
                    break;
                case DISPLAY.Unity:
                    break;
            }
            */

            start = clock();

            inputImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            pose = new Transform3Df().AddTo(subscriptions);
        }

        IPipeline manager;
        WebCamTexture webcam;

#pragma warning disable IDE1006 // Styles d'affectation de noms
        public IEnumerable<IComponentIntrospect> xpcfComponents => manager.xpcfComponents;
#pragma warning restore IDE1006 // Styles d'affectation de noms

        protected void Update()
        {
            switch (videoSource)
            {
                case SOURCE.SolAR:
                    if (srcCamera.getNextImage(inputImage) != FrameworkReturnCode._SUCCESS)
                        return;
                    break;
                case SOURCE.External:
                    if (!webcam.didUpdateThisFrame)
                        return;
                    var ptr = webcam.GetNativeTexturePtr();
                    if (inputTex == null)
                    {
                        int w = webcam.width, h = webcam.height;
                        inputTex = Texture2D.CreateExternalTexture(w, h, TextureFormat.RGB24, false, false, ptr);
                    }
                    else
                    {
                        inputTex.UpdateExternalTexture(ptr);
                    }
                    break;
            }
            count++;

            var retCode = manager.Proceed(inputImage, pose, srcCamera);
            var isTracking = retCode == FrameworkReturnCode._SUCCESS;

            //if (isTracking)
            //{
            //    if (mogrify)
            //    {
            //        overlay3D.draw(pose, inputImage);
            //    }
            //}
            onStatus(isTracking);
            Pose = pose.ToUnity();
            onPose(isTracking ? Pose : default(Pose?));

            //switch (display)
            {
                //case DISPLAY.SolAR:
                //    enabled = (imageViewer.display(inputImage) == FrameworkReturnCode._SUCCESS);
                //    break;
                //case DISPLAY.Unity:
                inputImage.ToUnity(ref inputTex);
                onFrame(inputTex, inputImage.getImageLayout());
                //break;
            }
        }

        protected override void OnDisable()
        {
            end = clock();
            double duration = (double)(end - start) / CLOCKS_PER_SEC;
            printf("Elasped time is {0} seconds.", duration);
            printf("Number of processed frames per second : {0}", count / duration);
            base.OnDisable();
        }
    }
}
