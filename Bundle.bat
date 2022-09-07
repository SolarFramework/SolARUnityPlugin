@ECHO OFF
SET COMPILER_WIN=win-cl-14.1
SET COMPILER_ANDROID=android-clang
SET SOLAR_PIPELINE_MANAGER_VERSION=1.0.0
SET SOLAR_WRAPPER_VERSION=1.0.0

IF "%~1"=="" (SET bconfig=release) ELSE (SET bconfig=%1)

GOTO :BUNDLE

:BUNDLE
ECHO Bundle third parties : %bconfig%
:: Bunlde all third parties in the ./Assets/plugins folder based on the packagedependencies.txt file. More information on remaken is available on https://github.com/b-com-software-basis/remaken

::bundle for windows
ECHO Bundle third parties for windows platform: %bconfig%
remaken bundle --recurse -d ./Assets/Plugins -c %bconfig% --cpp-std 17 -b cl-14.1 -o win -a x86_64 packagedependencies.txt

::bundle for Android
conan profile update settings.os="Android" default
conan profile update settings.os_build="Windows" default 
conan profile update settings.arch="armv8" default 
conan profile update settings.compiler="clang" default 
conan profile update settings.compiler.version="8" default 
conan profile update settings.compiler.libcxx="libc++" default 
conan profile update settings.os.api_level="21" default 
conan profile update settings.compiler.cppstd="17" default

if not exist "./Assets/Plugins/Android" mkdir "./Assets/Plugins/Android"

ECHO Bundle third parties for android platform: %bconfig%
remaken bundle --recurse -d ./Assets/Plugins/Android -c %bconfig% --cpp-std 17 -b clang -o android -a arm64-v8a packagedependencies.txt 

conan profile update settings.os="Windows" default
conan profile update settings.os_build="Windows" default
conan profile update settings.arch="x86_64" default
conan profile update settings.compiler="Visual Studio"  default
conan profile update settings.compiler.version="15" default
conan profile remove settings.compiler.libcxx default
conan profile remove settings.os.api_level default
conan profile update settings.compiler.cppstd="17"  default
 

:DEPLOY
:: copy csharp interfaces
ECHO ---------------- Copy c# interfaces ----------------------
ECHO Delete following pipeline manager wrapper files
IF EXIST ".\Assets\SolAR\Scripts\Swig" RMDIR ".\Assets\SolAR\Scripts\Swig" /S /Q
::TIMEOUT 1
ECHO Copy wrapper files

XCOPY "%REMAKEN_PKG_ROOT%\%COMPILER_WIN%\SolARBuild\SolARPipelineManager\%SOLAR_PIPELINE_MANAGER_VERSION%\csharp\*" ^
 ".\Assets\SolAR\Scripts\Swig\" /Q /S
 
::XCOPY "%REMAKEN_PKG_ROOT%\%COMPILER_ANDROID%\SolARBuild\SolARWrapper\%SOLAR_WRAPPER_VERSION%\csharp\*" ^
:: ".\Assets\SolAR\Scripts\Swig\Expert\" /Q /S
 
