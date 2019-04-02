::Usage: script.bat [debug|release(default)]

@ECHO OFF
CLS

PUSHD %~dp0

:: CS
ECHO Deploy wrapper scripts

::SET SRC="..\..\..\..\SolARWrapper\out\csharp"
SET SRC ="%BCOMDEVROOT%\..\source\SolARFramework\SolARWrapper\out\csharp"
SET DSTCs ="SolAR\Scripts\SolARFullWrapper\Swig"
SET DSTDll ="Plugins"

RMDIR /S /Q %DSTCs%
MKDIR %DSTCs%
ROBOCOPY %SRC% %DSTCs% /MIR

RMDIR /S /Q %DSTDll%
MKDIR %DSTDll%
ROBOCOPY %SRC% %DSTDll% /MIR

:: DLL
ECHO Deploy wrapper DLL

SET MODE=%1
IF "%MODE%" EQU "" (
SET MODE=release
)
ECHO MODE = %MODE%

::SET EXT=dll
SET EXT=*

PUSHD Plugins
SET DST="."

DEL *.dll *.lib *.pdb *.exp

:: SolAR
1>NUL COPY "%BCOMDEVROOT%\..\SolARFramework\build\%MODE%\SolARWrapper\SolARWrapper.%EXT%" %DSTCs%
1>NUL COPY "%BCOMDEVROOT%\bcomBuild\SolARFramework\0.5.0\lib\x86_64\shared\%MODE%\SolARFramework.%EXT%" %DSTDll%
1>NUL COPY "%BCOMDEVROOT%\thirdParties\xpcf\2.1.0\lib\x86_64\shared\%MODE%\xpcf.%EXT%" %DSTDll%
1>NUL COPY "%BCOMDEVROOT%\thirdParties\opencv\3.4.3\lib\x86_64\shared\%MODE%\opencv_world343.%EXT%" %DSTDll%

:: Modules
1>NUL COPY "%BCOMDEVROOT%\bcomBuild\SolARModuleFBOW\0.5.0\lib\x86_64\shared\%MODE%\SolARModuleFBOW.%EXT%" %DSTDll%
1>NUL COPY "%BCOMDEVROOT%\bcomBuild\SolARModuleNonFreeOpenCV\0.5.0\lib\x86_64\shared\%MODE%\SolARModuleNonFreeOpenCV.%EXT%" %DSTDll%
1>NUL COPY "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenCV\0.5.0\lib\x86_64\shared\%MODE%\SolARModuleOpenCV.%EXT%" %DSTDll%
1>NUL COPY "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenGL\0.5.0\lib\x86_64\shared\%MODE%\SolARModuleOpenGL.%EXT%" %DSTDll%
1>NUL COPY "%BCOMDEVROOT%\bcomBuild\SolARModuleTools\0.5.0\lib\x86_64\shared\%MODE%\SolARModuleTools.%EXT%" %DSTDll%

:: Boost
1>NUL COPY "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%MODE%\boost_context.%EXT%" %DSTDll%
1>NUL COPY "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%MODE%\boost_date_time.%EXT%" %DSTDll%
1>NUL COPY "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%MODE%\boost_fiber.%EXT%" %DSTDll%
1>NUL COPY "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%MODE%\boost_filesystem.%EXT%" %DSTDll%
1>NUL COPY "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%MODE%\boost_log.%EXT%" %DSTDll%
1>NUL COPY "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%MODE%\boost_system.%EXT%" %DSTDll%
1>NUL COPY "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%MODE%\boost_thread.%EXT%" %DSTDll%

POPD

POPD
