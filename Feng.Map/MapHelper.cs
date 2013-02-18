using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Map
{
    public static class MapHelper
    {
        public const string ShpFile4Road = "理论路线.shp";
        public const string ShpFile4Site = "地点.shp";
        public const string AddrColumnName = "Name";

        ////[System.Security.SecuritySafeCritical]
        //[System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        //[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        //public static extern bool QueryPerformanceCounter([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.I8)] ref long count);

        //[System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        //public static extern IntPtr Kernel32LoadLibrary(string lpFileName);


        public const string RepositoryName4Common = @"MapCommon.gbfs";
        public const string RepositoryName4Important = @"MapImportant.gbfs";

        private static Telogis.GeoBase.Repositories.Repository[] m_repositories = new Telogis.GeoBase.Repositories.Repository[2];
        static MapHelper()
        {
            string fileName = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), RepositoryName4Common);
            if (!System.IO.File.Exists(fileName))
            {
                fileName = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, RepositoryName4Common);
            }
            m_repositories[0] = new Telogis.GeoBase.Repositories.SimpleRepository(fileName);
            fileName = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), RepositoryName4Important);
            if (!System.IO.File.Exists(fileName))
            {
                fileName = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, RepositoryName4Important);
            }
            m_repositories[1] = new Telogis.GeoBase.Repositories.SimpleRepository(fileName);

            Telogis.GeoBase.Repositories.Repository.Default = m_repositories[0];
        }

        public static void ChangeRepository(MapType type)
        {
            switch(type)
            {
                case MapType.普通区域:
                    Telogis.GeoBase.Repositories.Repository.Default = m_repositories[0];
                    break;
                case MapType.重要地点:
                    Telogis.GeoBase.Repositories.Repository.Default = m_repositories[1];
                    break;
                default:
                    throw new ArgumentException("invalid type");
            }
        }
        //private static string Decode(string s)
        //{
        //    return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.GetEncoding("gb2312").GetBytes(s));
        //}

        public enum MapType
        {
            普通区域,
            重要地点
        }
        public static string GetAreaFromGpsData(TrackPoint p, string preArea = null)
        {
            Telogis.GeoBase.LatLon loc = new Telogis.GeoBase.LatLon(p.Latitude, p.Longitude);
            string tableName = "all";
            //if (type == AreaType.区域)
            //    tableName = "states";
            //else if (type == AreaType.重要地点)
            //    tableName = "islands";

            var ret = Telogis.GeoBase.DataQuery.QueryPolygonsAtPoint(loc, tableName);
            if (ret.Length == 0)
                return null;
            else if (ret.Length == 1 || string.IsNullOrEmpty(preArea))
                return ret[0].Name;
            else
            {
                foreach (var i in ret)
                {
                    if (i.Name == preArea)
                    {
                        return i.Name;
                    }
                }
                return ret[0].Name;
            }
        }

        public static int RoadWidth = 100;
        public static string GetRoadFromGpsData(TrackPoint gpsData, string preRoad = null)
        {
            Telogis.GeoBase.LatLon loc = new Telogis.GeoBase.LatLon(gpsData.Latitude, gpsData.Longitude);

            var arg = new Telogis.GeoBase.ReverseGeoCodeArgs(loc);
            arg.Heading = gpsData.Heading;
            if (!string.IsNullOrEmpty(preRoad))
            {
                arg.LastNames = new string[] { preRoad };
            }
            arg.Mode = Telogis.GeoBase.ReverseGeoCodeMode.AllLinks;
            arg.Speed = gpsData.Speed;

            var streetFull = Telogis.GeoBase.GeoCoder.ReverseGeoCodeFull(arg);
            if (streetFull != null)
            {
                double distance = streetFull.Intersection.DistanceTo(loc, Telogis.GeoBase.DistanceUnit.METERS);
                if (distance > RoadWidth)
                    return null;

                if (preRoad != null)
                {
                    if (streetFull.Address != null)
                    {
                        foreach (var i in streetFull.Address.Names)
                        {
                            if (i == preRoad)
                            {
                                return i;
                            }
                        }
                    }

                    if (streetFull.CrossStreet != null)
                    {
                        foreach (var i in streetFull.CrossStreet.Name)
                        {
                            if (i == preRoad)
                            {
                                return i;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(streetFull.Address.PrimaryName))
                    return preRoad;

                return streetFull.Address.PrimaryName;
            }
            return null;
        }
    }
}
