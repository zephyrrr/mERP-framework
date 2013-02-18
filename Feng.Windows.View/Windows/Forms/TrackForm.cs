using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    public partial class TrackForm : Feng.Windows.Forms.GeneratedArchiveSeeForm
    {
        public TrackForm()
            : base(ADInfoBll.Instance.GetWindowInfo("SD_Track"))
        {
            InitializeComponent();

            this.MasterGrid.DataRowTemplate.Cells["VehicleName"].DoubleClick += new EventHandler(TrackForm_VehicleName_DoubleClick);
            this.MasterGrid.DataRowTemplate.Cells["StartTime"].DoubleClick += new EventHandler(TrackForm_StartTime_DoubleClick);

            var toolStrip = new ToolStrip();
            toolStrip.Items.Add("ToGPX", null, new EventHandler(delegate(object sender, EventArgs e) 
                {
                    if (this.MasterGrid.CurrentRow == null)
                        return;
                    Track track = this.MasterGrid.CurrentRow.Tag as Track;
                    if (track == null)
                        return;
                    var gpx = GpxConverter.ConvertToGpx(track);

                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.Filter = "GPX 文件(*.gpx)|*.gpx";
                    //saveFileDialog1.Title = "保存";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.FileName))
                        {
                            sw.WriteLine(gpx);
                        }
                    }
                    saveFileDialog1.Dispose();
                }));

            toolStrip.Items.Add("ToKML", null, new EventHandler(delegate(object sender, EventArgs e)
            {
                if (this.MasterGrid.CurrentRow == null)
                    return;
                Track track = this.MasterGrid.CurrentRow.Tag as Track;
                if (track == null)
                    return;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.Filter = "KML 文件(*.kml)|*.kml";
                //saveFileDialog1.Title = "保存";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Feng.Map.KmlHelper.GenerateTourKml(track, saveFileDialog1.FileName);
                }
                saveFileDialog1.Dispose();
            }));
            toolStrip.Items.Add("ToCSV", null, new EventHandler(delegate(object sender, EventArgs e)
            {
                if (this.MasterGrid.CurrentRow == null)
                    return;
                Track track = this.MasterGrid.CurrentRow.Tag as Track;
                if (track == null)
                    return;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.Filter = "CSV 文件(*.csv)|*.csv";
                //saveFileDialog1.Title = "保存";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.FileName))
                    {
                        sw.WriteLine("Time, X, Y");
                        var ps = TrackPointDao.GetTrackPoints(track);
                        foreach (var i in ps)
                        {
                            sw.WriteLine(string.Format("{0}, {2}, {1}", i.GpsTime.ToString("yyyy-MM-ddTHH:mm:ss"), i.Latitude, i.Longitude));
                        }
                    }
                }
                saveFileDialog1.Dispose();
            }));
            base.MergeToolStrip(toolStrip);
        }

        void TrackForm_StartTime_DoubleClick(object sender, EventArgs e)
        {
            Xceed.Grid.Cell cell = sender as Xceed.Grid.Cell;
            Track entity = cell.ParentRow.Tag as Track;

            if (entity != null)
            {
                WebForm form = new WebForm("车辆轨迹", string.Format("{0}/CarTrackService/TrackAnimation.aspx?TrackId={1}",
                     SystemConfiguration.Server, entity.ID.ToString()));
                form.ShowDialog();
            }
        }

        private Feng.Map.MapForm m_mapForm;
        void TrackForm_VehicleName_DoubleClick(object sender, EventArgs e)
        {
            Xceed.Grid.Cell cell = sender as Xceed.Grid.Cell;
            Track entity = cell.ParentRow.Tag as Track;

            if (m_mapForm == null)
            {
                m_mapForm = new Feng.Map.MapForm();
                m_mapForm.FormClosing += new FormClosingEventHandler(m_mapForm_FormClosing);
            }
            if (!m_mapForm.Visible)
            {
                m_mapForm.Show(this);
            }

            m_mapForm.ClearTrack();
            m_mapForm.ClearRoute();

            m_mapForm.LoadTrack(entity.ID);
        }

        void m_mapForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                m_mapForm.Visible = false;
            }
        }

    }
}
