using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Utils
{
    public partial class QueryForm : Form
    {
        public QueryForm()
        {
            InitializeComponent();
        }

        public static readonly string[] KeyWordsSql = { "INSERT", "UPDATE", "DELETE", "CREATE", "ALTER", "DROP", "SELECT", "FROM", "WHERE", "GROUP", "ORDER", "BY", "INTO", "HAVING", "VALUES", "SET", "INNER", "LEFT", "RIGHT", "OUTER", "CROSS", "JOIN", "ON", "UNION", "IN", "AS", "AND", "OR", "DISTINCT",
                                        "TABLE","DEFAULT","NULL","NOT","CONSTRAINT","REFERERENCES","UNIQUE","INDEX","PRIMARY","KEY","CLUSTERED","NONCLUSTERED","ADD","FOREIGN","ASC","DESC","REFERENCES" };

        private void SetSyntax()
        {
            foreach (string keyWord in KeyWordsSql)
            {
                rtQuery.Settings.Keywords.Add(keyWord);
            }
            // Set the colors that will be used.
            rtQuery.Settings.KeywordColor = Color.Blue;
            rtQuery.Settings.CommentColor = Color.Green;
            rtQuery.Settings.StringColor = Color.Red;
            rtQuery.Settings.IntegerColor = Color.Red;

            // Let's not process strings and integers.
            rtQuery.Settings.EnableStrings = true;
            rtQuery.Settings.EnableIntegers = true;
            rtQuery.Settings.Comment = "--";
            rtQuery.Settings.CommentColor = Color.Green;
            rtQuery.Settings.EnableComments = true;
            // Let's make the settings we just set valid by compiling
            // the keywords to a regular expression.
            rtQuery.CompileKeywords();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string query = null;
            if (string.IsNullOrEmpty(rtQuery.SelectedText))
            {
                query = rtQuery.Text;
            }
            else
            {
                query = rtQuery.SelectedText;
            }
            if (string.IsNullOrEmpty(query))
            {
                return;
            }

            try
            {
                btnRun.Enabled = false;

                if (!ckbHql.Checked)
                {
                    DataTable dt = null;
                    if (!string.IsNullOrEmpty(txtConfig.Text))
                    {
                        dt = Feng.Data.DbHelper.CreateDatabase(txtConfig.Text).ExecuteDataTable(query);
                    }
                    else
                    {
                        dt = Feng.Data.DbHelper.Instance.ExecuteDataTable(query);
                    }
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    System.Collections.IList list = null;
                    using (Feng.NH.INHibernateRepository rep = new Feng.NH.Repository(txtConfig.Text))
                    {
                        list = rep.Session.CreateQuery(query).List();
                        dataGridView1.DataSource = list;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
            finally
            {
                btnRun.Enabled = true;
            }
        }

        private void QueryForm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
            dataGridView1.ReadOnly = true;

            SetSyntax();

            //var c = ServiceProvider.GetService<Feng.NH.ISessionFactoryManager>();
            //if (c != null)
            //{
            //    txtConfig.Text = c.GetDefaultConfiguration().GetProperty("Name");
            //}
            ckbHql.Checked = true;
        }

        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
        }
    }
}
