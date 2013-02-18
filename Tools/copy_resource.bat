mkdir .\Reference\ClientResource
del /Q .\Reference\ClientResource\* 

rem Copy Script
cd PythonScript
if exist pythonFiles.txt del pythonFiles.txt>nul
dir /s /b *.py > pythonFiles.txt
for /f %%i in (pythonFiles.txt ) do copy %%i ..\Reference\ClientResource
del pythonFiles.txt>nul
cd ..

rem Copy Assembly
cd Reference
copy hd.*.dll ..\Reference\ClientResource
cd..

rem Copy Report
cd Hd.Report
copy *.xsd ..\Reference\ClientResource
copy *.rpt ..\Reference\ClientResource
copy *.rdlc ..\Reference\ClientResource
cd..

rem Copy Config
cd Hd.Run
copy hd.model.*.config ..\Reference\ClientResource
copy sessionfactory.config ..\Reference\ClientResource
cd..

rem CopyConfigToRun
set ConfigDir=.\Hd.Run

copy %ConfigDir%\app.config .\Reference\Feng.Run.exe.config
copy %ConfigDir%\release.config .\Reference\Feng.Run.exe.config
copy %ConfigDir%\hd.model.*.config .\Reference

copy %ConfigDir%\ADInfosUtil.exe.config .\Reference\

copy %ConfigDir%\neokernel.exe.config .\Reference\
copy %ConfigDir%\mime_types .\Reference\
copy %ConfigDir%\neokernel_props.xml .\Reference\

copy %ConfigDir%\ipy.exe.config .\Reference\ipy.exe.config

.\Reference\ipy.exe .\encrypt_connectionstring.py

rem Upload Resource
.\Reference\ipy.exe .\upload_resource.py


IF ERRORLEVEL 1 pause