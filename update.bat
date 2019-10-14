echo off

SET version = "full"

echo Delete following pipeline manager wrapper files ?
del ".\Assets\Plugins\*.*"
del ".\Assets\SolAR\Swig\*.*" /S /Q

SET mode="release"
SET compiler=win-cl-14.1

:: copy dll needed in Expert, Novice and Full
echo ------------copy third parties dll------------
xcopy "%REMAKENROOT%\packages\thirdParties\%compiler%\boost\1.70.0\lib\x86_64\shared\%mode%\boost_context.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\thirdParties\%compiler%\boost\1.70.0\lib\x86_64\shared\%mode%\boost_date_time.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\thirdParties\%compiler%\boost\1.70.0\lib\x86_64\shared\%mode%\boost_fiber.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\thirdParties\%compiler%\boost\1.70.0\lib\x86_64\shared\%mode%\boost_filesystem.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\thirdParties\%compiler%\boost\1.70.0\lib\x86_64\shared\%mode%\boost_log.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\thirdParties\%compiler%\boost\1.70.0\lib\x86_64\shared\%mode%\boost_system.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\thirdParties\%compiler%\boost\1.70.0\lib\x86_64\shared\%mode%\boost_thread.dll" .\Assets\Plugins\

xcopy "%REMAKENROOT%\packages\thirdParties\%compiler%\opencv\3.4.3\lib\x86_64\shared\%mode%\opencv_world343.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\thirdParties\%compiler%\freeglut\3.0.0\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\

echo --------------copy framework dll--------------
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARFramework\0.6.0\lib\x86_64\shared\%mode%\SolARFramework.dll" .\Assets\Plugins\

echo --------------copy XPCF dll--------------
xcopy "%REMAKENROOT%\packages\%compiler%\xpcf\2.3.0\lib\x86_64\shared\%mode%\xpcf.dll" .\Assets\Plugins\

::Full
:: copy csharp interfaces
echo ---------------- copy c# ----------------------
timeout 2
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARPipelineManager\0.6.0\CSharp\*" ".\Assets\SolAR\Swig\SolARPluginNovice\"

xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\*" ".\Assets\SolAR\Swig\SolARPluginExpert\" /S /EXCLUDE:excludedFile_Bat.txt
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\SolAR\Core\*" ".\Assets\SolAR\Swig\Utilities\Core\" /S
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\SolAR\Datastructure\Vector3f.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\SolAR\Datastructure\Transform3Df.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\SolAR\Datastructure\Matrix3x3f.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\SolAR\Datastructure\solar_datastructurePINVOKE.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\SolAR\Datastructure\solar_datastructure.cs" ".\Assets\SolAR\Swig\Utilities\" /S

echo -----------copy pipeline Manager dll----------
timeout 2
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARPipelineManager\0.6.0\lib\x86_64\shared\%mode%\SolARPipelineManager.dll" .\Assets\Plugins\

echo ---------------copy modules dll---------------
timeout 2
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARModuleOpenCV\0.6.0\lib\x86_64\shared\%mode%\SolARModuleOpenCV.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARModuleTools\0.6.0\lib\x86_64\shared\%mode%\SolARModuleTools.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARModuleOpenGL\0.6.0\lib\x86_64\shared\%mode%\SolARModuleOpenGL.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARModuleOpenGV\0.6.0\lib\x86_64\shared\%mode%\SolARModuleOpenGV.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARModuleCeres\0.6.0\lib\x86_64\shared\%mode%\SolARModuleCeres.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARModuleFBOW\0.6.0\lib\x86_64\shared\%mode%\SolARModuleFBOW.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARModuleNonFreeOpenCV\0.6.0\lib\x86_64\shared\%mode%\SolARModuleNonFreeOpenCV.dll" .\Assets\Plugins\


echo --------------copy pipelines dll--------------
timeout 2
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\PipelineFiducialMarker\0.6.0\lib\x86_64\shared\%mode%\PipelineFiducialMarker.dll" .\Assets\Plugins\
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\\PipelineNaturalImageMarker\0.6.0\lib\x86_64\shared\%mode%\PipelineNaturalImageMarker.dll" .\Assets\Plugins\

echo -----------copy SolAR Wrapper dll----------
timeout 2
xcopy "%REMAKENROOT%\packages\SolARBuild\%compiler%\SolARWrapper\0.6.0\lib\x86_64\shared\%mode%\SolARWrapper.dll" .\Assets\Plugins\

exit /B 0