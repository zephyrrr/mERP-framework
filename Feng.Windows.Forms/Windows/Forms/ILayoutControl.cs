using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// Control for Layout Load&Save
    /// </summary>
    public interface ILayoutControl
    {
        /// <summary>
        /// SaveLayout
        /// </summary>
        bool SaveLayout();

        /// <summary>
        /// LoadLayout
        /// </summary>
        bool LoadLayout();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IProfileLayoutControl
    {
        /// <summary>
        /// 
        /// </summary>
        string LayoutFilePath
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        bool LoadLayout(AMS.Profile.IProfile profile);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        bool SaveLayout(AMS.Profile.IProfile profile);
    }

    /// <summary>
    /// 
    /// </summary>
    public static class LayoutControlExtention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        public static bool LoadLayout(this IProfileLayoutControl layout)
        {
            if (string.IsNullOrEmpty(layout.LayoutFilePath))
                return false;

            AMS.Profile.IProfile profile = new AMS.Profile.Xml(SystemDirectory.GetDirectory(DataDirectoryType.User, layout.LayoutFilePath));
            if (layout.LoadLayout(profile))
            {
                return true;
            }
            else
            {
                profile = new AMS.Profile.Xml(SystemDirectory.GetDirectory(DataDirectoryType.Global, layout.LayoutFilePath));
                return layout.LoadLayout(profile);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        public static bool SaveLayout(this IProfileLayoutControl layout)
        {
            if (string.IsNullOrEmpty(layout.LayoutFilePath))
                return false;

            string fileName = SystemDirectory.GetDirectory(DataDirectoryType.User, layout.LayoutFilePath);
            Feng.Utils.IOHelper.TryCreateDirectory(fileName);
            AMS.Profile.IProfile profile = new AMS.Profile.Xml(fileName);
            return layout.SaveLayout(profile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool LoadLayout(this IProfileLayoutControl layout, string fileName)
        {
            return layout.LoadLayout(new AMS.Profile.Xml(fileName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool SaveLayout(this IProfileLayoutControl layout, string fileName)
        {
            return layout.SaveLayout(new AMS.Profile.Xml(fileName));
        }
    }
}
