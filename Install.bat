@echo off
SET compiler=win-cl-14.1
SET config=release
SET SOLAR_PIPELINE_MANAGER_VERSION=0.8.0
SET SOLAR_WRAPPER_VERSION=0.8.0
SET SOURCEDIR=%cd%

echo ---------------- install third parties ----------------------
echo Install third parties
:: install all third parties in the ./Assets/plugins folder based on the packagedependencies.txt file. More information on remaken is available on https://github.com/b-com-software-basis/remaken 
remaken install -c %config% --cpp-std 17 -b cl-14.1 packagedependencies.txt
:: remaken install -b clang -o android -a arm64 packagedependencies-android.txt

echo ---------------- bundle plugins ----------------------
echo Bundle third parties
:: Bunlde all third parties in the ./Assets/plugins folder based on the packagedependencies.txt file. More information on remaken is available on https://github.com/b-com-software-basis/remaken 
remaken bundle -d ./Assets/Plugins -c %config% --cpp-std 17 -b cl-14.1 packagedependencies.txt 
:: remaken bundle file packagedependencies-android.txt -d ./Assets/Plugins/Android  -c %config% -b clang -o android -a arm64

::wrap cpp to c# interfaces
echo ---------------- wrap files with SWIG ----------------------
echo Delete following pipeline manager wrapper files ?
del ".\Assets\SolAR\Swig\*.*" /S /Q
cd "..\..\..\core\SolARFramework\SolARWrapper"
call "_build.bat"
cd %SOURCEDIR%
timeout 2

:: copy csharp interfaces
echo ---------------- copy c# interfaces ----------------------
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARPipelineManager\%SOLAR_PIPELINE_MANAGER_VERSION%\csharp\*" ".\Assets\SolAR\Swig\SolARPluginNovice\"

xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\*" ".\Assets\SolAR\Swig\SolARPluginExpert\" /S /EXCLUDE:excludedFile_Bat.txt
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Core\*" ".\Assets\SolAR\Swig\Utilities\Core\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Datastructure\Vector3f.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Datastructure\Transform3Df.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Datastructure\Matrix3x3f.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Datastructure\solar_datastructurePINVOKE.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Datastructure\solar_datastructure.cs" ".\Assets\SolAR\Swig\Utilities\" /S

exit /B 0