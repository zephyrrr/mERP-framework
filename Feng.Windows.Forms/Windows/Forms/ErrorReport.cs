using System.Diagnostics;
using System;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Data;
using System.Collections.Generic;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// Error Icon Type
    /// </summary>
    public enum MessageImage
    {
        /// <summary>
        /// Error
        /// </summary>
        Error,
        /// <summary>
        /// Exclaim
        /// </summary>
        Exclaim,
        /// <summary>
        /// Information
        /// </summary>
        Information
    }

    /// <summary>
    /// The class is used to generate an error message dialog that shows
    /// the user the error message, stack trace, and also provides the
    /// user with the means to submit the error encountered back to
    /// developer by means of a web service that stores the error information
    /// in an sql server database.
    /// </summary>
    public partial class ErrorReport
    {
        #region "Declarations"

        private string mMessage;
        private string mTitle;
        private string mDetail;
        private MessageImage mMessageImage;

        #endregion

        #region "Constructors"

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="msgImage"></param>
        public ErrorReport(string message, string title, MessageImage msgImage)
            : this(message, title, msgImage, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="detail"></param>
        /// <param name="msgImage"></param>
        public ErrorReport(string message, string title, MessageImage msgImage, string detail)
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(this.Size.Width, panTop.Height + 30);

            mMessage = message;
            mTitle = title;
            mDetail = detail;

            txtErrorMsg.Text = mMessage;
            txtDetails.Text = mDetail;
            this.Text = mTitle;

            ErrorImage = msgImage;

            btnViewDetails.Visible = !string.IsNullOrEmpty(detail);
            this.MaximizeBox = !string.IsNullOrEmpty(detail);
        }

        #endregion

        #region "Methods"

        private void btnClose_Click(System.Object sender, System.EventArgs e)
        {
            this.Dispose();
        }

        private void btnViewDetails_Click(System.Object sender, System.EventArgs e)
        {
            if (btnViewDetails.Text == "ÏêÏ¸(&D)¡ý")
            {
                this.Size = new System.Drawing.Size(this.Size.Width, panTop.Height + 150 + 30);
                btnViewDetails.Text = "¼òµ¥(&D)¡ü";
            }
            else
            {
                this.Size = new System.Drawing.Size(this.Size.Width, panTop.Height + 30);
                btnViewDetails.Text = "ÏêÏ¸(&D)¡ý";
            }
        }

        private void btnSendBugReport_Click(System.Object sender, System.EventArgs e)
        {
            //try
            //{
            //    Logger.Error("Title:" + ErrorMessage + Environment.NewLine + ErrorDetail);
            //    ServiceProvider.GetService<IMessageBox>().ShowInfo("´íÎóÐÅÏ¢ÒÑ³É¹¦·¢ËÍ¡£");
            //}
            //catch (Exception ex)
            //{
            //    ExceptionProcess.ProcessWithResume(ex);
            //    ServiceProvider.GetService<IMessageBox>().ShowWarning("´íÎóÐÅÏ¢Î´·¢ËÍ£¡");
            //}
            Feng.Windows.Utils.ClipboardHelper.CopyTextToClipboard(txtDetails.Text);
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// ErrorMessage
        /// </summary>
        public string ErrorMessage
        {
            get { return mMessage; }
            set
            {
                mMessage = value;
                txtErrorMsg.Text = mMessage;
            }
        }

        /// <summary>
        /// Detail
        /// </summary>
        public string ErrorDetail
        {
            get { return mDetail; }
            set
            {
                mDetail = value;
                txtDetails.Text = mDetail;
            }
        }

        /// <summary>
        /// ErrorTitle
        /// </summary>
        public string ErrorTitle
        {
            get { return mTitle; }
            set
            {
                mTitle = value;
                this.Text = mTitle;
            }
        }

        /// <summary>
        /// ErrorImage
        /// </summary>
        public MessageImage ErrorImage
        {
            get { return mMessageImage; }
            set
            {
                mMessageImage = value;

                switch (mMessageImage)
                {
                    case MessageImage.Error:
                        this.picMsgPicture.Image = System.Drawing.SystemIcons.Error.ToBitmap();
                        break;
                    case MessageImage.Exclaim:
                        this.picMsgPicture.Image = System.Drawing.SystemIcons.Exclamation.ToBitmap();
                        break;
                    case MessageImage.Information:
                        this.picMsgPicture.Image = System.Drawing.SystemIcons.Information.ToBitmap();
                        break;
                    default:
                        this.picMsgPicture.Image = System.Drawing.SystemIcons.Error.ToBitmap();
                        break;
                }
            }
        }

        #endregion

        private void btnThrowException_Click(object sender, EventArgs e)
        {
            throw new InvalidOperationException();
        }
    }
}