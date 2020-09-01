# SolARUnityPlugin

## Update the plugin with a new SolAR version

  If Code modifications you should


### Rebuild modified projects (Framework, Modules, Samples) and dependencies
* If **SolARPipelineManager** is rebuilt, execute `./core/SolARPipelineManager/_BuildCSharp.bat`
* If **SolARWrapper** is rebuilt, execute `./core/SolARFramework/SolARWrapper/_build.bat`

You can also use `Bundle.bat` to update everything.
		
### ~~Rebuild Expert~~
* ~~Go to `./core/SolARFramework` and execute the command line `./build-scripts/cmake-build-SolARWrapper.sh unity` (Select Generator <Visual Studio XX YYYY Win64>)~~

* ~~Execute `./plugin/unity/SolARUnityPlugin/copydll.bat`~~


## What are scripts doing ? 

* **BuildCSharp.bat :** Wrap a part of the pipelineManager code from C++ to C# using SWIG   (NoviceVersion of Unity plugin)

* **_build.bat :** Wrap SolAR code from C++ to C# using SWIG	(ExpertVersion of Unity plugin)

* ~~**copydll.bat :** Copy all Dlls and C# scripts need in UnityPlugin from there SolAR location to Unity "./Assets/Plugin" folder.~~

* ~~**cmake-build-SolARWrapper.sh :** Compile and build the results of _build.bat for the expert version of unity plugin.~~