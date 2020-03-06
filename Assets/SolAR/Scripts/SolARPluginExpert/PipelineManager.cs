using System;
using System.Collections.Generic;
using SolAR.Api.Display;
using SolAR.Api.Input.Devices;
using SolAR.Core;
using SolAR.Datastructure;
using SolAR.Samples;
using UniRx;
using UnityEngine;
using XPCF.Api;
using XPCF.Core;
using System.IO;
using System.Xml.Linq;

namespace SolAR
{
    public class PipelineManager : AbstractSample
    {
        public event Action<bool> OnStatus;
        public event Action<Texture> OnFrame;
        public event Action<Sizei, Matrix3x3f, Vector5f> OnCalibrate;
        public Pose Pose { get { return pose.ToUnity(); } }

        protected enum SOURCE { SolAR, Unity }
        [SerializeField] protected SOURCE source = SOURCE.SolAR;

        protected enum DISPLAY { None, SolAR, Unity, }
        [SerializeField] protected DISPLAY display = DISPLAY.Unity;

        [SerializeField] protected bool mogrify;

        Image inputImage;
        Transform3Df pose;

        IComponentManager xpcfComponentManager;

        // components
        new ICamera camera;
        I3DOverlay overlay3D;
        IImageViewer imageViewer;

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
        public PIPELINE mode;

        protected override void OnEnable()
        {
            base.OnEnable();

            xpcfComponentManager = xpcf_api.getComponentManagerInstance();
            Disposable.Create(xpcfComponentManager.clear).AddTo(subscriptions);
            xpcfComponentManager.AddTo(subscriptions);

#if !UN    
            conf.path = File.ReadAllText("confPath.txt");
#endif
            if (xpcfComponentManager.load(conf.path) != XPCFErrorCode._SUCCESS)
            {
               Debug.LogErrorFormat("Failed to load the configuration file {0}", conf.path);
                enabled = false;
                return;
            }

            switch (mode)
            {
                case PIPELINE.Fiducial:
                    pipeline = new FiducialPipeline(xpcfComponentManager).AddTo(subscriptions);
                    break;
                case PIPELINE.Natural:
                    pipeline = new NaturalPipeline(xpcfComponentManager).AddTo(subscriptions);
                    break;
                case PIPELINE.SLAM:
                    pipeline = new SlamPipeline(xpcfComponentManager).AddTo(subscriptions);
                    break;
            }

            overlay3D = xpcfComponentManager.Create("SolAR3DOverlayOpencv").BindTo<I3DOverlay>().AddTo(subscriptions);

            switch (source)
            {
                case SOURCE.SolAR:
                    camera = xpcfComponentManager.Create("SolARCameraOpencv").BindTo<ICamera>().AddTo(subscriptions);

                    var intrinsic = camera.getIntrinsicsParameters();
                    var distorsion = camera.getDistorsionParameters();
                    var resolution = camera.getResolution();
                    pipeline.SetCameraParameters(intrinsic, distorsion);
                    overlay3D.setCameraParameters(intrinsic, distorsion);
                    OnCalibrate?.Invoke(resolution, intrinsic, distorsion);

                    if (camera.start() != FrameworkReturnCode._SUCCESS)
                    {
                        LOG_ERROR("Camera cannot start");
                        enabled = false;
                    }
                    break;
                case SOURCE.Unity:
                    webcam = new WebCamTexture();
                    webcam.Play();
                    if (!webcam.isPlaying)
                    {
                        LOG_ERROR("Camera cannot start");
                        enabled = false;
                    }
                    break;
            }

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

            start = clock();

            inputImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            pose = new Transform3Df().AddTo(subscriptions);
        }

        IPipeline pipeline;
        WebCamTexture webcam;

        public IEnumerable<IComponentIntrospect> xpcfComponents
        {
            get { return pipeline.xpcfComponents; }
        }

        protected void Update()
        {
            switch (source)
            {
                case SOURCE.SolAR:
                    if (camera.getNextImage(inputImage) != FrameworkReturnCode._SUCCESS)
                        return;
                    break;
                case SOURCE.Unity:
                    if (!webcam.didUpdateThisFrame)
                        return;
                    var ptr = webcam.GetNativeTexturePtr();
                    if (inputTex == null)
                    {
                        int w = webcam.width;
                        int h = webcam.height;
                        inputTex = Texture2D.CreateExternalTexture(w, h, TextureFormat.RGB24, false, false, ptr);
                    }
                    else
                    {
                        inputTex.UpdateExternalTexture(ptr);
                    }
                    break;
            }
            count++;

            var isTracking = pipeline.Proceed(inputImage, pose, camera) == FrameworkReturnCode._SUCCESS;

            //if(mode == PIPELINE.SLAM)
            //{

            //}



            foreach (GameObject g in GameObject.FindGameObjectsWithTag("SolARObject"))
                g.transform.GetComponent<Renderer>().enabled = isTracking;
            
            if (isTracking)
            {
                if (mogrify)
                {
                    overlay3D.draw(pose, inputImage);
                }
            }
            OnStatus?.Invoke(isTracking);

            switch (display)
            {
                case DISPLAY.SolAR:
                    enabled = (imageViewer.display(inputImage) == FrameworkReturnCode._SUCCESS);
                    break;
                case DISPLAY.Unity:
                    inputImage.ToUnity(ref inputTex);
                    OnFrame?.Invoke(inputTex);
                    break;
            }
        }

        protected override void OnDisable()
        {
            end = clock();
            double duration = (double)(end - start) / CLOCKS_PER_SEC;
            printf("Elasped time is {0} seconds.", duration);
            printf("Number of processed frames per second : {0}", count / duration);
            camera.Dispose();
            base.OnDisable();
        }

        public void PipelineMngchangePath(string t)
        {
            conf.path = t;
        }

    }
}
