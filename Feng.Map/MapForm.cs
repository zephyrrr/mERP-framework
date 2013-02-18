using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace Feng.Map
{
    [CLSCompliant(false)]
    public partial class MapForm : Form
    {
        public MapForm()
        {
            InitializeComponent();

            tsmReadGpsData.Visible = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (m_timer != null)
            {
                m_timer.Enabled = false;
            }
            base.Dispose(disposing);
        }

        private Feng.Map.MapControl m_MainMap = new Feng.Map.MapControl();
        //private System.Windows.Forms.DataGridView m_grid = new DataGridView();

        public Feng.Map.MapControl MainMap
        {
            get { return m_MainMap; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //MainMap.EnableLocalCache = false;

            m_MainMap.Dock = DockStyle.Fill;
            this.Controls.Add(m_MainMap);

            m_trackOverlay = new GMapOverlay(m_MainMap, "tracks");
            m_MainMap.Overlays.Add(m_trackOverlay);
        }

        private GMapOverlay m_trackOverlay;

        public GMapOverlay TrackOverlay
        {
            get { return m_trackOverlay; }
        }
        //private void DrawCircle(PointLatLng point, double radius = 0.0005, int segments = 100)
        //{
        //    List<PointLatLng> gpollist = new List<PointLatLng>();

        //    double seg = Math.PI * 2 / segments;

        //    for (int i = 0; i < segments; i++)
        //    {
        //        double theta = seg * i;
        //        double a = point.Lat + Math.Cos(theta) * radius;
        //        double b = point.Lng + Math.Sin(theta) * radius;

        //        PointLatLng gpoi = new PointLatLng(a, b);

        //        gpollist.Add(gpoi);
        //    }
        //    GMapPolygon gpol = new GMapPolygon(gpollist, "currentPoint");
        //    gpol.Fill = new SolidBrush(Color.Red);
        //    m_trackOverlay.Polygons.Add(gpol);
        //}

        public void ClearMap()
        {
            this.ClearTrack();
            this.ClearRoute();
        }

        public void ClearTrack()
        {
            if (m_trackOverlay != null)
            {
                // TrackPoint
                m_trackOverlay.Routes.Clear();
                m_trackOverlay.Markers.Clear();
                m_trackOverlay.Polygons.Clear();
            }
        }
        public void ClearRoute()
        {
            if (m_routeRoad != null)
            {
                m_routeRoad.Clear(); 
            }
            if (m_routeSite != null)
            {
                m_routeSite.Clear();
            }
        }

        public void LoadTrackPoint(IList<TrackPoint> ps)
        {
            foreach (var p in ps)
            {
                var m = new GMap.NET.WindowsForms.Markers.GMapMarkerCircle(new PointLatLng(p.Latitude, p.Longitude));
                m.Radius = 3;
                m_trackOverlay.Markers.Add(m);
            }
        }

        public void LoadTrack(long trackId)
        {
            var track = TrackDao.GetTrack(trackId);
            LoadTrack(track);
        }

        private void LoadTrack(Track track)
        {
            m_currentTrack = track;
            if (track == null)
                return;

            if (string.IsNullOrEmpty(track.Gpx))
            {
                LoadTrackData(track);
            }
            else
            {
                LoadGpx(track.Gpx);
            }
        }

        private SharpMapMarker m_routeRoad, m_routeSite;
        /// <summary>
        /// 显示理论路线
        /// </summary>
        /// <param name="routeName"></param>
        private void LoadRoute(string routeName)
        {
            if (m_routeRoad == null)
            {
                string fileName = System.IO.Directory.GetCurrentDirectory() + "\\" + MapHelper.ShpFile4Road;
                if (System.IO.File.Exists(fileName))
                {
                    m_routeRoad = this.m_MainMap.AddSharpMarker(fileName);
                    m_routeRoad.Layer.Style.Line = new System.Drawing.Pen(System.Drawing.Color.Blue, 2f);
                    m_routeRoad.Layer.Style.Line.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                }
            }
            else
            {
                m_routeRoad.Clear();
            }
            if (m_routeSite == null)
            {
                string fileName = System.IO.Directory.GetCurrentDirectory() + "\\" + MapHelper.ShpFile4Site;
                if (System.IO.File.Exists(fileName))
                {
                    m_routeSite = this.m_MainMap.AddSharpMarker(fileName);
                    m_routeSite.Layer.Style.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
                    m_routeSite.Layer.Style.Line = new System.Drawing.Pen(System.Drawing.Color.Green, 2f);
                    m_routeSite.Layer.Style.Line.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                }
            }
            else
            {
                m_routeSite.Clear();
            }

            Route route = RouteDao.GetRoute(routeName);
            if (m_routeRoad != null && route != null && tsm显示理论路径.Checked)
            {
                if (!string.IsNullOrEmpty(route.DirectionReal))
                {
                    m_routeRoad.ShowFeature(new Predicate<DataRow>(p =>
                    {
                        return Feng.Utils.RouteHelper.IsInRoute(route.DirectionReal, p[MapHelper.AddrColumnName].ToString());
                    }));
                }
            }

            if (m_routeSite != null && tsm显示重要地点.Checked)
            {
                m_routeSite.ShowFeature();
                //m_routeSite.ShowFeature(new Predicate<DataRow>(p => Array.IndexOf<string>(ss, p[MapHelper.AddrColumnName].ToString()) != -1));
            }
        }

        private void LoadTrackData(Track track, bool clearFirst = false)
        {
            Feng.Async.AsyncHelper.Start(() =>
                {
                    IList<TrackPoint> trackPoints = TrackPointDao.GetTrackPoints(track);
                    if (trackPoints.Count == 0)
                    {
                        if (track.StartTime.HasValue)
                        {
                            if (!track.EndTime.HasValue)
                                track.EndTime = DateTime.MaxValue;

                            trackPoints = TrackPointDao.GetTrackPoints(track.VehicleName, track.StartTime.Value, track.EndTime.Value);
                        }
                    }
                    return trackPoints;
                }, (result) =>
                    {
                        var trackPoints = result as IList<TrackPoint>;
                        List<PointLatLng> points = new List<GMap.NET.PointLatLng>();
                        foreach (var p in trackPoints)
                        {
                            points.Add(new PointLatLng(p.Latitude, p.Longitude));
                        }
                        if (clearFirst)
                        {
                            ClearTrack();
                        }

                        DarwTrack(points);

                        if (trackPoints.Count > 0)
                        {
                            //DrawCircle();
                            var p = trackPoints[trackPoints.Count - 1];
                            var p2 = new PointLatLng(p.Latitude, p.Longitude);
                            var m = new GMap.NET.WindowsForms.Markers.GMapMarkerCircle(p2);
                            m_trackOverlay.Markers.Add(m);

                            m_MainMap.Position = p2;
                        }

                        if (tsm显示路线点地址.Checked)
                        {
                            // Show Start, End
                            for (int i = 0; i < trackPoints.Count; i += trackPoints.Count - 1)
                            {
                                var p = trackPoints[i];
                                var m = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleRed(new PointLatLng(p.Latitude, p.Longitude));
                                m.ToolTipText = (i == 0 ? "Start" : "End") + ", " + p.GpsTime.ToLongTimeString();
                                m_trackOverlay.Markers.Add(m);
                            }

                            try
                            {
                                DrawTrackPointAddr(trackPoints);
                            }
                            catch (Exception)
                            {
                            }
                        }

                        if (tsm显示路线点.Checked)
                        {
                            IList<WayPoint> wayPoint = WayPointDao.GetWaypoints(track);

                            foreach (var p in wayPoint)
                            {
                                if (!string.IsNullOrEmpty(p.Action))
                                {
                                    var m = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleRed(new PointLatLng(p.Latitude, p.Longitude));
                                    m.ToolTipText = p.Action + "," + p.GpsTime.ToLongTimeString();
                                    m_trackOverlay.Markers.Add(m);
                                }
                            }
                        }

                        if (!track.EndTime.HasValue)
                        {
                            if (m_timer == null)
                            {
                                m_timer = new System.Timers.Timer(30 * 1000);
                                m_timer.Elapsed += new System.Timers.ElapsedEventHandler(m_timer_Elapsed);
                            }

                            m_timer.AutoReset = true;
                            m_timer.Enabled = true;
                        }

                        if (track.Route != null)
                        {
                            LoadRoute(track.Route.Name);
                        }
                    });
        }

        void m_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (m_currentTrack == null)
                return;

            try
            {
                m_timer.Enabled = false;
                RefreshMap(m_currentTrack);
                m_timer.Enabled = true;
            }
            catch (Exception)
            {
            }

            if (m_currentTrack.EndTime.HasValue)
            {
                m_timer.Enabled = false;
            }
        }
        private System.Timers.Timer m_timer;

        private Track m_currentTrack;
        private void RefreshMap(Track track)
        {
            if (!this.Visible)
                return;

            if (track != null)
            {
                if (!this.IsDisposed)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        LoadTrackData(track, true);
                    }));
                }
            }
        }
        private void LoadGpx(string gpxData)
        {
            gpxType[] rs = GpxImporter.importGPXFile(gpxData);
            if (rs != null && rs.Length > 0)
            {
                gpxType r = rs[0];
                if (r != null)
                {
                    if (r.trk != null && r.trk.Length > 0)
                    {
                        foreach (var trk in r.trk)
                        {
                            List<PointLatLng> points = new List<PointLatLng>();

                            foreach (var seg in trk.trkseg)
                            {
                                foreach (var p in seg.trkpt)
                                {
                                    if (p.lat != 0.0m && p.lon != 0.0m)
                                    {
                                        points.Add(new PointLatLng((double)p.lat, (double)p.lon));
                                    }
                                }
                            }

                            DarwTrack(points);
                        }
                    }

                    if (r.wpt != null && r.wpt.Length > 0)
                    {
                        List<PointLatLng> points = new List<PointLatLng>();
                        foreach (var p in r.wpt)
                        {
                            if (p.lat != 0.0m && p.lon != 0.0m)
                            {
                                points.Add(new PointLatLng((double)p.lat, (double)p.lon));
                            }
                        }
                        DarwTrack(points);
                    }
                }
            }

            m_MainMap.ZoomAndCenterRoutes(null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void DarwTrack(List<PointLatLng> points)
        {
            List<PointLatLng> ps = new List<PointLatLng>();
            for (int i = 0; i < points.Count; ++i)
            {
                // 5km
                if (i == 0 || this.m_MainMap.MapProvider.Projection.GetDistance(points[i], points[i - 1]) < 5)
                {
                    ps.Add(points[i]);
                    if (i != points.Count - 1)
                        continue;
                }

                GMapRoute rt = new GMapRoute(ps, string.Empty);
                {
                    rt.Stroke = new Pen(Color.FromArgb(144, Color.Red));
                    rt.Stroke.Width = 5;
                    rt.Stroke.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                }
                m_trackOverlay.Routes.Add(rt);

                ps = new List<PointLatLng>();
                ps.Add(points[i]);
            }
            
            if (tsm显示路线点.Checked)
            {
                DrawTrackPoint();
            }
        }

        private void DrawTrackPoint()
        {
            foreach (var route in m_trackOverlay.Routes)
            {
                foreach (var p in route.Points)
                {
                    var m = new GMap.NET.WindowsForms.Markers.GMapMarkerCross(new PointLatLng(p.Lat, p.Lng));
                    //m.ToolTipText = p.LocalGpsTime.ToLongTimeString();
                    m_trackOverlay.Markers.Add(m);
                }
            }
        }

        private void DrawTrackPointAddr(IList<TrackPoint> data)
        {
            string oldAddress = string.Empty;
            string oldRoad = string.Empty;
            foreach (var p in data)
            {
                string address = MapHelper.GetAreaFromGpsData(p);

                if (!string.IsNullOrEmpty(address) && address != oldAddress)
                {
                    oldAddress = address;
                    // Marker
                    //var m = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleGreen(Feng.Map.GoogleMapOffset.GetOffseted(new PointLatLng(p.Lat, p.Lon)));
                    var m = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleGreen(new PointLatLng(p.Latitude, p.Longitude));

                    m.ToolTipText = address + "," + p.GpsTime.ToLongTimeString();
                    m_trackOverlay.Markers.Add(m);
                }

                string road = MapHelper.GetRoadFromGpsData(p);
                if (!string.IsNullOrEmpty(road) && road != oldRoad)
                {
                    oldRoad = road;
                    //var m = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleRed(Feng.Map.GoogleMapOffset.GetOffseted(new PointLatLng(p.Lat, p.Lon)));
                    var m = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleGreen(new PointLatLng(p.Latitude, p.Longitude));

                    m.ToolTipText = road + "," + p.GpsTime.ToLongTimeString();
                    m_trackOverlay.Markers.Add(m);
                }
            }
        }

        //private bool m_showGpsDataAction = true;
        ///// <summary>
        ///// 是否显示Gps数据中的Action
        ///// </summary>
        //public bool ShowGpsDataAction
        //{
        //    get { return m_showGpsDataAction; }
        //    set { m_showGpsDataAction = value; }
        //}

        //private bool m_showGeoCode = false;
        ///// <summary>
        ///// 是否显示GeoCode的结果
        ///// </summary>
        //public bool ShowGeoCode
        //{
        //    get { return m_showGeoCode; }
        //    set { m_showGeoCode = value; }
        //}

        private void tsmUseSatellite_Click(object sender, EventArgs e)
        {
            this.MainMap.IsSatellite = tsmUseSatellite.Checked;
        }

        private void tsmReadGpsData_Click(object sender, EventArgs e)
        {
            using (var form = new GpsDataSelector())
            {
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    var ps = TrackPointDao.GetTrackPoints(form.CarName, form.StartTime, form.EndTime);
                    this.LoadTrackPoint(ps);
                }
            }
        }

        private void tsmShowGpsPoint_Click(object sender, EventArgs e)
        {
            //if (tsmShowGpsPoint.Checked)
            //{
            //    DrawTrackPoint();
            //}
            //else
            //{
            //    m_trackOverlay.Markers.Clear();
            //}
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshMap(m_currentTrack);
        }
    }
}
