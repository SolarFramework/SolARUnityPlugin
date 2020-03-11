SET compiler=win-cl-14.1
SET config=release

echo off

echo Delete following pipeline manager wrapper files ?

del ".\Assets\SolAR\Swig\*.*" /S /Q

:: copy csharp interfaces
echo ---------------- copy c# ----------------------
timeout 2
echo "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARPipelineManager\0.7.0\CSharp\*" ".\Assets\SolAR\Swig\SolARPluginNovice\"
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARPipelineManager\0.7.0\CSharp\*" ".\Assets\SolAR\Swig\SolARPluginNovice\"

:: Bunlde all third parties in the ./Assets/plugins folder based on the packagedependencies.txt file. More information on remaken is available on https://github.com/b-com-software-basis/remaken 
remaken bundle -d ./Assets/Plugins -c %config% --cpp-std 17 -b cl-14.1 packagedependencies.txt

exit /B 0