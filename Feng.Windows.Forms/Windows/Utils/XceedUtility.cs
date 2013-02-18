using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// Utility for Xceed
    /// </summary>
    public sealed class XceedUtility
    {
        static XceedUtility()
        {
            DefaultUIStyle = "ResourceAssembly";
            DefaultUIStyleResourceAssembly = "Silver";
        }

        /// <summary>
        /// DefaultStyle
        /// </summary>
        public static string DefaultUIStyle
        {
            get;
            set;
        }

        /// <summary>
        /// DefaultUIStyleResourceAssembly
        /// </summary>
        public static string DefaultUIStyleResourceAssembly
        {
            get;
            set;
        }

        private static Assembly TryLoadAssembly(string assemblyFileName)
        {
            if (System.IO.File.Exists(string.Format("{0}\\{1}.dll", SystemDirectory.WorkDirectory, assemblyFileName)))
            {
                return System.Reflection.Assembly.Load(assemblyFileName);
            }
            return null;
        }

        public static void SetUIStyle(Xceed.SmartUI.SmartControl uiControl)
        {
            try
            {
                switch (DefaultUIStyle)
                {
                    case "WindowsClassic":
                        uiControl.UIStyle = Xceed.SmartUI.UIStyle.UIStyle.WindowsClassic;
                        break;
                    case "WindowsXP":
                        uiControl.UIStyle = Xceed.SmartUI.UIStyle.UIStyle.WindowsXP;
                        break;
                    case "OfficeXP":
                        uiControl.UIStyle = Xceed.SmartUI.UIStyle.UIStyle.OfficeXP;
                        break;
                    case "Office2003":
                        uiControl.UIStyle = Xceed.SmartUI.UIStyle.UIStyle.Office2003;
                        break;
                    case "ResourceAssembly":
                        uiControl.UIStyle = Xceed.SmartUI.UIStyle.UIStyle.ResourceAssembly;
                        System.Reflection.Assembly resourceAssembly = null;
                        switch (DefaultUIStyleResourceAssembly)
                        {
                            case "Blue":
                                resourceAssembly = TryLoadAssembly("Xceed.SmartUI.UIStyle.WindowsXP.Blue");
                                break;
                            case "OliveGreen":
                                resourceAssembly = TryLoadAssembly("Xceed.SmartUI.UIStyle.WindowsXP.OliveGreen");
                                break;
                            case "Silver":
                                resourceAssembly = TryLoadAssembly("Xceed.SmartUI.UIStyle.WindowsXP.Silver");
                                break;
                            default:
                                throw new NotSupportedException("Not supported UIStyleResourceAssembly!");
                        }
                        if (resourceAssembly != null)
                        {
                            uiControl.UIStyleResourceAssembly = resourceAssembly;
                        }
                        break;
                    default:
                        throw new NotSupportedException("Not supported UIStyle!");
                }
            }
            // maybe not support
            catch (NotSupportedException)
            {
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
        }

        public static void SetUIStyle(Xceed.UI.ThemedScrollableControl uiControl)
        {
            try
            {
                switch (DefaultUIStyle)
                {
                    case "WindowsClassic":
                        uiControl.UIStyle = Xceed.UI.UIStyle.WindowsClassic;
                        break;
                    case "WindowsXP":
                        uiControl.UIStyle = Xceed.UI.UIStyle.WindowsXP;
                        break;
                    case "System":
                        uiControl.UIStyle = Xceed.UI.UIStyle.System;
                        break;
                    case "ResourceAssembly":
                        uiControl.UIStyle = Xceed.UI.UIStyle.ResourceAssembly;
                        System.Reflection.Assembly resourceAssembly = null;
                        switch (DefaultUIStyleResourceAssembly)
                        {
                            case "Blue":
                                resourceAssembly = TryLoadAssembly("Xceed.UI.WindowsXP.Blue");
                                break;
                            case "OliveGreen":
                                resourceAssembly = TryLoadAssembly("Xceed.UI.WindowsXP.OliveGreen");
                                break;
                            case "Silver":
                                resourceAssembly = TryLoadAssembly("Xceed.UI.WindowsXP.Silver");
                                break;
                            default:
                                throw new ArgumentException("Not supported UIStyleResourceAssembly!");
                        }
                        if (resourceAssembly != null)
                        {
                            uiControl.ResourceAssembly = resourceAssembly;
                        }
                        break;
                    default:
                        throw new ArgumentException("Not supported UIStyle!");
                }
            }
            // maybe not support
            catch (NotSupportedException)
            {
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
        }

        public static void SetUIStyle(Xceed.UI.ThemedControl uiControl)
        {
            try
            {
                switch (DefaultUIStyle)
                {
                    case "WindowsClassic":
                        uiControl.UIStyle = Xceed.UI.UIStyle.WindowsClassic;
                        break;
                    case "WindowsXP":
                        uiControl.UIStyle = Xceed.UI.UIStyle.WindowsXP;
                        break;
                    case "System":
                        uiControl.UIStyle = Xceed.UI.UIStyle.System;
                        break;
                    case "ResourceAssembly":
                        uiControl.UIStyle = Xceed.UI.UIStyle.ResourceAssembly;
                        System.Reflection.Assembly resourceAssembly = null;
                        switch (DefaultUIStyleResourceAssembly)
                        {
                            case "Blue":
                                resourceAssembly = TryLoadAssembly("Xceed.UI.WindowsXP.Blue");
                                break;
                            case "OliveGreen":
                                resourceAssembly = TryLoadAssembly("Xceed.UI.WindowsXP.OliveGreen");
                                break;
                            case "Silver":
                                resourceAssembly = TryLoadAssembly("Xceed.UI.WindowsXP.Silver");
                                break;
                            default:
                                throw new ArgumentException("Not supported UIStyleResourceAssembly!");
                        }
                        if (resourceAssembly != null)
                        {
                            uiControl.ResourceAssembly = resourceAssembly;
                        }
                        break;
                    default:
                        throw new ArgumentException("Not supported UIStyle!");
                }
            }
            // maybe not support
            catch (NotSupportedException)
            {
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
        }

        private static bool m_licenseSetted = false;
        /// <summary>
        /// ³õÊ¼»¯XceedµÄLicense
        /// </summary>
        public static void SetXceedLicense()
        {
            if (!m_licenseSetted)
            {
                XceedLicense.SetXceedLicense("Xceed.Grid", "Xceed.Grid.v3.9", "GRD39-X8SPR-GY5R1-HHLA");
                XceedLicense.SetXceedLicense("Xceed.DockingWindows", "Xceed.DockingWindows.v2.2", "DWN22-J8EY3-9NL5S-JWLA");
                XceedLicense.SetXceedLicense("Xceed.Editors", "Xceed.Editors.v2.6", "EDN26-JR0P3-KZ166-LHLA");
                XceedLicense.SetXceedLicense("Xceed.Validation", "Xceed.Validation.v1.3", "IVN13-L81U0-3RLN8-1WTA");
                XceedLicense.SetXceedLicense("Xceed.SmartUI", "Xceed.SmartUI.v3.6", "SUN36-J76TZ-PNJY8-3HNA");
                XceedLicense.SetXceedLicense("Xceed.Chart", "Xceed.Chart.v4.3", "CHT43-B839R-MX0NH-0W2A");
                m_licenseSetted = true;
            }
        }
    }
}