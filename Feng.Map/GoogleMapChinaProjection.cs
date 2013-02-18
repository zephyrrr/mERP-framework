using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Map
{
    [CLSCompliant(false)]
    public class GoogleMapChinaProjection : GMap.NET.Projections.MercatorProjection
    {
        public static GoogleMapChinaProjection Instance2 = new GoogleMapChinaProjection();

        private Dictionary<double, Dictionary<double, Dictionary<int, GMap.NET.GPoint>>> m_buffer1 = new Dictionary<double, Dictionary<double, Dictionary<int, GMap.NET.GPoint>>>();
        private Dictionary<int, Dictionary<int, Dictionary<int, GMap.NET.PointLatLng>>> m_buffer2 = new Dictionary<int, Dictionary<int, Dictionary<int, GMap.NET.PointLatLng>>>();

        public override GMap.NET.GPoint FromLatLngToPixel(double lat, double lng, int zoom)
        {
            if (m_buffer1.ContainsKey(lat) && m_buffer1[lat].ContainsKey(lng) && m_buffer1[lat][lng].ContainsKey(zoom))
                return m_buffer1[lat][lng][zoom];

            GMap.NET.GPoint p = base.FromLatLngToPixel(lat, lng, zoom);
            GMap.NET.GPoint offset = GoogleMapChinaOffset.Instance.GetOffset(lat, lng, zoom);
            p = p + new GMap.NET.GSize(offset.X, offset.Y);

            if (!m_buffer1.ContainsKey(lat))
                m_buffer1[lat] = new Dictionary<double, Dictionary<int, GMap.NET.GPoint>>();
            if (!m_buffer1[lat].ContainsKey(lng))
                m_buffer1[lat][lng] = new Dictionary<int, GMap.NET.GPoint>();
            if (!m_buffer1[lat][lng].ContainsKey(zoom))
                m_buffer1[lat][lng][zoom] = p;

            return p;
        }

        public override GMap.NET.PointLatLng FromPixelToLatLng(int x, int y, int zoom)
        {
            if (m_buffer2.ContainsKey(x) && m_buffer2[x].ContainsKey(y) && m_buffer2[x][y].ContainsKey(zoom))
                return m_buffer2[x][y][zoom];

            GMap.NET.PointLatLng p = base.FromPixelToLatLng(x, y, zoom);
            GMap.NET.GPoint offset = GoogleMapChinaOffset.Instance.GetOffset(p.Lat, p.Lng, zoom);
            p = base.FromPixelToLatLng(x - offset.X, y - offset.Y, zoom);

            if (!m_buffer2.ContainsKey(x))
                m_buffer2[x] = new Dictionary<int, Dictionary<int, GMap.NET.PointLatLng>>();
            if (!m_buffer2[x].ContainsKey(y))
                m_buffer2[x][y] = new Dictionary<int, GMap.NET.PointLatLng>();
            if (!m_buffer2[x][y].ContainsKey(zoom))
                m_buffer2[x][y][zoom] = p;

            return p;
        }
    }
}
