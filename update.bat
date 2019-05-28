echo off

SET version = "full"
IF "%1"=="novice" (SET version="novice")
IF "%1"=="expert" (SET version="expert")

echo Delete following pipeline manager wrapper files ?
del ".\Assets\Plugins\*.*"
del ".\Assets\SolAR\Swig\*.*" /S /Q

SET mode="release"
IF "%2"=="-debug" (SET mode="debug")

:: copy dll needed in Expert, Novice and Full
echo ------------copy third parties dll------------
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_context.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_date_time.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_fiber.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_filesystem.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_log.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_system.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_thread.dll" .\Assets\Plugins\

xcopy "%BCOMDEVROOT%\thirdParties\xpcf\2.1.0\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\thirdParties\opencv\3.4.3\lib\x86_64\shared\%mode%\opencv_world343.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\thirdParties\freeglut\3.0.0\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\

echo --------------copy framework dll--------------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARFramework\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\


REM IF "%1"=="full" (GOTO Full)
REM IF "%1"=="novice" (GOTO Novice)
REM IF "%1"=="expert" (GOTO Expert)


::Full
:: copy csharp interfaces
echo ---------------- copy c# ----------------------
timeout 2
xcopy "%BCOMDEVROOT%\bcombuild\SolARPipelineManager\0.5.2\CSharp\*" ".\Assets\SolAR\Swig\SolARPluginNovice\"

xcopy "%BCOMDEVROOT%\bcomBuild\SolARWrapper\csharp\*" ".\Assets\SolAR\Swig\SolARPluginExpert\" /S /EXCLUDE:excludedFile_Bat.txt

xcopy "%BCOMDEVROOT%\bcomBuild\SolARWrapper\csharp\SolAR\Core\*" ".\Assets\SolAR\Swig\Utilities\Core\" /S
xcopy "%BCOMDEVROOT%\bcomBuild\SolARWrapper\csharp\SolAR\Datastructure\Vector3f.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%BCOMDEVROOT%\bcomBuild\SolARWrapper\csharp\SolAR\Datastructure\Transform3Df.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%BCOMDEVROOT%\bcomBuild\SolARWrapper\csharp\SolAR\Datastructure\Matrix3x3f.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%BCOMDEVROOT%\bcomBuild\SolARWrapper\csharp\SolAR\Datastructure\solar_datastructurePINVOKE.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%BCOMDEVROOT%\bcomBuild\SolARWrapper\csharp\SolAR\Datastructure\solar_datastructure.cs" ".\Assets\SolAR\Swig\Utilities\" /S

echo -----------copy pipeline Manager dll----------
timeout 2
xcopy "%BCOMDEVROOT%\bcomBuild\SolARPipelineManager\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\

echo ---------------copy modules dll---------------
timeout 2
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenCV\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleTools\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenGL\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenGV\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleCeres\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleFBOW\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleNonFreeOpenCV\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\


echo --------------copy pipelines dll--------------
timeout 2
xcopy "%BCOMDEVROOT%\bcomBuild\PipelineFiducialMarker\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
xcopy "%BCOMDEVROOT%\bcomBuild\PipelineNaturalImageMarker\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\

echo -----------copy SolAR Wrapper dll----------
timeout 2
xcopy "%BCOMDEVROOT%\bcomBuild\SolARWrapper\build\Release\*.dll" .\Assets\Plugins\

exit /B 0

REM :Novice

REM echo ---------------- copy c# ----------------------
REM timeout 2
REM xcopy "%BCOMDEVROOT%\bcombuild\SolARPipelineManager\0.5.2\CSharp\*" ".\Assets\SolAR\Swig\SolARPluginNovice\"
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARWrapper\csharp\SolAR\Datastructure\*" ".\Assets\SolAR\Swig\SolARPluginNovice\" /S
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARWrapper\csharp\SolAR\*.cs" ".\Assets\SolAR\Swig\SolARPluginNovice\" /S

REM echo -----------copy pipeline Manager dll----------
REM timeout 2
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARPipelineManager\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\

REM echo ---------------copy modules dll---------------
REM timeout 2
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenCV\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleTools\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenGL\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenGV\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleCeres\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleFBOW\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleNonFreeOpenCV\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
REM echo --------------copy pipelines dll--------------
REM timeout 2
REM xcopy "%BCOMDEVROOT%\bcomBuild\PipelineFiducialMarker\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\PipelineNaturalImageMarker\0.5.2\lib\x86_64\shared\%mode%\*.dll" .\Assets\Plugins\

REM exit /B 0

REM :Expert

REM echo ---------------- copy c# ----------------------
REM timeout 2
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARWrapper\csharp\*" ".\Assets\SolAR\Swig\SolARPluginExpert" /S

REM echo -----------copy SolAR Wrapper dll----------
REM timeout 2
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARWrapper\build\Release\*.dll" .\Assets\Plugins\

REM exit /B 0






REM IF "%1"=="-debug" (GOTO Debug)
REM exit /B 0

REM :: copy .pdb
REM :Debug
REM echo ------------copy third parties pdb------------
REM timeout 2
REM ::xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\*.pdb" .\
REM xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_context.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_date_time.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_fiber.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_filesystem.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_log.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_system.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_thread.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\thirdParties\xpcf\2.1.0\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\thirdParties\opencv\3.4.3\lib\x86_64\shared\%mode%\opencv_world343.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\thirdParties\freeglut\3.0.0\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\

REM echo --------------copy framework pdb--------------
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARFramework\0.5.2\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\

REM echo -----------copy pipeline Manager pdb----------
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARPipelineManager\0.5.2\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\

REM echo ---------------copy modules pdb---------------
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenCV\0.5.2\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenGL\0.5.2\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleTools\0.5.2\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenGV\0.5.1\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleCeres\0.5.1\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleFBOW\0.5.1\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleNonFreeOpenCV\0.5.1\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\

REM echo --------------copy pipelines pdb--------------
REM xcopy "%BCOMDEVROOT%\bcomBuild\PipelineFiducialMarker\0.5.2\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\
REM xcopy "%BCOMDEVROOT%\bcomBuild\PipelineNaturalImageMarker\0.5.2\lib\x86_64\shared\%mode%\*.pdb" .\Assets\Plugins\

REM exit /B 0