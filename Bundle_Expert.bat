SET compiler=win-cl-14.1
SET config=release

echo off

echo Delete following pipeline manager wrapper files ?

del ".\Assets\SolAR\Swig\*.*" /S /Q

:: copy csharp interfaces
echo ---------------- copy c# ----------------------
timeout 2
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARPipelineManager\0.7.0\CSharp\*" ".\Assets\SolAR\Swig\SolARPluginNovice\"

xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\*" ".\Assets\SolAR\Swig\SolARPluginExpert\" /S /EXCLUDE:excludedFile_Bat.txt
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\SolAR\Core\*" ".\Assets\SolAR\Swig\Utilities\Core\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\SolAR\Datastructure\Vector3f.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\SolAR\Datastructure\Transform3Df.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\SolAR\Datastructure\Matrix3x3f.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\SolAR\Datastructure\solar_datastructurePINVOKE.cs" ".\Assets\SolAR\Swig\Utilities\" /S
xcopy "%REMAKEN_PKG_ROOT%\packages\SolARBuild\%compiler%\SolARWrapper\csharp\SolAR\Datastructure\solar_datastructure.cs" ".\Assets\SolAR\Swig\Utilities\" /S

:: Bunlde all third parties in the ./Assets/plugins folder based on the packagedependencies_expert.txt file. More information on remaken is available on https://github.com/b-com-software-basis/remaken 
remaken --action bundle -f packagedependencies_Expert.txt -d ./Assets/Plugins -c %config% --cpp-std 17 -b cl-14.1

exit /B 0