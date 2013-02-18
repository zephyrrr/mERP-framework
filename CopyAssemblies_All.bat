call CopyAssemblies.bat .\Reference\Release
call CopyAssemblies.bat .\Reference

call CopyAssemblies_CR.bat .\Reference\Release
call CopyAssemblies_CR.bat .\Reference

call CopyAssemblies_Map.bat .\Reference\Release
call CopyAssemblies_Map.bat .\Reference

call CopyAssemblies_Server.bat .\Reference\Release
call CopyAssemblies_Server.bat .\Reference

call CopyAssemblies_Silverlight.bat .\Reference\Silverlight\Release
call CopyAssemblies_Silverlight.bat .\Reference\Silverlight

if "%PROCESSOR_ARCHITEW6432%" == "AMD64" goto 64BIT
goto END

:64BIT
call CopyAssemblies_x86.bat .\Reference\Release
call CopyAssemblies_x86.bat .\Reference

:END