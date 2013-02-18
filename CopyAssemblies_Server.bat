set ReferenceDir=%1
if "%ReferenceDir%"=="" set ReferenceDir=.\Reference

mkdir %ReferenceDir%

copy ..\Support\neokernel\neokernel.dll %ReferenceDir%
copy ..\Support\neokernel\neokernel.exe %ReferenceDir%
copy ..\Support\neokernel\neokernel.bat %ReferenceDir%
copy ..\Support\neokernel\neokernel.dll %ReferenceDir%

copy ..\Support\ComponentArt\ComponentArt.SOA.UI.dll %ReferenceDir%

IF ERRORLEVEL 1 pause
