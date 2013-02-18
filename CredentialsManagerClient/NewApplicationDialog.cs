// © 2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using CredentialsManagerClient.Properties;

namespace CredentialsManagerClient
{
   partial class CreateApplicationDialog : Form
   {
      string m_Url;
      string[] m_ExistingApplications;
      List<string> m_NewApplications = new List<string>();

      public string[] Applications
      {
         get
         {
            return m_NewApplications.ToArray();
         }
         set
         {
            m_NewApplications = new List<string>(value);
         }
      }

      public CreateApplicationDialog(string url,string[] applications)
      {
         InitializeComponent();

         m_ExistingApplications = applications;
         m_Url = url;

         m_ApplicationTextBox.Focus();
         m_CreatedApplicationsListView.SmallImageList = new ImageList();
         m_CreatedApplicationsListView.SmallImageList.Images.Add(Resources.Application);

         m_ApplicationTextBox.Focus();
      }
      void OnCreateApplication(object sender,EventArgs e)
      {
         string application = m_ApplicationTextBox.Text;
         Predicate<string> exists = delegate(string appToMatch)
                           {
                              return application == appToMatch;
                           };
         if(Array.Exists(m_ExistingApplications,exists))
         {
            m_Validator.SetError(m_ApplicationTextBox,"Application already exists");
            return;
         }
         m_Validator.Clear();
         if(application == String.Empty)
         {
            m_Validator.SetError(m_ApplicationTextBox,"The application name cannot be empty");
            return;
         }
         m_Validator.Clear();
         m_NewApplications.Add(application);
         m_CreatedApplicationsListView.AddItem(m_ApplicationTextBox.Text,true);

         m_ApplicationTextBox.Text = String.Empty;
         m_ApplicationTextBox.Focus();
      }
      void OnClose(object sender,EventArgs e)
      {
         Close();
      }
   }
}