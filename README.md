# SolARUnityPlugin
A Unity plugin allowing to load SolAR pipelines

We offer two mods : 

* Novice
    We give you Pipelines (DLL) directly from our C++ SolAR framework.

 * Expert
    We give you the opportunity to modify and create your pipeline in C# directly in Unity. (Wrapping of our SolAR C++ framework)

More details on [http://solarframework.org/use/unity](https://solarframework.github.io/use/unity/)

## How build it  with pre-compiled 
* Install [Remaken](https://github.com/b-com-software-basis/remaken) (a meta dependencies management tool)
* Download [Swig](http://www.swig.org/) (use for wrapping) and add an environment variable to the swig.exe 
* Open a terminal and execute `Install.bat` to download module, wrap to C# and import DLLs in Unity

## How build it with source :

1° Build    xpcf
            & SolARFramework
            
2° Execute  the file "SolARFramework / SolARWrapper / _build.bat"

3° Build    SolARWrapper
            & SolARPipelineManager
            
4° Build    necessary module (OpenCV, Tools, FBOW... depending on pipelines you want to use)

5° Build    Pipelines (Fiducial, Natural Image, SLAM ...)

6° `remaken bundle file packagedependencies.txt -d ./Assets/Plugins` to update Unity's Plugins directory

7° Add .fbow files for SLAM

## We follow this hierarchy (Unity) (only folders) :

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
        -->
        -->CameraCalibration  (our calibration files)
        -->Markers            (our markers)
        -->Configuration      (our .xml files to configure our pipelines)
