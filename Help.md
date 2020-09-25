# SolARUnityPlugin Help

## Android <img src="https://raw.githubusercontent.com/github/explore/80688e429a7d4ef2fca1e82350fe8e3517d3494d/topics/android/android.png" alt="android logo" width="32" height="32"> 

SolAR pipelines support Android build using Unity Plugin. 
### Use

It is recommended  to develop your AR application using libraries  built for your developer  environment (Windows/Linux). Then you can build your **PipelinePlugin** for Android, more information in the dedicated [Android install](https://solarframework.github.io/install/android/) section.You should put every required librariy in `./Assets/Plugins/Android`.

:information_source: You can use `./Bundle.bat` to import libraries for Android and wrap C++ to C#.

### Deployment


**/!\ Only [arm64-v8a](https://developer.android.com/ndk/guides/abis#arm64-v8a) architecture are supported by SolAR.**

While you launch build for Android platform : 

* On Unity
  * Unity pre-build process will be called to generate `./Assets/StreamingAssets/SolAR/Android/android.xml`. This file will list every file in under your `./Assets/StreamingAssets/`. 

  * Path of pipelines xml in `./Assets/StreamingAssets/SolAR/Pipelines/` are set to match Android filesystem to the public application directory. This will let your xml available on your device and you could edit them.

  * This file will be read to clone every asset included in the Android private JAR to Android application public path

  * APK is built and it includes Unity StreamingAssets and libraries


* On Android

  * While the application is launched for the first time, it will look for `android.xml` in the private application JAR. Then if this file doesn't exist in the device application public path (`/storage/emulated/0/Android/com.bcom/SolARDemo/files/StreamingAssets/SolAR/Android`) it will be read and all of his content lists will be cloned into this path. Otherwise, the already present one will be read and cloned. If you don't want to **overwrite** an asset at each application launching you can set the overwrite attribute for a dedicated file to false.

  * The pipeline selected by the application will be load (from public application path) and his path to `./Assets/Plugins` will be matched with Android private JAR path (`/data/app/com.bcom/SolARUnityPlugin-[only-known-on-running]==/lib/arm64/`).

  * Application is initialized correctly. You can change the pipeline selected with SolARMenu in the right-hand corner.

More information is available on the website in the section [Assemble/Unity Android Deployment](https://solarframework.github.io/assemble/unity_pipeline/#UnityAndroidDeployment).



## Update the plugin with a new SolAR version

If Code modifications you should :
* Rebuild modified projects (Framework, Modules, Samples) and dependencies
* If **SolARPipelineManager** is rebuilt, execute `./core/SolARPipelineManager/_BuildCSharp.bat`
* If **SolARWrapper** is rebuilt, execute `./core/SolARFramework/SolARWrapper/_build.bat`

You can also use `Bundle.bat` to update every DLL in `./Assets/Plugins`.
		

## What are scripts doing ? 

* **BuildCSharp.bat :** Wrap a part of the pipelineManager code from C++ to C# using SWIG   (NoviceVersion of Unity plugin)

* **_build.bat :** Wrap SolAR code from C++ to C# using SWIG	(ExpertVersion of Unity plugin)

