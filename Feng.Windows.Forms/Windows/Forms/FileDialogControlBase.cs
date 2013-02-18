//  Copyright (c) 2006, Gustavo Franco
//  Copyright ?Decebal Mihailescu 2007
//  Email:  dmihailescu@hotmail.com
//  Email:  gustavo_franco@hotmail.com
//  All rights reserved.

//  Redistribution and use in source and binary forms, with or without modification, 
//  are permitted provided that the following conditions are met:

//  Redistributions of source code must retain the above copyright notice, 
//  this list of conditions and the following disclaimer. 
//  Redistributions in binary form must reproduce the above copyright notice, 
//  this list of conditions and the following disclaimer in the documentation 
//  and/or other materials provided with the distribution. 

//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE. IT CAN BE DISTRIBUTED FREE OF CHARGE AS LONG AS THIS HEADER 
//  REMAINS UNCHANGED.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using Feng.Windows.Forms;

namespace Feng.Windows.Forms
{
    #region Base class
    
    public partial class FileDialogControlBase : UserControl//, IMessageFilter
    {
        #region Delegates
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="filePath"></param>
        public delegate void PathChangedEventHandler(IWin32Window sender, string filePath);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="index"></param>
        public delegate void FilterChangedEventHandler(IWin32Window sender, int index);      
        #endregion

        #region Events
        //for weird reasons the designer wants the events public not protected   
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        public event PathChangedEventHandler EventFileNameChanged;

        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        public event PathChangedEventHandler EventFolderNameChanged;

        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        public event FilterChangedEventHandler EventFilterChanged;

        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        public event System.ComponentModel.CancelEventHandler EventClosingDialog;
        #endregion

        #region Constants Declaration
        private const SetWindowPosFlags UFLAGSHIDE =
            SetWindowPosFlags.SWP_NOACTIVATE |
            SetWindowPosFlags.SWP_NOOWNERZORDER |
            SetWindowPosFlags.SWP_NOMOVE |
            SetWindowPosFlags.SWP_NOSIZE |
            SetWindowPosFlags.SWP_HIDEWINDOW;
        #endregion

