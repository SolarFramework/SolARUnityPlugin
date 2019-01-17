echo off
:: remove existing dll and pdb files
del .\*.dll
del .\*.pdb
:: remove existing pipeline manager C# wrapper files
echo Delete following pipeline manager wrapper files ?
del ".\Assets\Standard Assets\SolAR\PipelineManagerWrapper\*.*"

:: copy csharp interfaces
xcopy "%BCOMDEVROOT%\bcombuild\SolARPipelineManager\0.1.0\CSharp\*" ".\Assets\Standard Assets\SolAR\PipelineManagerWrapper\"

:: copy dll
SET mode="release"
IF "%1"=="-debug" (SET mode="debug")
echo ------------copy third parties dll------------
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_context.dll" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_date_time.dll" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_fiber.dll" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_filesystem.dll" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_log.dll" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_system.dll" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_thread.dll" .\

xcopy "%BCOMDEVROOT%\thirdParties\xpcf\2.1.0\lib\x86_64\shared\%mode%\*.dll" .\
xcopy "%BCOMDEVROOT%\thirdParties\opencv\3.4.3\lib\x86_64\shared\%mode%\opencv_world343.dll" .\
xcopy "%BCOMDEVROOT%\thirdParties\freeglut\3.0.0\lib\x86_64\shared\%mode%\*.dll" .\

echo --------------copy framework dll--------------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARFramework\0.5.1\lib\x86_64\shared\%mode%\*.dll" .\

echo -----------copy pipeline Manager dll----------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARPipelineManager\0.1.0\lib\x86_64\shared\%mode%\*.dll" .\

echo ---------------copy modules dll---------------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenCV\0.5.1\lib\x86_64\shared\%mode%\*.dll" .\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenGL\0.5.1\lib\x86_64\shared\%mode%\*.dll" .\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleTools\0.5.1\lib\x86_64\shared\%mode%\*.dll" .\

echo --------------copy pipelines dll--------------
xcopy "%BCOMDEVROOT%\bcomBuild\PipelineFiducialMarker\0.1.0\lib\x86_64\shared\%mode%\*.dll" .\

IF "%1"=="-debug" (GOTO Debug)
exit /B 0

:: copy .pdb
:Debug
echo ------------copy third parties pdb------------
::xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\*.pdb" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_context.pdb" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_date_time.pdb" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_fiber.pdb" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_filesystem.pdb" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_log.pdb" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_system.pdb" .\
xcopy "%BCOMDEVROOT%\thirdParties\boost\1.68.0\lib\x86_64\shared\%mode%\boost_thread.pdb" .\
xcopy "%BCOMDEVROOT%\thirdParties\xpcf\2.1.0\lib\x86_64\shared\%mode%\*.pdb" .\
xcopy "%BCOMDEVROOT%\thirdParties\opencv\3.4.3\lib\x86_64\shared\%mode%\opencv_world343.pdb" .\
xcopy "%BCOMDEVROOT%\thirdParties\freeglut\3.0.0\lib\x86_64\shared\%mode%\*.pdb" .\

echo --------------copy framework pdb--------------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARFramework\0.5.1\lib\x86_64\shared\%mode%\*.pdb" .\

echo -----------copy pipeline Manager pdb----------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARPipelineManager\0.1.0\lib\x86_64\shared\%mode%\*.pdb" .\

echo ---------------copy modules pdb---------------
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenCV\0.5.1\lib\x86_64\shared\%mode%\*.pdb" .\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleOpenGL\0.5.1\lib\x86_64\shared\%mode%\*.pdb" .\
xcopy "%BCOMDEVROOT%\bcomBuild\SolARModuleTools\0.5.1\lib\x86_64\shared\%mode%\*.pdb" .\

echo --------------copy pipelines pdb--------------
xcopy "%BCOMDEVROOT%\bcomBuild\PipelineFiducialMarker\0.1.0\lib\x86_64\shared\%mode%\*.pdb" .\

exit /B 0