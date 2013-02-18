using System;
using System.Drawing;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// Summary description for SystemTrayNotificationEventType.
    /// Changed for ContentMenu to ContentMenuStrip
    /// </summary>
    public enum SystemTrayNotificationEventType
    {
        /// <summary>
        /// 
        /// </summary>
        Hiding,
        /// <summary>
        /// 
        /// </summary>
        Showing,
        /// <summary>
        /// 
        /// </summary>
        StartingAnimation,
        /// <summary>
        /// 
        /// </summary>
        StopingAnimation,
        /// <summary>
        /// 
        /// </summary>
        IconChanged,
        /// <summary>
        /// 
        /// </summary>
        Disposing
    } ;

    /// <summary>
    /// Summary description for SystemTrayNotificationEventArgs.
    /// </summary>
    public class SystemTrayNotificationEventArgs : EventArgs
    {
        private SystemTrayNotificationEventType state;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public SystemTrayNotificationEventArgs(SystemTrayNotificationEventType state)
            : base()
        {
            this.state = state;
        }

        /// <summary>
        /// 
        /// </summary>
        public SystemTrayNotificationEventType State
        {
            get { return state; }
        }
    }

    /// <summary>
    /// Summary description for SystemTrayNotifyIcon.
    /// </summary>
    public class SystemTrayNotifyIcon
    {
        private System.Windows.Forms.NotifyIcon notifyIcon = new NotifyIcon(); // NotifyIcon object
        private System.Windows.Forms.Form mainForm;
        private System.Drawing.Icon[] iconArray;
        private System.Drawing.Icon mainIcon;
        private System.Windows.Forms.Timer iconTimer;
        private int timerInterval = 200;
        private int iconCounter = 0;
        private int totalAnimations = 0;
        private int animationCounter = 0;
        private bool iconsLoaded = false;
        private bool keepAnimationAlive = false;

        // Declaring Event 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void StatusChanged(object sender, SystemTrayNotificationEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        public event StatusChanged OnStatusChanged;

        /// <summary>
        /// Internal NotifyIcon
        /// </summary>
        public NotifyIcon NotifyIcon
        {
            get { return notifyIcon; }
        }

        /// <summary>
        /// Property for showing and hiding icon
        /// </summary>
        public bool Visibility
        {
            get { return notifyIcon.Visible; }
            set
            {
                switch (value)
                {
                    case true:
                        if (notifyIcon.Visible == false)
                        {
                            OnStatusChanged(this,
                                            new SystemTrayNotificationEventArgs(SystemTrayNotificationEventType.Showing));
                        }
                        break;
                    case false:
                        if (notifyIcon.Visible == true)
                        {
                            OnStatusChanged(this,
                                            new SystemTrayNotificationEventArgs(SystemTrayNotificationEventType.Hiding));
                        }
                        break;
                }
                notifyIcon.Visible = value;
            }
        }

        /// <summary>
        /// Property
        /// </summary>
        public bool KeepAnimationAlive
        {
            get { return keepAnimationAlive; }
            set
            {
                switch (value)
                {
                    case true:
                        if (keepAnimationAlive == false)
                        {
                            OnStatusChanged(this,
                                            new SystemTrayNotificationEventArgs(
                                                SystemTrayNotificationEventType.StartingAnimation));
                        }
                        break;
                    case false:
                        if (keepAnimationAlive == true)
                        {
                            OnStatusChanged(this,
                                            new SystemTrayNotificationEventArgs(
                                                SystemTrayNotificationEventType.StopingAnimation));
                        }
                        break;
                }
                keepAnimationAlive = value;
            }
        }

        /// <summary>
        /// Overloaded Constructor -- 1 --
        /// Icon = Application Icon (Default),
        /// Tooltip = Application Name (Default),
        /// Visibility = Programmer must provide,
        /// </summary>
        public SystemTrayNotifyIcon(System.Windows.Forms.Form form, bool visible)
        {
            OnStatusChanged += new SystemTrayNotifyIcon.StatusChanged(SystemTrayNotificationHandler);
                // Setting Event Handler
            mainForm = form;
            notifyIcon.Visible = visible;
            notifyIcon.Icon = mainForm.Icon;
            notifyIcon.Text = mainForm.Text;
            iconTimer = new Timer();
            iconTimer.Interval = timerInterval;
            iconTimer.Tick += new EventHandler(TimerProc);
        }

        /// <summary>
        /// Overloaded Constructor -- 2 --
        /// Icon = Application Icon (Default),
        /// Tooltip = Programmer must provide,
        /// Visibility = Programmer must provide,
        /// </summary>
        public SystemTrayNotifyIcon(System.Windows.Forms.Form form, bool visible, string toolTip)
        {
            OnStatusChanged += new SystemTrayNotifyIcon.StatusChanged(SystemTrayNotificationHandler);
                // Setting Event Handler
            mainForm = form;
            notifyIcon.Visible = visible;
            notifyIcon.Icon = mainForm.Icon;
            notifyIcon.Text = toolTip;
            iconTimer = new Timer();
            iconTimer.Interval = timerInterval;
            iconTimer.Tick += new EventHandler(TimerProc);
        }

        /// <summary>
        /// Overloaded Constructor -- 3 --
        /// Icon = Programmer must provide,
        /// Tooltip = Programmer must provide,
        /// Visibility = Programmer must provide,
        /// </summary>
        public SystemTrayNotifyIcon(System.Windows.Forms.Form form, bool visible, string toolTip, Icon icon)
        {
            OnStatusChanged += new SystemTrayNotifyIcon.StatusChanged(SystemTrayNotificationHandler);
                // Setting Event Handler
            mainForm = form;
            notifyIcon.Visible = visible;
            if (icon.Size.Height > 16 || icon.Size.Width > 16)
            {
                notifyIcon.Icon = mainForm.Icon;
            }
            else
            {
                notifyIcon.Icon = icon;
            }
            notifyIcon.Text = toolTip;
            iconTimer = new Timer();
            iconTimer.Interval = timerInterval;
            iconTimer.Tick += new EventHandler(TimerProc);
        }

        /// <summary>
        /// Overloaded Constructor -- 4 --
        /// Icon = Application Icon (Default),
        /// Tooltip = Programmer must provide,
        /// Visibility = Programmer must provide,
        /// </summary>
        public SystemTrayNotifyIcon(System.Windows.Forms.Form form, bool visible, string toolTip,
                                    ContextMenuStrip contextMenu)
        {
            OnStatusChanged += new SystemTrayNotifyIcon.StatusChanged(SystemTrayNotificationHandler);
                // Setting Event Handler
            mainForm = form;
            notifyIcon.Visible = visible;
            notifyIcon.Icon = mainForm.Icon;
            notifyIcon.Text = toolTip;
            notifyIcon.ContextMenuStrip = contextMenu;
            iconTimer = new Timer();
            iconTimer.Interval = timerInterval;
            iconTimer.Tick += new EventHandler(TimerProc);
        }

        /// <summary>
        /// Overloaded Constructor -- 5 --
        /// Icon = Programmer must provide,
        /// Tooltip = Programmer must provide,
        /// Visibility = Programmer must provide,
        /// </summary>
        public SystemTrayNotifyIcon(System.Windows.Forms.Form form, bool visible, string toolTip, Icon icon,
                                    ContextMenuStrip contextMenu)
        {
            OnStatusChanged += new SystemTrayNotifyIcon.StatusChanged(SystemTrayNotificationHandler);
                // Setting Event Handler
            mainForm = form;
            notifyIcon.Visible = visible;
            if (icon.Size.Height > 16 || icon.Size.Width > 16)
            {
                notifyIcon.Icon = mainForm.Icon;
            }
            else
            {
                notifyIcon.Icon = icon;
            }
            notifyIcon.Text = toolTip;
            notifyIcon.ContextMenuStrip = contextMenu;
            iconTimer = new Timer();
            iconTimer.Interval = timerInterval;
            iconTimer.Tick += new EventHandler(TimerProc);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            OnStatusChanged(this, new SystemTrayNotificationEventArgs(SystemTrayNotificationEventType.Disposing));
            iconTimer.Tick -= new EventHandler(TimerProc);
            notifyIcon.Dispose();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~SystemTrayNotifyIcon()
        {
        }

        ///// <summary>
        ///// Loads a Default ContextMenu
        ///// </summary>
        //private ContextMenu LoadDefaultContextMenu()
        //{
        //    //BaseMenu = new System.Windows.Forms.ContextMenu();

        //    //// Adding menu items
        //    //BaseMenu.MenuItems.Add(new MenuItem("Hide Icon", new System.EventHandler(DefaultMenuHandler)));
        //    //BaseMenu.MenuItems.Add(new MenuItem("-", new System.EventHandler(DefaultMenuHandler)));
        //    //BaseMenu.MenuItems.Add(new MenuItem("Exit Application", new System.EventHandler(DefaultMenuHandler)));

        //    //return BaseMenu;
        //    return null;
        //}

        /// <summary>
        /// Default menu's Event Handler
        /// </summary>
        private void DefaultMenuHandler(object sender, System.EventArgs e)
        {
            try
            {
                switch (((MenuItem) sender).Text)
                {
                    case "Animate":
                        Animate(-1, 100);
                        break;
                    case "Stop Animation":
                        OnStatusChanged(this,
                                        new SystemTrayNotificationEventArgs(
                                            SystemTrayNotificationEventType.StopingAnimation));
                        keepAnimationAlive = false;
                        break;
                    case "Hide Icon":
                        OnStatusChanged(this,
                                        new SystemTrayNotificationEventArgs(SystemTrayNotificationEventType.Hiding));
                        notifyIcon.Visible = false;
                        break;
                    case "Exit Application":
                        notifyIcon.Visible = false;
                        mainForm.Close();
                        break;
                }
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show(err.Message, "Error");
            }
        }

        /// <summary>
        /// Animation Initializer for showing animation in System Tray -- 
        /// nTimes = Programmer must provide,
        /// timeInterval = 1000 equals 1 Second (Default: 200).
        /// </summary>
        public void Animate(int nTimes)
        {
            if (!iconsLoaded)
            {
                System.Windows.Forms.MessageBox.Show("LoadIcons() must be called before Animate().", "Warning!", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if ((nTimes == -1) || (nTimes > 0))
            {
                totalAnimations = nTimes;
                OnStatusChanged(this,
                                new SystemTrayNotificationEventArgs(SystemTrayNotificationEventType.StartingAnimation));
                keepAnimationAlive = true;
                iconTimer.Start();
            }
        }

        /// <summary>
        /// Overloaded Animation Initializer for showing animation in System Tray -- 
        /// nTimes = Programmer must provide,
        /// timeInterval = Programmer must provide (1000 equals 1 Second) (Limit 50 - 50000).
        /// </summary>
        public void Animate(int nTimes, int timerinterval)
        {
            timerInterval = (timerinterval > 50000 || timerinterval < 50) ? 200 : timerinterval;
            if (!iconsLoaded)
            {
                System.Windows.Forms.MessageBox.Show("LoadIcons() must be called before Animate().", "Warning!", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if ((nTimes == -1) || (nTimes > 0))
            {
                totalAnimations = nTimes;
                iconTimer.Interval = timerInterval;
                OnStatusChanged(this,
                                new SystemTrayNotificationEventArgs(SystemTrayNotificationEventType.StartingAnimation));
                keepAnimationAlive = true;
                iconTimer.Start();
            }
        }

        /// <summary>
        /// Loads default icons of size 16x16 for animation,
        /// </summary>
        public void LoadIcons(System.Drawing.Icon[] iconarray)
        {
            iconsLoaded = true;
            iconCounter = 0;
            totalAnimations = 0;
            mainIcon = notifyIcon.Icon;
            iconArray = iconarray;

            //for (int cnt = 0; cnt < iconArray.Length; cnt++)
            //{
            //    try
            //    {
            //        if (iconArray[cnt].Size.Height > 16 || iconArray[cnt].Size.Width > 16)
            //            throw new ArgumentException("You should use 16*16 Icons!");
            //            //iconArray[cnt] = mainForm.Icon;
            //    }
            //    catch (Exception e)
            //    {
            //        MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
        }

        /// <summary>
        /// Timer proc for showing animation effect in System Tray
        /// </summary>
        private void TimerProc(object sender, EventArgs e)
        {
            if (keepAnimationAlive == false)
            {
                iconTimer.Stop();
                iconCounter = 0;
                animationCounter = 0;
                notifyIcon.Icon = mainIcon;
            }
            else
            {
                OnStatusChanged(this, new SystemTrayNotificationEventArgs(SystemTrayNotificationEventType.IconChanged));
                notifyIcon.Icon = iconArray[iconCounter++];
                if (iconCounter == iconArray.Length)
                {
                    iconCounter = 0;
                    animationCounter++;
                }

                if ((animationCounter == totalAnimations) && (totalAnimations != -1))
                {
                    animationCounter = 0;
                    totalAnimations = 0;
                    OnStatusChanged(this,
                                    new SystemTrayNotificationEventArgs(SystemTrayNotificationEventType.StopingAnimation));
                    keepAnimationAlive = false;
                }
            }
        }

        /// <summary>
        /// Default Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SystemTrayNotificationHandler(object sender, SystemTrayNotificationEventArgs e)
        {
            // This event handler is not required here exactly
            // It just facilitates the programmer to save him/her from
            // exceptions if he/she don't provides his/her own EventHandler
            // function
        }
    }
}