using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace Feng.GPS
{
    public partial class Map : UserControl
    {
        public Map()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                InitializeGMap();
            }
        }

        // @"Data Source=17haha8.kmip.net,8033;Initial Catalog=GMapBuffer;User ID=sa;Password=qazwsxedc;";
        public void AddSQLImageCache(string connectionString)
        {
            // add your custom map db provider
            GMap.NET.CacheProviders.MsSQLPureImageCache ch = new GMap.NET.CacheProviders.MsSQLPureImageCache();
            ch.ConnectionString = connectionString;
            GMaps.Instance.ImageCacheSecond = ch;
        }

        private void InitializeGMap()
        {
            MainMap.CacheLocation = System.IO.Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + "MapBuffer" + System.IO.Path.DirectorySeparatorChar;

            this.trackBar1.ValueChanged +=new EventHandler(trackBar1_ValueChanged);
            this.btnGo.Click += new EventHandler(btnGo_Click);
            this.comboBoxMapType.DropDownClosed += new System.EventHandler(this.comboBoxMapType_DropDownClosed);
            this.comboBoxMode.DropDownClosed += new System.EventHandler(this.comboBoxMode_DropDownClosed);

            // config gmaps
            GMaps.Instance.Language = LanguageType.ChineseSimplified;
            GMaps.Instance.UseRouteCache = true;
            GMaps.Instance.UseGeocoderCache = true;
            GMaps.Instance.UsePlacemarkCache = true;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            // set your proxy here if need
            //GMaps.Instance.Proxy = new System.Net.WebProxy("127.0.0.1", 9666);
            //GMaps.Instance.Proxy.Credentials = new NetworkCredential("ogrenci@bilgeadam.com", "bilgeadam");

            // config map             
            MainMap.MapType = MapType.GoogleMap;
            MainMap.MaxZoom = 19;
            MainMap.MinZoom = 1;
            MainMap.Zoom = 12;
            MainMap.CurrentPosition = new PointLatLng(39.9265884219094, 116.38916015625);

            // map events
            MainMap.OnCurrentPositionChanged += new CurrentPositionChanged(MainMap_OnCurrentPositionChanged);
            MainMap.OnTileLoadStart += new TileLoadStart(MainMap_OnTileLoadStart);
            MainMap.OnTileLoadComplete += new TileLoadComplete(MainMap_OnTileLoadComplete);
            MainMap.OnMarkerClick += new MarkerClick(MainMap_OnMarkerClick);
            MainMap.OnEmptyTileError += new EmptyTileError(MainMap_OnEmptyTileError);
            MainMap.OnMapZoomChanged += new MapZoomChanged(MainMap_OnMapZoomChanged);

            MainMap.MouseMove += new MouseEventHandler(MainMap_MouseMove);
            MainMap.MouseDown += new MouseEventHandler(MainMap_MouseDown);
            MainMap.MouseUp += new MouseEventHandler(MainMap_MouseUp);

            // get map type
            comboBoxMapType.DataSource = Enum.GetValues(typeof(MapType));
            comboBoxMapType.SelectedItem = MainMap.MapType;

            // acccess mode
            comboBoxMode.DataSource = Enum.GetValues(typeof(AccessMode));
            comboBoxMode.SelectedItem = GMaps.Instance.Mode;

            // get position
            textBoxLat.Text = MainMap.CurrentPosition.Lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
            textBoxLng.Text = MainMap.CurrentPosition.Lng.ToString(System.Globalization.CultureInfo.InvariantCulture);

            // get cache modes
            checkBoxUseRouteCache.Checked = GMaps.Instance.UseRouteCache;
            checkBoxUseGeoCache.Checked = GMaps.Instance.UseGeocoderCache;

            // get zoom  
            trackBar1.Minimum = MainMap.MinZoom;
            trackBar1.Maximum = MainMap.MaxZoom;

            // set current marker and get ground layer
            currentMarker = new GMapMarkerGoogleRed(MainMap.CurrentPosition);
            if (MainMap.Overlays.Count > 0)
            {
                ground = MainMap.Overlays[0] as GMapOverlay;
                ground.Markers.Add(currentMarker);
            }

            // add custom layers
            {
                objects = new GMapOverlay(MainMap, "objects");
                MainMap.Overlays.Add(objects);

                routes = new GMapOverlay(MainMap, "routes");
                MainMap.Overlays.Add(routes);
            }

            // add my city location for demo
            GeoCoderStatusCode status = GeoCoderStatusCode.Unknow;
            PointLatLng? pos = GMaps.Instance.GetLatLngFromGeocoder("ningbo, china", out status);
            if (pos != null)
            {
                currentMarker.Position = pos.Value;

                GMapMarker myCity = new GMapMarkerGoogleGreen(pos.Value);
                myCity.ToolTipMode = MarkerTooltipMode.Always;
                myCity.ToolTipText = "Welcome to ningbo!";
                ground.Markers.Add(myCity);

                MainMap.CurrentPosition = pos.Value;
            }
        }

        // marker
        GMapMarker currentMarker;

        // layers
        GMapOverlay ground;
        GMapOverlay objects;
        GMapOverlay routes;

        bool isMouseDown = false;
        void MainMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
            }
        }

        void MainMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                currentMarker.Position = MainMap.FromLocalToLatLng(e.X, e.Y);
                UpdateCurrentMarkerPositionText();
            }
        }

        // move current marker with left holding
        void MainMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isMouseDown)
            {
                currentMarker.Position = MainMap.FromLocalToLatLng(e.X, e.Y);
                UpdateCurrentMarkerPositionText();
            }
        }

        private void UpdateCurrentMarkerPositionText()
        {
            GMapControl.CurrentPosition = currentMarker.Position;
        }

        // MapZoomChanged
        void MainMap_OnMapZoomChanged()
        {
            trackBar1.Value = (int)MainMap.Zoom;
        }


        // empty tile displayed
        void MainMap_OnEmptyTileError(int zoom, GMap.NET.Point pos)
        {
            //MessageBox.Show("OnEmptyTileError, Zoom: " + zoom + ", " + pos.ToString(), "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        // click on some marker
        void MainMap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
           
        }

        // loader start loading tiles
        void MainMap_OnTileLoadStart()
        {
            groupBoxLoading.Invalidate(true);
        }

        // loader end loading tiles
        void MainMap_OnTileLoadComplete(long ElapsedMilliseconds)
        {
            groupBoxLoading.Invalidate(true);
        }


        // current point changed
        void MainMap_OnCurrentPositionChanged(PointLatLng point)
        {
            textBoxLat.Text = point.Lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
            textBoxLng.Text = point.Lng.ToString(System.Globalization.CultureInfo.InvariantCulture);

            currentMarker.Position = point;
        }

        // change map type
        private void comboBoxMapType_DropDownClosed(object sender, EventArgs e)
        {
            MainMap.MapType = (MapType)comboBoxMapType.SelectedValue;
            MainMap.ReloadMap();
        }

        // change mdoe
        private void comboBoxMode_DropDownClosed(object sender, EventArgs e)
        {
            GMaps.Instance.Mode = (AccessMode)comboBoxMode.SelectedValue;
            MainMap.ReloadMap();
        }

        // zoom
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            MainMap.Zoom = trackBar1.Value;
        }

        // goto by geocoder
        private void btnGo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxGeo.Text))
            {
                GeoCoderStatusCode status = MainMap.SetCurrentPositionByKeywords(textBoxGeo.Text);
                if (status != GeoCoderStatusCode.G_GEO_SUCCESS)
                {
                    MessageBox.Show("Google Maps Geocoder can't find: '" + textBoxGeo.Text + "', reason: " + status.ToString(), "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                double lat = double.Parse(textBoxLat.Text, System.Globalization.CultureInfo.InvariantCulture);
                double lng = double.Parse(textBoxLng.Text, System.Globalization.CultureInfo.InvariantCulture);

                MainMap.CurrentPosition = new PointLatLng(lat, lng);
            }
        }

        private void checkBoxUseRouteCache_CheckedChanged(object sender, EventArgs e)
        {
            GMaps.Instance.UseRouteCache = checkBoxUseRouteCache.Checked;
            GMaps.Instance.UseGeocoderCache = checkBoxUseGeoCache.Checked;
            GMaps.Instance.UsePlacemarkCache = GMaps.Instance.UseGeocoderCache;
        }

        private void checkBoxUseGeoCache_CheckedChanged(object sender, EventArgs e)
        {
            GMaps.Instance.UseRouteCache = checkBoxUseRouteCache.Checked;
            GMaps.Instance.UseGeocoderCache = checkBoxUseGeoCache.Checked;
            GMaps.Instance.UsePlacemarkCache = GMaps.Instance.UseGeocoderCache;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            MainMap.ShowExportDialog();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            MainMap.ShowImportDialog();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You sure?", "Clear GMap.NET cache?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    System.IO.Directory.Delete(MainMap.CacheLocation, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void btnPrefetch_Click(object sender, EventArgs e)
        {
            RectLatLng area = MainMap.SelectedArea;
            if (!area.IsEmpty)
            {
                for (int i = (int)MainMap.Zoom; i <= MainMap.MaxZoom; i++)
                {
                    DialogResult res = MessageBox.Show("Ready ripp at Zoom = " + i + " ?", "GMap.NET", MessageBoxButtons.YesNoCancel);

                    if (res == DialogResult.Yes)
                    {
                        TilePrefetcher obj = new TilePrefetcher();
                        obj.ShowCompleteMessage = true;
                        obj.Start(area, MainMap.Projection, i, MainMap.MapType, 100);
                    }
                    else if (res == DialogResult.No)
                    {
                        continue;
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Select map area holding ALT", "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public GMap.NET.WindowsForms.GMapControl GMapControl
        {
            get { return MainMap; }
        }

        public void AddRoute(List<PointLatLng> route)
        {
            // add route
            GMapRoute r = new GMapRoute(route, "test");
            routes.Routes.Add(r);

            //// add route start/end marks
            //GMapMarker m1 = new GMapMarkerGoogleRed(start);
            //m1.ToolTipText = "Start: " + start.ToString();
            //m1.TooltipMode = MarkerTooltipMode.Always;

            //GMapMarker m2 = new GMapMarkerGoogleGreen(end);
            //m2.ToolTipText = "End: " + end.ToString();
            //m2.TooltipMode = MarkerTooltipMode.Always;

            //objects.Markers.Add(m1);
            //objects.Markers.Add(m2);

            MainMap.ZoomAndCenterRoute(r);
        }


        private void btnMoveReset_Click(object sender, EventArgs e)
        {
            //MainMap.OverLayBottomOffsetX = 0;
            //MainMap.OverLayBottomOffsetY = 0;
            //ShowOffset();
            //MainMap.Invalidate();

        }

        private void ShowOffset()
        {
            //nudOffsetX.Value = MainMap.OverLayBottomOffsetX;
            //nudOffsetY.Value = MainMap.OverLayBottomOffsetY;
        }

        private void nudOffsetY_ValueChanged(object sender, EventArgs e)
        {
            //MainMap.OverLayBottomOffsetY = (int)nudOffsetY.Value;
            //MainMap.Invalidate();
        }

        private void nudOffsetX_ValueChanged(object sender, EventArgs e)
        {
            //MainMap.OverLayBottomOffsetX = (int)nudOffsetX.Value;
            //MainMap.Invalidate();
        }
        
    }
}
