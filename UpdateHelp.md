# SolARUnityPlugin

## Update the plugin with a new SolAR version

  If Code modifications you should


### Rebuild modified projects (Framework, Modules, Samples) and dependencies
* If **SolARPipelineManager** is rebuilt, execute `./core/SolARPipelineManager/_BuildCSharp.bat`
* If **SolARWrapper** is rebuilt, execute `./core/SolARFramework/SolARWrapper/_build.bat`

You can also use `Bundle.bat` to update everything.
		

## What are scripts doing ? 

* **BuildCSharp.bat :** Wrap a part of the pipelineManager code from C++ to C# using SWIG   (NoviceVersion of Unity plugin)

* **_build.bat :** Wrap SolAR code from C++ to C# using SWIG	(ExpertVersion of Unity plugin)

