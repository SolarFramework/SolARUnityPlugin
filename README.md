# SolARUnityPlugin
A Unity plugin allowing to load SolAR pipelines

We offer two mods : 

-Novice
    We give you Pipelines (DLL) directly from our C++ SolAR framework.
-Expert
    We give you the opportunity to modify and create your pipeline in C# directly in Unity. (Wrapping of our SolAR C++ framework)


## We follow this hierarchie (Unity) (only folders) :

  Assets
    ==> Objects   (3D models)
                
    ==> Plugins   (Our DLLs)
    
    ==> Scenes    (Samples Scenes)
    
    ==> SolAR     (everything else)
        --> Editor      (scripts User Interface in Editor)
        
        --> Materials   (our Unity Materials)
        
        --> Pipelines   (our pipelines descriptions - Novice version only)
        
        --> Scripts     (all our scripts Novice and Expert version)
        
            *NoviceVersion
            
            *ExpertVersion
               
        --> Shaders     (our shaders)
        
        
    ==> StreamingAssets
        -->CameraCalibration  (our calibration files)
        -->Markers            (our markers)
        -->Configuration      (our .xml files to configure our pipelines)
