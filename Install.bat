@ECHO OFF
SET CONFIG=release

ECHO ---------------- install third parties ----------------------
ECHO Install third parties
:: install all third parties in the %REMAKEN_PKG_ROOT%\packages. More information on remaken is available on https://github.com/b-com-software-basis/remaken 
REMAKEN install -c %CONFIG% --cpp-std 17 -b cl-14.1 packagedependencies.txt
::REMAKEN install -c %CONFIG% --cpp-std 17 -b clang -o android -a arm64-v8a packagedependencies.txt
::REMAKEN install -c %CONFIG% --cpp-std 17 -b clang -o android -a arm64-v8a packagedependencies-android.txt

ECHO ---------------- bundle plugins ----------------------
CALL "Bundle.bat" %CONFIG%

EXIT /B 0