        #region Variables Declaration
        System.Windows.Forms.FileDialog _MSdialog;
        NativeWindow _dlgWrapper;
        private AddonWindowLocation _StartLocation = AddonWindowLocation.Right;
        private FolderViewMode _DefaultViewMode = FolderViewMode.Default;
        IntPtr _hFileDialogHandle = IntPtr.Zero;
        FileDialogType _FileDlgType;
        string _InitialDirectory = string.Empty;
        string _Filter = "All files (*.*)|*.*";
        string _DefaultExt = "jpg";
        string _FileName = string.Empty;
        string _Caption = "Save";
        string _OKCaption = "&Open";
        int _FilterIndex = 1;
        bool _AddExtension = true;
        bool _CheckFileExists = true;
        bool _EnableOkBtn = true;
        bool _DereferenceLinks = true;
        bool _ShowHelp;
        RECT _OpenDialogWindowRect = new RECT();
        IntPtr _hOKButton = IntPtr.Zero;
        private bool _hasRunInitMSDialog;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        IntPtr _hListViewPtr;

        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public FileDialogControlBase()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public FileDialog MSDialog
        {
            set { _MSdialog = value; }
            get { return _MSdialog; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue(AddonWindowLocation.Right)]
        public AddonWindowLocation FileDlgStartLocation
        {
            get { return _StartLocation; }
            set
            {
                _StartLocation = value;
                if (DesignMode)
                {
                    this.Refresh();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue(FolderViewMode.Default)]
        public FolderViewMode FileDlgDefaultViewMode
        {
            get { return _DefaultViewMode; }
            set { _DefaultViewMode = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue(FileDialogType.OpenFileDlg)]
        public FileDialogType FileDlgType
        {
            get { return _FileDlgType; }
            set { _FileDlgType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue("")]
        public string FileDlgInitialDirectory
        {
            get { return DesignMode?_InitialDirectory:MSDialog.InitialDirectory; }
            set { _InitialDirectory = value;
                    if (!DesignMode)
                        MSDialog.InitialDirectory = value;
                }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue("")]
        public string FileDlgFileName
        {
            get { return DesignMode?_FileName:MSDialog.FileName; }
            set { _FileName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue("")]
        public string FileDlgCaption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue("&Open")]
        public string FileDlgOkCaption
        {
            get { return _OKCaption; }
            set { _OKCaption = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue("jpg")]
        public string FileDlgDefaultExt
        {
            get { return DesignMode?_DefaultExt:MSDialog.DefaultExt; }
            set { _DefaultExt = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue("All files (*.*)|*.*")]
        public string FileDlgFilter
        {
            get { return DesignMode?_Filter:MSDialog.Filter; }
            set { _Filter = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue(1)]
        public int FileDlgFilterIndex
        {
            get { return DesignMode?_FilterIndex:MSDialog.FilterIndex; }
            set { _FilterIndex = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue(true)]
        public bool FileDlgAddExtension
        {
            get { return DesignMode?_AddExtension:MSDialog.AddExtension; }
            set { _AddExtension = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue(true)]
        public bool FileDlgEnableOkBtn
        {
            get { return _EnableOkBtn; }
            set { _EnableOkBtn = value;
                    if(!DesignMode && MSDialog != null && _hOKButton != IntPtr.Zero)
                        Feng.Windows.Forms.NativeMethods.EnableWindow(_hOKButton, _EnableOkBtn);
                }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue(true)]
        public bool FileDlgCheckFileExists
        {
            get { return DesignMode?_CheckFileExists:MSDialog.CheckFileExists; }
            set 
            { _CheckFileExists = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue(false)]
        public bool FileDlgShowHelp
        {
            get { return DesignMode ? _ShowHelp : MSDialog.ShowHelp; }
            set { _ShowHelp = value; }  
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("FileDialogExtenders")]
        [DefaultValue(true)]
        public bool FileDlgDereferenceLinks
        {
            get { return DesignMode ? _DereferenceLinks : MSDialog.DereferenceLinks; }
            set { _DereferenceLinks = value; }  
        }
        #endregion

        #region Virtuals

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
            {
                if (MSDialog != null)
                {
                    MSDialog.FileOk += new System.ComponentModel.CancelEventHandler(FileDialogControlBase_ClosingDialog);
                    MSDialog.Disposed += new EventHandler(FileDialogControlBase_DialogDisposed);
                    MSDialog.HelpRequest += new EventHandler(FileDialogControlBase_HelpRequest);
                    //Feng.Windows.Forms.NativeMethods.EnableWindow(_hOKButton, _EnableOKBtn);
                    FileDlgEnableOkBtn = _EnableOkBtn;//that's desigh time value
                    NativeMethods.SetWindowText(_dlgWrapper.Handle, _Caption);

                    //will work only for open dialog, save dialog will not update
                    NativeMethods.SetWindowText(_hOKButton, _OKCaption);//SetDlgItemText fails too 
                    //bool res = NativeMethods.SetDlgItemText(_FileDialogHandle, (int)ControlsId.ButtonOk, _OKCaption);
                }
            } 
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;
            if (MSDialog != null)
            {
                MSDialog.FileOk -= new System.ComponentModel.CancelEventHandler(FileDialogControlBase_ClosingDialog);
                MSDialog.Disposed -= new EventHandler(FileDialogControlBase_DialogDisposed);
                //if (MSDialog.ShowHelp)
                MSDialog.HelpRequest -= new EventHandler(FileDialogControlBase_HelpRequest);
                MSDialog.Dispose();
                MSDialog = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fileName"></param>
        public virtual void OnFileNameChanged(IWin32Window sender, string fileName)
        {
            if (EventFileNameChanged != null)
                EventFileNameChanged(sender, fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="folderName"></param>
        public void OnFolderNameChanged(IWin32Window sender, string folderName)
        {
            if (EventFolderNameChanged != null)
                EventFolderNameChanged(sender, folderName);
            UpdateListView();
        }

        private void UpdateListView()
        {
            _hListViewPtr = Feng.Windows.Forms.NativeMethods.GetDlgItem(_hFileDialogHandle, (int)ControlsId.DefaultView);
            if (FileDlgDefaultViewMode != FolderViewMode.Default && _hFileDialogHandle != IntPtr.Zero)
                NativeMethods.SendMessage(new HandleRef(this, _hListViewPtr), (int)Msg.WM_COMMAND, (IntPtr)(int)FileDlgDefaultViewMode, IntPtr.Zero);
        }

        internal void OnFilterChanged(IWin32Window sender, int index)
        {
            if (EventFilterChanged != null)
                EventFilterChanged(sender, index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                Graphics gr = e.Graphics;
                {
                    HatchBrush hb = null;                   
                    Pen p = null;
                    try
                    {
                        switch (this.FileDlgStartLocation)
                        {
                            case AddonWindowLocation.Right:
                                hb = new System.Drawing.Drawing2D.HatchBrush(HatchStyle.NarrowHorizontal, Color.Black, Color.Red);
                                p = new Pen(hb, 5);
                                gr.DrawLine(p, 0, 0, 0, this.Height);
                                break;
                            case AddonWindowLocation.Bottom:
                                hb = new System.Drawing.Drawing2D.HatchBrush(HatchStyle.NarrowVertical, Color.Black, Color.Red);
                                p = new Pen(hb, 5);
                                gr.DrawLine(p, 0, 0, this.Width, 0);
                                break;
                            case AddonWindowLocation.BottomRight:
                            default:
                                hb = new System.Drawing.Drawing2D.HatchBrush(HatchStyle.Sphere, Color.Black, Color.Red);
                                p = new Pen(hb, 5);
                                gr.DrawLine(p, 0, 0, 4, 4);
                                break;
                        }
                    }
                    finally
                    {
                        if (p != null)
                            p.Dispose();
                        if (hb != null)
                            hb.Dispose();
                    }
                }
            }
            base.OnPaint(e);
        }


        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DialogResult ShowDialog()
        {
            return ShowDialog(null);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnPrepareMSDialog()
        {
            InitMSDialog();
        }
        private void InitMSDialog()
        {
            MSDialog.InitialDirectory = _InitialDirectory.Length == 0 ? Path.GetDirectoryName(Application.ExecutablePath) : _InitialDirectory;
            MSDialog.AddExtension = _AddExtension;
            MSDialog.Filter = _Filter;
            MSDialog.FilterIndex = _FilterIndex;
            MSDialog.CheckFileExists = _CheckFileExists;
            MSDialog.DefaultExt = _DefaultExt;
            MSDialog.FileName = _FileName;
            MSDialog.DereferenceLinks = _DereferenceLinks;
            MSDialog.ShowHelp = _ShowHelp;
            MSDialog.RestoreDirectory = true;
            _hasRunInitMSDialog = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public DialogResult ShowDialog(IWin32Window owner)
        {
            DialogResult returnDialogResult = DialogResult.Cancel;
            if(this.IsDisposed)
                return returnDialogResult;
            if (FileDlgType == FileDialogType.OpenFileDlg)
            {
                _dlgWrapper = new DialogWrapper<OpenFileDialog>(this);
            }
            else
            {
                _dlgWrapper = new DialogWrapper<SaveFileDialog>(this);
            }
            OnPrepareMSDialog();
            if(!_hasRunInitMSDialog)
                InitMSDialog();
            try
            {
                returnDialogResult = _MSdialog.ShowDialog(owner);
            }
            // Sometimes if you open a animated .gif on the preview and the Form is closed, .Net class throw an exception
            // Lets ignore this exception and keep closing the form.
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("unable to get the modal dialog handle", ex.Message);
            }
            return returnDialogResult;
        }
        #endregion


        #region event handlers
        void FileDialogControlBase_DialogDisposed(object sender, EventArgs e)
        {
            Dispose(true);
        }

        private void FileDialogControlBase_ClosingDialog(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (EventClosingDialog != null)
            {
                EventClosingDialog(this, e);
            }
        }
        #endregion

        void FileDialogControlBase_HelpRequest(object sender, EventArgs e)
        {
            //this is a virtual call tahht should call the event in the subclass
            OnHelpRequested(new HelpEventArgs(new Point()));
        }

    }
    #endregion


}