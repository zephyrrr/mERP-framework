set ReferenceDir=%1
if "%ReferenceDir%"=="" set ReferenceDir=.\Reference\Release

mkdir %ReferenceDir%

rem copy ..\Support\GMap.Net\BSE.Windows.Forms.dll %ReferenceDir%
copy ..\Support\GMap.Net\GMap.NET.Core.dll %ReferenceDir%
copy ..\Support\GMap.Net\GMap.NET.WindowsForms.dll %ReferenceDir%

copy ..\Support\SQLite\System.Data.SQLite.dll %ReferenceDir% 

copy ..\Support\geobase\geobase.dll %ReferenceDir%
copy ..\Support\geobase\geobase.net.dll %ReferenceDir%

copy ..\Support\SharpMap\SharpMap.dll %ReferenceDir%
copy ..\Support\SharpMap\SharpMap.UI.dll %ReferenceDir%
copy ..\Support\SharpMap\SharpMap.Extensions.dll %ReferenceDir%
copy ..\Support\SharpMap\BruTile.dll %ReferenceDir%
copy ..\Support\SharpMap\ProjNet.dll %ReferenceDir%

IF ERRORLEVEL 1 pause
