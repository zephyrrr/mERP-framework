set ReferenceDir=%1
if "%ReferenceDir%"=="" set ReferenceDir=.\Reference

mkdir %ReferenceDir%
mkdir %ReferenceDir%\zh-CHS
mkdir %ReferenceDir%\zh-CN

copy ..\Support\AMS\AMS.Profile.dll %ReferenceDir%

copy "..\Support\Microsoft EL\Microsoft.Practices.EnterpriseLibrary.Common.dll" %ReferenceDir%
copy "..\Support\Microsoft EL\Microsoft.Practices.EnterpriseLibrary.Data.dll" %ReferenceDir%
copy "..\Support\Microsoft EL\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll" %ReferenceDir%
copy "..\Support\Microsoft EL\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.dll" %ReferenceDir%
copy "..\Support\Microsoft EL\Microsoft.Practices.EnterpriseLibrary.Security.dll" %ReferenceDir%
copy "..\Support\Microsoft EL\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll" %ReferenceDir%
copy "..\Support\Microsoft EL\Microsoft.Practices.Unity.dll" %ReferenceDir%
copy "..\Support\Microsoft EL\Microsoft.Practices.Unity.Interception.dll" %ReferenceDir%
copy "..\Support\Microsoft EL\Microsoft.Practices.EnterpriseLibrary.Validation.dll" %ReferenceDir%

copy "..\Support\Microsoft EL\Silverlight\Microsoft.Practices.ServiceLocation.dll" %ReferenceDir%
copy "..\Support\Microsoft EL\Microsoft.Practices.ServiceLocation.SpringAdapter.dll" %ReferenceDir%
copy "..\Support\Microsoft EL\Microsoft.Practices.EnterpriseLibrary.Data.SQLite.dll" %ReferenceDir%


copy "..\Support\Microsoft\ChnCharInfo.dll" %ReferenceDir%
copy "..\Support\Microsoft\zh-CN\ChnCharInfo.resources.dll" %ReferenceDir%\zh-CN\
copy "..\Support\Microsoft\Microsoft.Data.ConnectionUI.dll" %ReferenceDir%
copy "..\Support\Microsoft\Microsoft.Data.ConnectionUI.Dialog.dll" %ReferenceDir%
copy "..\Support\Microsoft\zh-CHS\Microsoft.Data.ConnectionUI.Dialog.resources.dll" %ReferenceDir%\zh-CHS\

copy ..\Support\NHibernate\Antlr3.Runtime.dll %ReferenceDir%
copy ..\Support\NHibernate\Iesi.Collections.dll %ReferenceDir%
copy ..\Support\NHibernate\log4net.dll %ReferenceDir%
copy ..\Support\NHibernate\NHibernate.dll %ReferenceDir%
copy ..\Support\NHibernate\NHibernate.ByteCode.LinFu.dll %ReferenceDir%
copy ..\Support\NHibernate\LinFu.DynamicProxy.dll %ReferenceDir%
copy ..\Support\NHibernate\NHibernate.ByteCode.Castle.dll %ReferenceDir%
copy ..\Support\NHibernate\Castle.Core.dll %ReferenceDir%
copy ..\Support\NHibernate\Remotion.Data.Linq.dll %ReferenceDir%

copy ..\Support\NHibernate\NHibernate.Caches.SysCache.dll %ReferenceDir%
copy ..\Support\NHibernate\NHibernate.Mapping.Attributes.dll %ReferenceDir%

copy ..\Support\EvaluationEngine\EvaluationEngine.dll %ReferenceDir%

copy ..\Support\Spring.Net\Common.Logging.dll %ReferenceDir%
copy ..\Support\Spring.Net\Spring.Core.dll %ReferenceDir%

rem copy ..\Support\SQLite\*.dll %ReferenceDir%

copy ..\Support\Xceed\Xceed.DockingWindows.v2.2.dll %ReferenceDir%
copy ..\Support\Xceed\zh-CN\Xceed.DockingWindows.v2.2.resources.dll %ReferenceDir%\zh-CN
copy ..\Support\Xceed\Xceed.DockingWindows.Layout.v2.2.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.Editors.v2.6.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.Grid.v3.9.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.Grid.Reporting.v3.9.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.Pdf.v1.1.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.SmartUI.v3.6.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.SmartUI.Controls.v3.6.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.SmartUI.UIStyle.v3.6.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.SmartUI.UIStyle.WindowsXP.Silver.v3.6.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.UI.v1.4.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.UI.WindowsXP.Silver.v1.4.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.Validation.v1.3.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.Compression.v5.1.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.Compression.Formats.v5.1.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.FileSystem.v5.1.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.Ftp.v5.1.dll %ReferenceDir%
copy ..\Support\Xceed\Xceed.Zip.v5.1.dll %ReferenceDir%

copy ..\Support\IronPython\ipy.exe %ReferenceDir%
copy ..\Support\IronPython\IronPython.dll %ReferenceDir%
copy ..\Support\IronPython\IronPython.Modules.dll %ReferenceDir%
copy ..\Support\IronPython\Microsoft.Dynamic.dll %ReferenceDir%
copy ..\Support\IronPython\Microsoft.Scripting.dll %ReferenceDir%
copy ..\Support\IronPython\Microsoft.Scripting.Metadata.dll %ReferenceDir%

copy ..\Support\iTextSharp\itextsharp.dll %ReferenceDir%

rem copy "..\Support\Rest Starter Kit\Microsoft.Http.dll" %ReferenceDir%
rem copy "..\Support\Rest Starter Kit\Microsoft.Http.Extensions.dll" %ReferenceDir%
rem copy "..\Support\Rest Starter Kit\Microsoft.ServiceModel.Web.dll" %ReferenceDir%

copy "..\Support\WCF\System.Net.Http.dll" %ReferenceDir%

copy ..\Support\Json.Net\Newtonsoft.Json.dll %ReferenceDir%

copy ..\Support\SharpZipLib\ICSharpCode.SharpZipLib.dll %ReferenceDir%

copy ..\Support\neokernel\neokernel.bat %ReferenceDir%
copy ..\Support\neokernel\neokernel.exe %ReferenceDir%

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
