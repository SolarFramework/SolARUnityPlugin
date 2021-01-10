@ECHO OFF
SET COMPILER=win-cl-14.1
SET SOLAR_PIPELINE_MANAGER_VERSION=0.9.0
SET SOLAR_WRAPPER_VERSION=0.9.0

IF "%~1"=="" (SET bconfig=release) ELSE (SET bconfig=%1)
ECHO Bundle third parties : %bconfig%
:: Bunlde all third parties in the ./Assets/plugins folder based on the packagedependencies.txt file. More information on remaken is available on https://github.com/b-com-software-basis/remaken 
REMAKEN bundle -d ./Assets/Plugins -c %bconfig% --cpp-std 17 -b cl-14.1 packagedependencies.txt
::REMAKEN bundle -d ./Assets/Plugins/Android -c %bconfig% --cpp-std 17 -b clang -o android -a arm64-v8a packagedependencies.txt 
::REMAKEN bundle -d ./Assets/Plugins/Android -c %bconfig% --cpp-std 17 -b clang -o android -a arm64-v8a packagedependencies-android.txt 

::wrap cpp to c# interfaces
::ECHO ---------------- wrap files with SWIG ----------------------
::PUSHD "..\..\..\core\SolARFramework\SolARWrapper"
::CALL "_build.bat"
::POPD

:: copy csharp interfaces
ECHO ---------------- copy c# interfaces ----------------------
ECHO Delete following pipeline manager wrapper files
DEL ".\Assets\SolAR\Scripts\SolARPluginNovice\Swig\*.*" /S /Q
DEL ".\Assets\SolAR\Scripts\SolARPluginExpert\Swig\*.*" /S /Q
TIMEOUT 1
ECHO Copy wrapper files
XCOPY "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%COMPILER%\SolARPipelineManager\%SOLAR_PIPELINE_MANAGER_VERSION%\csharp\*" ^
 ".\Assets\SolAR\Scripts\SolARPluginNovice\Swig\"
XCOPY "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%COMPILER%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\*" ^
 ".\Assets\SolAR\Scripts\SolARPluginExpert\Swig\" /S /EXCLUDE:excludedFile_Bat.txt
XCOPY "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%COMPILER%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\XPCF\Core\*" ^
 ".\Assets\SolAR\Scripts\SolARPluginNovice\Swig\Utilities\XPCF\" /S
XCOPY "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%COMPILER%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Core\*" ^
 ".\Assets\SolAR\Scripts\SolARPluginNovice\Swig\Utilities\Core\" /S
XCOPY "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%COMPILER%\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\SolAR\Datastructure\*" ^
 ".\Assets\SolAR\Scripts\SolARPluginNovice\Swig\Utilities\Datastructure\" /S
