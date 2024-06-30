set ver=0.1.0
set slnFolder=D:\dev\nuget\PowTrees
set nugetFolder=C:\Users\admin\.nuget\packages
set nugetUrl=https://api.nuget.org/v3/index.json


set prjName=PowTrees
rem =================
set nupkgFile=%slnFolder%\Libs\%prjName%\bin\Release\%prjName%.%ver%.nupkg
rmdir /s /q %nugetFolder%\%prjName%\%ver%
del /q %nupkgFile%
cd /d %slnFolder%\Libs\%prjName%
dotnet pack -p:version=%ver%
nuget add %nupkgFile% -source %nugetFolder% -expand
nuget push %nupkgFile% -Source %nugetUrl%
del /q %nupkgFile%


set prjName=PowTrees.LINQPad
rem ========================
set nupkgFile=%slnFolder%\Libs\%prjName%\bin\Release\%prjName%.%ver%.nupkg
rmdir /s /q %nugetFolder%\%prjName%\%ver%
del /q %nupkgFile%
cd /d %slnFolder%\Libs\%prjName%
dotnet pack -p:version=%ver%
nuget add %nupkgFile% -source %nugetFolder% -expand
nuget push %nupkgFile% -Source %nugetUrl%
del /q %nupkgFile%



cd %slnFolder%
