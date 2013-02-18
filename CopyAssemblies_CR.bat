set ReferenceDir=%1
if "%ReferenceDir%"=="" set ReferenceDir=.\Reference

rem copy ..\Support\CrystalReport\2010\*.dll %ReferenceDir%
copy ..\Support\CrystalReport\2008\*.dll %ReferenceDir%

IF ERRORLEVEL 1 pause
