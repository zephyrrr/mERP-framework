set ConfigDir=.\Reference
set ConfigDir=.\Reference\Release

mkdir ..\Support\Feng

copy %ConfigDir%\Feng.*.dll ..\Support\Feng\
copy %ConfigDir%\Feng.Run.exe ..\Support\Feng\
copy %ConfigDir%\Feng.Updater.exe ..\Support\Feng\
copy %ConfigDir%\AdInfosUtil.exe.* ..\Support\Feng\
copy %ConfigDir%\CredentialsManager.exe ..\Support\Feng\

rem copy .\Feng.Help\*.xslt ..\Support\Feng\Help\
rem copy .\Feng.Help\image\true.png ..\Support\Feng\Help\html\image\
rem copy .\Feng.Help\style\help.css ..\Support\Feng\Help\html\style\


IF ERRORLEVEL 1 pause