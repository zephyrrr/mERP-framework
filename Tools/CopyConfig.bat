set ConfigDir=.\Feng.Example

copy %ConfigDir%\app.config .\Reference\Feng.Run.exe.config
rem copy %ConfigDir%\release.config .\Reference\Feng.Run.exe.config
copy %ConfigDir%\example.config .\Reference

copy %ConfigDir%\ADInfosUtil.exe.config .\Reference\

copy %ConfigDir%\neokernel.exe.config .\Reference\
copy %ConfigDir%\mime_types .\Reference\
copy %ConfigDir%\neokernel_props.xml .\Reference\

copy %ConfigDir%\ipy.exe.config .\Reference\ipy.exe.config

.\Reference\ipy.exe .\Tools\encrypt_connectionstring.py


IF ERRORLEVEL 1 pause