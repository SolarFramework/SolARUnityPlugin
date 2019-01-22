# SolARUnityPlugin
A Unity plugin allowing to load SolAR pipelines


################################################
Unity settings
################################################

Edit -> ProjectSettings -> Player -> OtherSettings 

		-Unchecked the "Auto API windows" box
		-A table appears
		-Add OpenGLCore and put it at the first position
		
		-Modifying the .Net framework version  from 3.x to 4.x
		
The scene is composed of : 

A mainCamera
A DirectionalLight
An empty Gameobject SolARPipelineManager
A BackGroundCamera_canvas with a BackGroundCamera_Image (Raw Image) as child
An AR_Cube (the object to display on our Marker)
An EventSystem (Unity)

The package has ... too :

2 Materials :  one for Background , one for 3Dobjects like the AR_cube 
1 Shader
Scripts


now you have to :

	- Add SolarPipeline.cs to the SolARPipelineManager Gameobject
		--> Add BackGroundCamera_canvas, Maincamera, and AR_receiveTexture to the script
	- Add the AR_receiveTexture to Raw Image component of BackGroundCamera_Image
	- Add the AR_Object material to the AR_cube
	
	
Help :

	Bool SetupConsole can be use to see Debug.Log from the plugin.

