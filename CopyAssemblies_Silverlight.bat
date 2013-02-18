set ReferenceDir=%1
if "%ReferenceDir%"=="" set ReferenceDir=.\Reference\Silverlight

mkdir %ReferenceDir%

copy ..\Support\XceedSilverlight\Xceed.Silverlight.Controls.v1.3.dll %ReferenceDir%
copy ..\Support\XceedSilverlight\Xceed.Silverlight.Data.v1.3.dll %ReferenceDir%
copy ..\Support\XceedSilverlight\Xceed.Silverlight.Data.RiaServices.v1.3.dll %ReferenceDir%
copy ..\Support\XceedSilverlight\Xceed.Silverlight.DataGrid.Themes.v1.3.dll %ReferenceDir%
copy ..\Support\XceedSilverlight\Xceed.Silverlight.DataGrid.v1.3.dll %ReferenceDir%

copy "..\Support\WCF\Microsoft.Http.Silverlight.dll" %ReferenceDir%
copy "..\Support\Json.Net\Newtonsoft.Json.Silverlight.dll" %ReferenceDir%

copy "..\Support\IronPython\Silverlight\IronPython.dll" %ReferenceDir%
copy "..\Support\IronPython\Silverlight\IronPython.Modules.dll" %ReferenceDir%
copy "..\Support\IronPython\Silverlight\Microsoft.Dynamic.dll" %ReferenceDir%
copy "..\Support\IronPython\Silverlight\Microsoft.Scripting.dll" %ReferenceDir%
copy "..\Support\IronPython\Silverlight\Microsoft.Scripting.Silverlight.dll" %ReferenceDir%

rem copy ".\Reference\Feng.Base.dll" %ReferenceDir%
rem copy ".\Reference\Feng.Model.dll" %ReferenceDir%
rem copy ".\Reference\Feng.View.dll" %ReferenceDir%
rem copy ".\Reference\Feng.Controller.dll" %ReferenceDir%

IF ERRORLEVEL 1 pause
