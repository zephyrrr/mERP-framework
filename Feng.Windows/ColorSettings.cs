using System;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml;
using System.Drawing;

namespace Feng.Windows
{
    /// <summary>
    /// 定义警示标志颜色
    /// <section name="ColorSettings" type="Feng.ColorSettings, Feng.Windows.Utils" />
    /// <ColorSettings Critical="255, 0, 0" Error="255, 255, 0" Warning="200, 200, 20" Information="150, 150, 40" Verbose="100, 100, 80" Disable="112, 112, 112" DisableHalf="192, 192, 192" LocatedRow="255, 128, 255" />
    /// </summary>
	public sealed class ColorSettings : ConfigurationSection 
    {
        static ColorSettings()
        {
            try
            {
                s_instance = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection("ColorSettings") as ColorSettings;
                if (s_instance == null)
                {
                    s_instance = new ColorSettings();
                    s_instance.Critical = Color.FromArgb(255, 0, 0);
                    s_instance.Error = Color.FromArgb(255, 255, 0);
                    s_instance.Warning = Color.FromArgb(200, 200, 20);
                    s_instance.Information = Color.FromArgb(150, 150, 40);
                    s_instance.Verbose = Color.FromArgb(100, 100, 80);
                    s_instance.Disable = Color.FromArgb(112, 112, 112);
                    s_instance.DisableHalf = Color.FromArgb(192, 192, 192);
                    s_instance.LocatedRow = Color.FromArgb(255, 128, 255);
                }
            }
            catch(Exception)
            {
            }
        }
		/// <summary>
		/// Consturctor
		/// </summary>
		public ColorSettings()
		{
		}

        private static ColorSettings s_instance;
		/// <summary>
		/// 设置
		/// </summary>
		public static ColorSettings Setting
		{
			get { return s_instance; }
		}

        /// <summary>
		/// 严重错误
        /// </summary>
		[ConfigurationProperty("Critical")]
		public Color Critical
		{
			get { return (Color)this["Critical"]; }
			set { this["Critical"] = value; }
		}

		/// <summary>
		/// 一般错误
		/// </summary>
		[ConfigurationProperty("Error")]
		public Color Error
		{
			get { return (Color)this["Error"]; }
			set { this["Error"] = value; }
		}

		/// <summary>
		/// 警示
		/// </summary>
		[ConfigurationProperty("Warning")]
		public Color Warning
		{
			get { return (Color)this["Warning"]; }
			set { this["Warning"] = value; }
		}

		/// <summary>
		/// 一般信息
		/// </summary>
		[ConfigurationProperty("Information")]
		public Color Information
		{
			get { return (Color)this["Information"]; }
			set { this["Information"] = value; }
		}

		/// <summary>
		/// 详细信息
		/// </summary>
		[ConfigurationProperty("Verbose")]
		public Color Verbose
		{
			get { return (Color)this["Verbose"]; }
			set { this["Verbose"] = value; }
		}

		/// <summary>
		/// 不可写
		/// </summary>
		[ConfigurationProperty("Disable")]
		public Color Disable
		{
			get { return (Color)this["Disable"]; }
			set { this["Disable"] = value; }
		}

		/// <summary>
		/// 警示性不可写
		/// </summary>
		[ConfigurationProperty("DisableHalf")]
		public Color DisableHalf
		{
			get { return (Color)this["DisableHalf"]; }
			set { this["DisableHalf"] = value; }
		}

		/// <summary>
		/// 指定行
		/// </summary>
		[ConfigurationProperty("LocatedRow")]
		public Color LocatedRow
		{
			get { return (Color)this["LocatedRow"]; }
			set { this["LocatedRow"] = value; }
		}

		//public void TrySave()
		//{
		//    ColorSettings configData = new ColorSettings();

		//    configData.Critical = Color.FromArgb(100, 90, 80);

		//    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

		//    config.Sections.Remove("ColorSettings");
		//    config.Sections.Add("ColorSettings", configData);
		//    config.Save();
		//}

		///// <summary>
		///// This property contains a list of plugins
		///// </summary>
		//[ConfigurationProperty(PluginCollectionProperty)]
		//public PluginDataCollection Plugins
		//{
		//    get
		//    {
		//        return (PluginDataCollection)base[PluginCollectionProperty];
		//    }
		//}

    }

	//public class PluginDataCollection : PolymorphicConfigurationElementCollection<PluginData> // System.Configuration.ConfigurationElementCollection //
	//{
	//    /// <summary>
	//    /// Returns the <see cref="ConfigurationElement"/> type to created for the current xml node.
	//    /// </summary>
	//    /// <remarks>
	//    /// The <see cref="PluginData"/> include the configuration object type as a serialized attribute.
	//    /// </remarks>
	//    /// <param name="reader"></param>
	//    protected override Type RetrieveConfigurationElementType(XmlReader reader)
	//    {
	//        return typeof(PluginData);
	//    }
	//}

	//public class PluginData : NameTypeConfigurationElement
	//{
	//    public const string PluginTypeProperty = "plugintype";

	//    public const string PluginConfigProperty = "cfgfile";

	//    private static IDictionary<string, string> emptyAttributes = new Dictionary<string, string>(0);

	//    public PluginData()
	//    {
	//    }

	//    protected PluginData(string name, Type PluginType)
	//        : base(name, PluginType)
	//    {
	//        this.PluginType = this.GetType();
	//    }

	//    [ConfigurationProperty(PluginConfigProperty, DefaultValue = "", IsRequired = false)]
	//    public string ConfigFile
	//    {
	//        get
	//        {
	//            return (string)this[PluginConfigProperty];
	//        }
	//        set
	//        {
	//            this[PluginConfigProperty] = value;
	//        }
	//    }


	//    [ConfigurationProperty(PluginTypeProperty, IsRequired = true)]
	//    [TypeConverter(typeof(AssemblyQualifiedTypeNameConverter))]
	//    public Type PluginType
	//    {
	//        get
	//        {
	//            return (Type)this[PluginTypeProperty];
	//        }

	//        set
	//        {
	//            this[PluginTypeProperty] = value;
	//        }
	//    }
	//}

}
