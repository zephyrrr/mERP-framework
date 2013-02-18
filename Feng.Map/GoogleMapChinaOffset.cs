using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Feng.Map
{
    /// <summary>
    /// Google地图偏移量
    /// http://ixhan.com/note/index.php/Google_Map_中国地图偏移解决方案
    /// 最大最小：select max(lat), min(lat), max(lon), min(lon) from googlemapoffset： 53.5466666666646	18.1566666666667	134.789999999998	73.6666666666667
    /// </summary>
    [CLSCompliant(false)]
    public class GoogleMapChinaOffset : Singleton<GoogleMapChinaOffset>
    {
        public GoogleMapChinaOffset()
        {
        }
        private GMap.NET.PureProjection m_projection = new GMap.NET.Projections.MercatorProjection();
        public GMap.NET.PointLatLng GetOffseted(GMap.NET.PointLatLng latLon)
        {
            GMap.NET.GPoint offset = GetOffset(latLon.Lat, latLon.Lng, 18);
            if (offset == GMap.NET.GPoint.Empty)
                return latLon;

            GMap.NET.GPoint point = m_projection.FromLatLngToPixel(latLon, 18);
            point = point + new GMap.NET.GSize(offset.X, offset.Y);
            return m_projection.FromPixelToLatLng(point, 18);
        }
        public GMap.NET.PointLatLng GetAntiOffseted(GMap.NET.PointLatLng latLon)
        {
            GMap.NET.GPoint offset = GetOffset(latLon.Lat, latLon.Lng, 18);
            if (offset == GMap.NET.GPoint.Empty)
                return latLon;

            GMap.NET.GPoint point = m_projection.FromLatLngToPixel(latLon, 18);
            point = point - new GMap.NET.GSize(offset.X, offset.Y);
            return m_projection.FromPixelToLatLng(point, 18);
        }

        private PureGoogleMapChinaOffsetCache m_cache = new SQLiteGoogleMapChinaOffsetCache();
        public PureGoogleMapChinaOffsetCache Cache
        {
            get { return m_cache; }
        }

        private PureGoogleMapChinaOffsetCache m_cacheSecond;
        public PureGoogleMapChinaOffsetCache CacheSecond
        {
            get { return m_cacheSecond; }
            set { m_cacheSecond = value; }
        }

        public GMap.NET.GPoint GetOffset(int key)
        {
            GMap.NET.GPoint? offset = null;

            if (m_localDictionary.ContainsKey(key))
                offset = m_localDictionary[key];

            if (offset == null && this.Cache != null)
            {
                var ch = this.Cache.GetOffsetFromCache(key);
                if (ch.HasValue)
                {
                    m_localDictionary[key] = ch.Value;
                    offset = ch.Value;
                }
            }
            if (offset == null && this.CacheSecond != null)
            {
                var ch = this.CacheSecond.GetOffsetFromCache(key);
                if (ch.HasValue)
                {
                    m_localDictionary[key] = ch.Value;
                    offset = ch.Value;
                }
            }

            if (offset == null)
            {
                Data.DbHelper db = Feng.Data.DbHelper.CreateDatabase("GoogleMapOffset");
                if (db != null)
                {
                    var dt1 = db.ExecuteDataTable(string.Format("SELECT dlat, dlon FROM KeyOffset WHERE Id = {0}", key));

                    if (dt1.Rows.Count > 0)
                    {
                        offset = new GMap.NET.GPoint(Convert.ToInt32(dt1.Rows[0][1]), Convert.ToInt32(dt1.Rows[0][0]));
                        if (this.Cache != null)
                        {
                            this.Cache.PutOffsetToCache(key, offset.Value);
                        }
                        if (this.CacheSecond != null)
                        {
                            this.CacheSecond.PutOffsetToCache(key, offset.Value);
                        }

                        m_localDictionary[key] = offset.Value;
                    }
                }
            }

            if (offset == null)
            {
                return GMap.NET.GPoint.Empty;
            }
            else
            {
                return offset.Value;
            }
        }

        private Dictionary<Int32, GMap.NET.GPoint> m_localDictionary = new Dictionary<Int32, GMap.NET.GPoint>();
        public GMap.NET.GPoint GetOffset(double lat, double lon, int zoom)
        {
            if (zoom < 11)
                return GMap.NET.GPoint.Empty;

            if (lat > 53.55 || lat < 18.15 || lon > 134.79 || lon < 73.65)
            {
                return GMap.NET.GPoint.Empty;
                //throw new ArgumentException("lat or lon is beyond the limit!");
            }
            if (zoom == 19)
            {
                GMap.NET.GPoint ret1 = GetOffset(lat, lon, 18);
                return new GMap.NET.GPoint(2 * ret1.X, 2 * ret1.Y);
            }
            if (zoom > 18)
            {
                throw new ArgumentException("zoom is beyond the limit!");
            }

            Int16 iLat = (Int16)((Int16)Math.Round(lat * 100) + 9000);   //纬度, 最大18000
            Int16 iLon = (Int16)Math.Round(lon * 100);
            Int32 key = ((Int32)iLat << 16) + iLon;

            GMap.NET.GPoint offset = GetOffset(key);

            if (offset == null || offset == GMap.NET.GPoint.Empty)
            {
                return GMap.NET.GPoint.Empty;
            }
            else
            {
                int dz = (int)Math.Pow(2, 18 - zoom);
                return new GMap.NET.GPoint(offset.X / dz, offset.Y / dz);
            }
        }
    }
}
