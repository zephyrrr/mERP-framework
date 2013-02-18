set ReferenceDir=%1
if "%ReferenceDir%"=="" set ReferenceDir=.\Reference

mkdir %ReferenceDir%

copy ..\Support\SQLite\x64\System.Data.SQLite.dll %ReferenceDir%
copy ..\Support\SQLite\x64\sqlite3.dll %ReferenceDir%

copy ..\Support\geobase\x64\geobase64.dll %ReferenceDir%

IF ERRORLEVEL 1 pause
