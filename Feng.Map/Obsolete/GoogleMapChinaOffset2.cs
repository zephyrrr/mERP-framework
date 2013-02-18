using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Feng.Map.Obsolete
{
    /// <summary>
    /// Google地图偏移量
    /// http://ixhan.com/note/index.php/Google_Map_中国地图偏移解决方案
    /// 最大最小：select max(lat), min(lat), max(lon), min(lon) from googlemapoffset： 53.5466666666646	18.1566666666667	134.789999999998	73.6666666666667
    /// </summary>
    public class GoogleMapChinaOffset2 : Singleton<GoogleMapChinaOffset2>
    {
        private GMap.NET.PureProjection m_projection = new GMap.NET.Projections.MercatorProjection();
        public GMap.NET.PointLatLng GetOffseted(GMap.NET.PointLatLng latLon)
        {
            System.Drawing.Size offset = GetOffset(latLon.Lat, latLon.Lng, 18).Value;
            GMap.NET.GPoint point = m_projection.FromLatLngToPixel(latLon, 18);
            point = point + new GMap.NET.GSize(offset.Width, offset.Height);
            return m_projection.FromPixelToLatLng(point, 18);
        }
        public GMap.NET.PointLatLng GetAntiOffseted(GMap.NET.PointLatLng latLon)
        {
            System.Drawing.Size offset = GetOffset(latLon.Lat, latLon.Lng, 18).Value;
            GMap.NET.GPoint point = m_projection.FromLatLngToPixel(latLon, 18);
            point = point - new GMap.NET.GSize(offset.Width, offset.Height);
            return m_projection.FromPixelToLatLng(point, 18);
        }

        private PureGoogleMapChinaOffsetCache2 m_cache = new SQLiteGoogleMapChinaOffsetCache2();
        public PureGoogleMapChinaOffsetCache2 Cache
        {
            get { return m_cache; }
        }

        private PureGoogleMapChinaOffsetCache2 m_cacheSecond;
        public PureGoogleMapChinaOffsetCache2 CacheSecond
        {
            get { return m_cacheSecond; }
            set { m_cacheSecond = value; }
        }

        //private int m_getCnt = 0;
        private Dictionary<Tuple<int, int, int>, System.Drawing.Size> m_localDictionary = new Dictionary<Tuple<int, int, int>, System.Drawing.Size>();
        public System.Drawing.Size? GetOffset(double lat, double lon, int zoom)
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("Get Offset of {0}, {1}, {2}", lat, lon, zoom));
            //m_getCnt++;

            if (zoom < 11)
                return System.Drawing.Size.Empty;

            if (lat > 53.55 || lat < 18.15 || lon > 134.79 || lon < 73.65)
            {
                return System.Drawing.Size.Empty;
                //throw new ArgumentException("lat or lon is beyond the limit!");
            }
            if (zoom == 19)
            {
                System.Drawing.Size? ret1 = GetOffset(lat, lon, 18);
                if (ret1.HasValue)
                {
                    return new System.Drawing.Size(2 * ret1.Value.Width, 2 * ret1.Value.Height);
                }
                else
                {
                    return null;
                }
            }
            if (zoom > 18)
            {
                throw new ArgumentException("zoom is beyond the limit!");
            }

            Tuple<int, int, int> key = new Tuple<int,int,int>((int)Math.Round(lat * 100), (int)Math.Round(lon * 100), zoom);
            if (m_localDictionary.ContainsKey(key))
                return m_localDictionary[key];

            if (this.Cache != null)
            {
                var ch = this.Cache.GetOffsetFromCache(new GMap.NET.GPoint(key.Item1, key.Item2), zoom);
                if (ch.HasValue)
                {
                    m_localDictionary[key] = ch.Value;
                    return ch.Value;
                }
            }
            if (this.CacheSecond != null)
            {
                var ch = this.CacheSecond.GetOffsetFromCache(new GMap.NET.GPoint(key.Item1, key.Item2), zoom);
                if (ch.HasValue)
                {
                    m_localDictionary[key] = ch.Value;
                    return ch.Value;
                }
            }

            if (m_offsetLat == null)
            {
                m_offsetLat = new List<int>();
                m_offsetIndex = new List<int>();
                LoadOffsetIndex();
            }

            //int r = m_offsetLat.BinarySearch((int)Math.Round(lat * 100));
            // 必须找到一个，如果找不到，找相近的
            var r = BinarySearchLat(lat);
           
            if (!m_lonBuffer.ContainsKey(r))
            {
                System.Diagnostics.Debug.Assert(r + 1 < m_offsetIndex.Count, "R must not the last!");

                try
                {
	                var dt1 = Feng.Data.DbHelper.CreateDatabase("GoogleMapOffset").ExecuteDataTable(
	                    string.Format("SELECT Id, Lon, Offset FROM GoogleMapOffset WHERE Id >= {0} AND ID < {1}", 
	                    m_offsetIndex[r] + 1, m_offsetIndex[r + 1] + 1));
	                m_lonBuffer[r] = dt1;
                }
                catch(Exception)
                {
                	//ServiceProvider.GetService<IExceptionProcess>().ProcessWithResume(ex);
                	return System.Drawing.Size.Empty;
                }
            }

            System.Data.DataTable dt = m_lonBuffer[r];

            var ret = BinarySearchLon(dt, lon, zoom);

            if (ret.HasValue)
            {
                if (this.Cache != null)
                {
                    this.Cache.PutOffsetToCache(new GMap.NET.GPoint(key.Item1, key.Item2), zoom, ret.Value);
                }
                if (this.CacheSecond != null)
                {
                    this.CacheSecond.PutOffsetToCache(new GMap.NET.GPoint(key.Item1, key.Item2), zoom, ret.Value);
                }

                m_localDictionary[key] = ret.Value;
            }

            return ret;
        }

        private int BinarySearchLat(double lat)
        {
            int wantLat = (int)Math.Round(lat * 100);

            int lowNum = 0;
            int highNum = m_offsetLat.Count - 1;
            while (lowNum <= highNum)
            {
                int midNum = (lowNum + highNum) / 2;
                int midLon = m_offsetLat[midNum];
                if (wantLat == midLon)
                {
                    return midNum;
                }
                else if (wantLat < midLon)
                {
                    //search value is lower than this index of our array
                    //so set the high number equal to the middle number
                    //minus 1
                    highNum = midNum - 1;
                }
                else if (wantLat > midLon)
                {
                    //search value is higher than this index of our array
                    //so set the low number to the middle number + 1
                    lowNum = midNum + 1;
                }
            }
            return lowNum;
        }

        private System.Drawing.Size? BinarySearchLon(System.Data.DataTable dt, double lon, int zoom)
        {
            int lowNum = 0;
            int highNum = dt.Rows.Count - 1;
            while (lowNum <= highNum)
            {
                int midNum = (lowNum + highNum) / 2;
                double midLon = (double)dt.Rows[midNum]["Lon"];
                if (Math.Abs(lon - midLon) < 0.009)
                {
                    // [5, 5, 11, 10, 23, 20, 46, 40, 93, 81, 187, 162, 375, 325, 750, 651]
                    // 分为别从11级到18级的地图和卫星图的偏移像素数量
                    string offset = (string)dt.Rows[midNum]["Offset"];
                    string[] ss = offset.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    return new System.Drawing.Size(Convert.ToInt32(ss[2 * (zoom - 11)]), Convert.ToInt32(ss[2 * (zoom - 11) + 1]));
                }
                else if (lon < midLon)
                {
                    //search value is lower than this index of our array
                    //so set the high number equal to the middle number
                    //minus 1
                    highNum = midNum - 1;
                }
                else if (lon > midLon)
                {
                    //search value is higher than this index of our array
                    //so set the low number to the middle number + 1
                    lowNum = midNum + 1;
                }
            }

            // 返回最接近的值
            {
                string offset = (string)dt.Rows[highNum]["Offset"];
                string[] ss = offset.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);
                return new System.Drawing.Size(Convert.ToInt32(ss[2 * (zoom - 11)]), Convert.ToInt32(ss[2 * (zoom - 11) + 1]));
            }
        }
        private List<int> m_offsetLat = null;
        private List<int> m_offsetIndex = null;
        private Dictionary<int, System.Data.DataTable> m_lonBuffer = new Dictionary<int, System.Data.DataTable>();

        private void LoadOffsetIndex()
        {
            using (StreamReader sr = new StreamReader(System.Reflection.Assembly.GetCallingAssembly().GetManifestResourceStream("Feng.Map.GoogleMapChinaOffsetIndex.dat")))
            {
                while (true)
                {
                    string s = sr.ReadLine();
                    if (string.IsNullOrEmpty(s))
                        break;
                    string[] ss = s.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    m_offsetLat.Add((int)Math.Round(Convert.ToDouble(ss[0]) * 100));
                    m_offsetIndex.Add(Convert.ToInt32(ss[1]));
                }
            }
        }
    }
}
