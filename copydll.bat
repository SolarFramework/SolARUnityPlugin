echo off

:: remove existing pipeline manager C# wrapper files
echo Delete following pipeline manager wrapper files ?
del ".\Assets\SolAR\PipelineManagerWrapper\*.*"
del ".\Assets\Plugins\SolAR\*.*"

:: copy csharp interfaces
xcopy "%BCOMDEVROOT%\bcombuild\SolARPipelineManager\0.1.0\CSharp\*" ".\Assets\SolAR\PipelineManagerWrapper\"

:: copy dll
SET mode="release"
IF "%1"=="-debug" (SET mode="debug")
echo ------------copy third parties dll------------
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_context.dll" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_date_time.dll" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_fiber.dll" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_filesystem.dll" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_log.dll" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_system.dll" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_thread.dll" .\Assets\Plugins\SolAR\

xcopy "%BCOMDEVROOT%\thirdParties\xpcf\2.1.0\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\opencv\3.4.3\lib\x86_64\shared\%mode%\opencv_world343.dll" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\freeglut\3.0.0\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\SolAR\

echo --------------copy framework dll--------------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARFramework\0.5.1\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\SolAR\

echo -----------copy pipeline Manager dll----------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARPipelineManager\0.1.0\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\SolAR\

echo ---------------copy modules dll---------------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenCV\0.5.1\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenGL\0.5.1\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleTools\0.5.1\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\SolAR\

echo --------------copy pipelines dll--------------
xcopy "%BCOMDEVROOT%\bcomBuild\PipelineFiducialMarker\0.1.0\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\bcomBuild\PipelineNaturalImageMarker\0.1.0\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\SolAR\

IF "%1"=="-debug" (GOTO Debug)
exit /B 0

:: copy .pdb
:Debug
echo ------------copy third parties pdb------------
::xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\*.pdb" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_context.pdb" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_date_time.pdb" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_fiber.pdb" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_filesystem.pdb" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_log.pdb" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_system.pdb" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_thread.pdb" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\xpcf\2.1.0\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\opencv\3.4.3\lib\x86_64\shared\%mode%\opencv_world343.pdb" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\thirdParties\freeglut\3.0.0\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\SolAR\

echo --------------copy framework pdb--------------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARFramework\0.5.1\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\SolAR\

echo -----------copy pipeline Manager pdb----------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARPipelineManager\0.1.0\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\SolAR\

echo ---------------copy modules pdb---------------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenCV\0.5.1\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenGL\0.5.1\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleTools\0.5.1\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\SolAR\

echo --------------copy pipelines pdb--------------
xcopy "%BCOMDEVROOT%\bcomBuild\PipelineFiducialMarker\0.1.0\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\SolAR\
xcopy "%BCOMDEVROOT%\bcomBuild\PipelineNaturalImageMarker\0.1.0\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\SolAR\

exit /B 0