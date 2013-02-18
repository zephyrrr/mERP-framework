set ReferenceDir=%1
if "%ReferenceDir%"=="" set ReferenceDir=.\Reference\Release

mkdir ..\Support\Feng

copy %ReferenceDir%\Feng.*.dll ..\Support\Feng\
copy %ReferenceDir%\Feng.*.pdb ..\Support\Feng\
copy %ReferenceDir%\Feng.Run.exe ..\Support\Feng\
copy %ReferenceDir%\Feng.Updater.exe ..\Support\Feng\
copy %ReferenceDir%\AdInfosUtil.exe.* ..\Support\Feng\
copy %ReferenceDir%\CredentialsManager.exe ..\Support\Feng\
copy %ReferenceDir%\MapUtil.exe ..\Support\Feng\
copy %ReferenceDir%\NeokernelService.exe ..\Support\Feng\
copy %ReferenceDir%\NeokernelService.Admin.exe ..\Support\Feng\

rem copy %ReferenceDir%\Silverlight\Feng.Script.Silverlight.dll ..\Support\Feng\
rem copy %ReferenceDir%\Silverlight\Feng.Silverlight.Controller.dll ..\Support\Feng\
rem copy %ReferenceDir%\Silverlight\Feng.Silverlight.Model.dll ..\Support\Feng\
rem copy %ReferenceDir%\Silverlight\Feng.Silverlight.XceedSilverlightData.dll ..\Support\Feng\

rem copy .\Feng.Help\*.xslt ..\Support\Feng\Help\
rem copy .\Feng.Help\image\true.png ..\Support\Feng\Help\html\image\
rem copy .\Feng.Help\style\help.css ..\Support\Feng\Help\html\style\


IF ERRORLEVEL 1 pause