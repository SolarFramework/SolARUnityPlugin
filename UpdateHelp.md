# SolARUnityPlugin

## Update the plugin with a new SolAR version

  If Code modifications
		
		--> ReBuild modified projects (Framework, Modules, Samples) and dependencies.   Dependencies :   Modules projects depends on Framework
			|																							 Samples depend on Framework and some Modules 
			|
			|--> if PipelineManager is rebuilt, execute _"BuildCSharp.bat" in [SolARFramework/sources/SolARPipelineManager/]
		
		    |
			|--> if SolARWrapper is rebuilt, execute _"_build.bat" in [SolARFramework/sources/SolARFramework/SolARWrapper/]
		
		--> Go to [SolARFramework/], open a BASH window here (Git Bash for exemaple) 
			|
			|--> use this line command _"./build-scripts/cmake-build-SolARWrapper.sh unity" (Select Generator <Visual Studio XX YYYY Win64>)
		
		--> Execute _"copydll.bat" in [SolARFramework/sources/SolARUnityPlugin/]
		
		
## What are scripts doing ? 

	BuildCSharp.bat --> Wrap a part of the pipelineManager code from C++ to C# using SWIG   (NoviceVersion of Unity plugin)
	
	  _build.bat    --> Wrap SolAR code from C++ to C# using SWIG	(ExpertVersion of Unity plugin)

	  copydll.bat 	--> copy all Dlls and C# scripts need in UnityPlugin from there SolAR location to Unity "./Assets/Plugin" folder.
	
	cmake-build-SolARWrapper.sh  --> compile and build the results of _build.bat for the expert version of unity plugin.