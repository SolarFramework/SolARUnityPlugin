@echo off
SET compiler=win-cl-14.1
SET SOLAR_PIPELINE_MANAGER_VERSION=0.9.1
SET SOLAR_WRAPPER_VERSION=0.9.1
SET SOURCEDIR=%cd%

if "%~1"=="" (SET bconfig=release) else (SET bconfig=%1)
echo Bundle third parties : %bconfig%
:: Bunlde all third parties in the ./Assets/plugins folder based on the packagedependencies.txt file. More information on remaken is available on https://github.com/b-com-software-basis/remaken 
remaken bundle -d ./Assets/Plugins -c %bconfig% --cpp-std 17 -b cl-14.1 packagedependencies.txt
remaken bundle -d ./Assets/Plugins/Android -c %bconfig% --cpp-std 17 -b clang -o android -a arm64-v8a packagedependencies.txt 
remaken bundle -d ./Assets/Plugins/Android -c %bconfig% --cpp-std 17 -b clang -o android -a arm64-v8a packagedependencies-android.txt 

::wrap cpp to c# interfaces
echo ---------------- wrap files with SWIG ----------------------
cd "..\..\..\core\SolARFramework\SolARWrapper"
call "_build.bat"
cd %SOURCEDIR%


:: copy csharp interfaces
echo ---------------- copy c# interfaces ----------------------
echo Delete following pipeline manager wrapper files
del ".\Assets\SolAR\Swig\*.*" /S /Q
timeout 2
echo Copy wrapper files
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARPipelineManager\%SOLAR_PIPELINE_MANAGER_VERSION%\csharp\*" ".\Assets\SolAR\Swig\SolARPluginNovice\"
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\*" ".\Assets\SolAR\Swig\SolARPluginExpert\" /S /EXCLUDE:excludedFile_Bat.txt
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Core\*" ".\Assets\SolAR\Swig\Utilities\Core\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Datastructure\Vector3f.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Datastructure\Transform3Df.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Datastructure\Matrix3x3f.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Datastructure\solar_datastructurePINVOKE.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Datastructure\solar_datastructure.cs" ".\Assets\SolAR\Swig\Utilities\" /S