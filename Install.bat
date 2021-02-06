@ECHO OFF
SET CONFIG=release

ECHO ---------------- install third parties ----------------------
ECHO Install third parties
:: install all third parties in the %REMAKEN_PKG_ROOT%\packages. More information on remaken is available on https://github.com/b-com-software-basis/remaken 
::install for windows
REMAKEN install -c %CONFIG% --cpp-std 17 -b cl-14.1 packagedependencies.txt

::bundle for Android
conan profile update settings.os="Android" default
conan profile update settings.os_build="Windows" default 
conan profile update settings.arch="armv8" default 
conan profile update settings.compiler="clang" default 
conan profile update settings.compiler.version="8" default 
conan profile update settings.compiler.libcxx="libc++" default 
conan profile update settings.os.api_level="21" default 
conan profile update settings.compiler.cppstd="17" default
remaken profile init --cpp-std 17 -b clang -o android -a arm64-v8a

REMAKEN install -c %CONFIG% --cpp-std 17 -b clang -o android -a arm64-v8a packagedependencies-android.txt 

conan profile update settings.os="Windows" default
conan profile update settings.os_build="Windows" default
conan profile update settings.arch="x86_64" default
conan profile update settings.compiler="Visual Studio"  default
conan profile update settings.compiler.version="15" default
conan profile remove settings.compiler.libcxx default
conan profile remove settings.os.api_level default
conan profile update settings.compiler.cppstd="17"  default
remaken profile init --cpp-std 17 -b cl-14.1 -o win -a x86_64

ECHO ---------------- bundle plugins ----------------------
CALL "Bundle.bat" %CONFIG%

EXIT /B 0
