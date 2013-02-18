using System;
using System.Collections.Generic;
using System.Text;
using GMap.NET.WindowsForms;

namespace Feng.Map
{
    [CLSCompliant(false)]
    public class MapControl : GMap.NET.WindowsForms.GMapControl
    {
        public MapControl()
        {
            this.Dock = System.Windows.Forms.DockStyle.Fill;

            this.DragButton = System.Windows.Forms.MouseButtons.Left;

            try
            {
                this.CacheLocation = ".\\GMapCache\\";

                if (GoogleMapChinaOffset.Instance.Cache is SQLiteGoogleMapChinaOffsetCache)
                {
                    (GoogleMapChinaOffset.Instance.Cache as SQLiteGoogleMapChinaOffsetCache).CacheLocation = ".\\GMapCache\\";
                }
            }
            catch (Exception)
            {
            }

            if (System.Configuration.ConfigurationManager.ConnectionStrings["GMapBuffer"] != null)
            {
                string s = System.Configuration.ConfigurationManager.ConnectionStrings["GMapBuffer"].ConnectionString;
                // 内网
                if (s.Contains("192.168."))
                {
                    this.SetSecondCacheToSQL(System.Configuration.ConfigurationManager.ConnectionStrings["GMapBuffer"].ConnectionString);
                }
            }

            // config gmaps
            //GMap.NET.GMaps.Instance.Language = GMap.NET.LanguageType.ChineseSimplified;
            GMap.NET.GMaps.Instance.UseRouteCache = true;
            GMap.NET.GMaps.Instance.UseGeocoderCache = true;
            GMap.NET.GMaps.Instance.UsePlacemarkCache = true;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;

            // Local cache works faster if you re-index it sometimes
            //GMaps.Instance.OptimizeMapDb(null);

            this.MapProvider = new GMapProviderWrapper(GMap.NET.MapProviders.GoogleChinaMapProvider.Instance);

            //typeof(GMap.NET.WindowsForms.GMapControl).InvokeMember("Projection", 
            //    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.NonPublic, null, this, 
            //    new object[] { new Feng.Map.GoogleMapChinaProjection() });

            //var proj = new Feng.Map.GoogleMapChinaProjection();
            //this.MapProvider.Projection = new Feng.Map.GoogleMapChinaProjection();
            //Feng.Utils.ReflectionHelper.SetObjectValue(Feng.Utils.ReflectionHelper.GetObjectValue(this, "Core"), "Projection", proj);

            this.MaxZoom = 18;
            this.MinZoom = 1;
            this.Zoom = 11;

            this.Position = new GMap.NET.PointLatLng(29.868336, 121.54399);   // Ningbo
        }

        private bool m_isSatellite = false;
        public bool IsSatellite
        {
            get { return m_isSatellite; }
            set
            {
                if (m_isSatellite != value)
                {
                    m_isSatellite = value;
                    if (m_isSatellite)
                    {
                        this.MapProvider = new GMapProviderWrapper(GMap.NET.MapProviders.GoogleChinaHybridMapProvider.Instance);
                        //GMapProviderWrapper.Instance.Provider = GMap.NET.MapProviders.GoogleChinaHybridMapProvider.Instance;
                    }
                    else
                    {
                        this.MapProvider = new GMapProviderWrapper(GMap.NET.MapProviders.GoogleChinaMapProvider.Instance);
                        //GMapProviderWrapper.Instance.Provider = GMap.NET.MapProviders.GoogleChinaMapProvider.Instance;
                    }
                }

            }
        }
        

        // @"Data Source=;Initial Catalog=GMapBuffer;User ID=sa;Password=;";
        public void SetSecondCacheToSQL(string connectionString)
        {
            // add your custom map db provider
            GMap.NET.CacheProviders.MsSQLPureImageCache ch = new GMap.NET.CacheProviders.MsSQLPureImageCache();
            ch.ConnectionString = connectionString;
            GMap.NET.GMaps.Instance.ImageCacheSecond = ch;

            //MsSqlGoogleMapChinaOffsetCache2 ch2 = new MsSqlGoogleMapChinaOffsetCache2();
            //ch2.ConnectionString = connectionString;
            //GoogleMapChinaOffset.Instance.CacheSecond = ch2;
        }

        private GMap.NET.PureImageCache m_localCache;
        public bool EnableLocalCache
        {
            get
            {
                return GMap.NET.GMaps.Instance.ImageCacheLocal != null;
            }
            set
            {
                if (value)
                {
                    if (GMap.NET.GMaps.Instance.ImageCacheLocal == null)
                    {
                        GMap.NET.GMaps.Instance.ImageCacheLocal = m_localCache;
                    }
                }
                else
                {
                    m_localCache = GMap.NET.GMaps.Instance.ImageCacheLocal;
                    GMap.NET.GMaps.Instance.ImageCacheLocal = null;
                }
            }
        }


        public Feng.Map.SharpMapMarker AddSharpMarker(string fileName)
        {
            try
            {
                Feng.Map.SharpMapMarker sharpMarker = new Feng.Map.SharpMapMarker(this);
                GMapOverlay overlay = new GMapOverlay(this, "sharpMap" + Guid.NewGuid());
                overlay.Markers.Add(sharpMarker);
                this.Overlays.Insert(0, overlay);
                sharpMarker.Load(fileName);
                return sharpMarker;
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }
        public Feng.Map.GeobaseMapMarker AddGeobaseMarker(string fileName)
        {
            try
            {
                Feng.Map.GeobaseMapMarker marker = new Feng.Map.GeobaseMapMarker(this, fileName);
                GMapOverlay overlay = new GMapOverlay(this, "geobase" + Guid.NewGuid());
                overlay.Markers.Add(marker);
                this.Overlays.Insert(0, overlay);
                return marker;
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }
        public Feng.Map.KmlOverlay AddKmlOverlay(string fileName)
        {
            Feng.Map.KmlOverlay kmlOverlay = new Feng.Map.KmlOverlay(this, "kmlOverlay" + Guid.NewGuid());
            this.Overlays.Add(kmlOverlay);
            kmlOverlay.Load(fileName);
            return kmlOverlay;
        }
    }
}
