
@echo off

if "%~1"=="" (SET bconfig=release) else (SET bconfig=%1)
echo Bundle third parties : %bconfig%
:: Bunlde all third parties in the ./Assets/plugins folder based on the packagedependencies.txt file. More information on remaken is available on https://github.com/b-com-software-basis/remaken 
remaken bundle -d ./Assets/Plugins -c %bconfig% --cpp-std 17 -b cl-14.1 packagedependencies.txt
remaken bundle -d ./Assets/Plugins/Android -c %bconfig% --cpp-std 17 -b clang -o android -a arm64-v8a packagedependencies.txt 
remaken bundle -d ./Assets/Plugins/Android -c %bconfig% --cpp-std 17 -b clang -o android -a arm64-v8a packagedependencies-android.txt 




