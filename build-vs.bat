rem mkdir "C:\Program Files\Microsoft SDKs\Windows\v6.0A\lib"

rem del .\Reference\Feng.*
rem del .\Reference\Release\Feng.*

call "D:\Program Files\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" 

msbuild /p:configuration="Release" /p:Platform="Any CPU" Feng.All.sln
rem msbuild /p:configuration="Debug" /p:Platform="Any CPU" Feng.All.sln

rem msbuild /verbosity:n build-vs.proj
rem msbuild /verbosity:n build-vs.debug.proj

IF ERRORLEVEL 1 "C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" msbuild /p:configuration="Release" /p:Platform="Any CPU" Feng.All.sln
IF ERRORLEVEL 1 "C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" msbuild /p:configuration="Debug" /p:Platform="Any CPU" Feng.All.sln


pause