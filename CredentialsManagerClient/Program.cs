// ?2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CredentialsManagerClient
{
   static class Program
   {
      [STAThread]
      static void Main(string[] args)
      {
          //System.Windows.Forms.MessageBox.Show("Start");

          Application.EnableVisualStyles();

          try
          {
              if (args.Length == 2 && args[0] == "-applicationName")
              {
                  Application.Run(new CredentialsManagerForm(args[1]));
              }
              else if (args.Length == 1 && args[0] == "-all")
              {
                  Application.Run(new CredentialsManagerForm());
              }
          }
          catch (Exception ex)
          {
              System.Windows.Forms.MessageBox.Show(ex.Message, "Error");
          }
      }
   }
}