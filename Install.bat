@echo off
SET config=release

echo ---------------- install third parties ----------------------
echo Install third parties
:: install all third parties in the %REMAKEN_PKG_ROOT%\packages. More information on remaken is available on https://github.com/b-com-software-basis/remaken 
remaken install -c %config% --cpp-std 17 -b cl-14.1 packagedependencies.txt
remaken install -c %config% --cpp-std 17 -b clang -o android -a arm64-v8a packagedependencies.txt
remaken install -c %config% --cpp-std 17 -b clang -o android -a arm64-v8a packagedependencies-android.txt

echo ---------------- bundle plugins ----------------------
call "Bundle.bat" %config%

exit /B 0