namespace Feng.Windows.Forms
{
    using System.ComponentModel;

    /// <summary>
    /// MyToolTip
    /// </summary>
    public class MyToolTip : System.Windows.Forms.ToolTip
    {
        #region "Default Property"

        /// <summary>
        /// Constructor
        /// </summary>
        public MyToolTip()
            : base()
        {
            base.AutoPopDelay = 5000;
            base.InitialDelay = 1000;
            base.IsBalloon = true;
            base.ReshowDelay = 500;
        }

        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="cont"></param>
        public MyToolTip(IContainer cont)
            : base(cont)
        {
            base.AutoPopDelay = 5000;
            base.InitialDelay = 1000;
            base.IsBalloon = true;
            base.ReshowDelay = 500;
        }

        /// <summary>
        /// AutoPopDelay
        /// </summary>
        [DefaultValue(5000)]
        public new int AutoPopDelay
        {
            get { return base.AutoPopDelay; }
            set { base.AutoPopDelay = value; }
        }

        /// <summary>
        /// InitialDelay
        /// </summary>
        [DefaultValue(1000)]
        public new int InitialDelay
        {
            get { return base.InitialDelay; }
            set { base.InitialDelay = value; }
        }

        /// <summary>
        /// IsBalloon
        /// </summary>
        [DefaultValue(true)]
        public new bool IsBalloon
        {
            get { return base.IsBalloon; }
            set { base.IsBalloon = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(500)]
        public new int ReshowDelay
        {
            get { return base.ReshowDelay; }
            set { base.ReshowDelay = value; }
        }

        #endregion
    }
}