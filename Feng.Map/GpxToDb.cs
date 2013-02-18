using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq; 
using GMap.NET;

namespace Feng.Map
{
    public class GpxToDb
    {
        /// <summary>
        /// Load the Xml document for parsing
        /// </summary>
        /// <param name="sFile">Fully qualified file name (local)</param>
        /// <returns>XDocument</returns>
        private XDocument GetGpxDoc(string gpxContent)
        {
            XDocument gpxDoc = XDocument.Load(new System.IO.StringReader(gpxContent));
            return gpxDoc;
        }

        /// <summary>
        /// Load the namespace for a standard GPX document
        /// </summary>
        /// <returns></returns>
        private XNamespace GetGpxNameSpace()
        {
            XNamespace gpx = XNamespace.Get("http://www.topografix.com/GPX/1/0");
            return gpx;
        }

        /// <summary>
        /// When passed a file, open it and parse all waypoints from it.
        /// </summary>
        /// <param name="sFile">Fully qualified file name (local)</param>
        /// <returns>string containing line delimited waypoints from
        /// the file (for test)</returns>
        /// <remarks>Normally, this would be used to populate the
        /// appropriate object model</remarks>
        public string LoadGPXWaypoints(string gpxContent)
        {
            XDocument gpxDoc = GetGpxDoc(gpxContent);
            XNamespace gpx = GetGpxNameSpace();

            var waypoints = from waypoint in gpxDoc.Descendants(gpx + "wpt")
                            select new
                            {
                                Latitude = waypoint.Attribute("lat").Value,
                                Longitude = waypoint.Attribute("lon").Value,
                                Elevation = waypoint.Element(gpx + "ele") != null ?
                                    waypoint.Element(gpx + "ele").Value : null,
                                Name = waypoint.Element(gpx + "name") != null ?
                                    waypoint.Element(gpx + "name").Value : null,
                                Dt = waypoint.Element(gpx + "cmt") != null ?
                                    waypoint.Element(gpx + "cmt").Value : null
                            };

            StringBuilder sb = new StringBuilder();
            foreach (var wpt in waypoints)
            {
                // This is where we'd instantiate data
                // containers for the information retrieved.
                sb.Append(
                  string.Format("Name:{0} Latitude:{1} Longitude:{2} Elevation:{3} Date:{4}\n",
                  wpt.Name, wpt.Latitude, wpt.Longitude,
                  wpt.Elevation, wpt.Dt));
            }

            return sb.ToString();
        }

        /// <summary>
        /// When passed a file, open it and parse all tracks
        /// and track segments from it.
        /// </summary>
        /// <param name="sFile">Fully qualified file name (local)</param>
        /// <returns>string containing line delimited waypoints from the
        /// file (for test)</returns>
        public string LoadGPXTracks(string gpxContent)
        {
            XDocument gpxDoc = GetGpxDoc(gpxContent);
            XNamespace gpx = GetGpxNameSpace();
            var tracks = from track in gpxDoc.Descendants(gpx + "trk")
                         select new
                         {
                             Name = track.Element(gpx + "name") != null ?
                              track.Element(gpx + "name").Value : null,
                             Segs = (
                                  from trackpoint in track.Descendants(gpx + "trkpt")
                                  select new
                                  {
                                      Latitude = trackpoint.Attribute("lat").Value,
                                      Longitude = trackpoint.Attribute("lon").Value,
                                      Elevation = trackpoint.Element(gpx + "ele") != null ?
                                        trackpoint.Element(gpx + "ele").Value : null,
                                      Time = trackpoint.Element(gpx + "time") != null ?
                                        trackpoint.Element(gpx + "time").Value : null
                                  }
                                )
                         };

            StringBuilder sb = new StringBuilder();
            foreach (var trk in tracks)
            {
                // Populate track data objects.
                foreach (var trkSeg in trk.Segs)
                {
                    // Populate detailed track segments
                    // in the object model here.
                    sb.Append(
                      string.Format("Track:{0} - Latitude:{1} Longitude:{2} " +
                                   "Elevation:{3} Date:{4}\n",
                      trk.Name, trkSeg.Latitude,
                      trkSeg.Longitude, trkSeg.Elevation,
                      trkSeg.Time));
                }
            }
            return sb.ToString();
        }

        public void ConvertGpx(string vehicleName, string gpxData)
        {
            XDocument gpxDoc = GetGpxDoc(gpxData);
            XNamespace gpx = GetGpxNameSpace();

            var tracks = from track in gpxDoc.Descendants(gpx + "trk")
                         select new
                         {
                             Name = track.Element(gpx + "name") != null ?
                              track.Element(gpx + "name").Value : null,
                             Segs = (
                                  from trackpoint in track.Descendants(gpx + "trkpt")
                                  select new
                                  {
                                      Latitude = trackpoint.Attribute("lat").Value,
                                      Longitude = trackpoint.Attribute("lon").Value,
                                      Elevation = trackpoint.Element(gpx + "ele") != null ?
                                        trackpoint.Element(gpx + "ele").Value : null,
                                      Time = trackpoint.Element(gpx + "time") != null ?
                                        trackpoint.Element(gpx + "time").Value : null
                                  }
                                )
                         };

            var waypoints = from waypoint in gpxDoc.Descendants(gpx + "wpt")
                            select new
                            {
                                Latitude = waypoint.Attribute("lat").Value,
                                Longitude = waypoint.Attribute("lon").Value,
                                Elevation = waypoint.Element(gpx + "ele") != null ?
                                    waypoint.Element(gpx + "ele").Value : null,
                                Name = waypoint.Element(gpx + "name") != null ?
                                    waypoint.Element(gpx + "name").Value : null,
                                Dt = waypoint.Element(gpx + "cmt") != null ?
                                    waypoint.Element(gpx + "cmt").Value : null
                            };

            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<TrackPoint>())
            {
                try
                {
                    rep.BeginTransaction();

                    Track track = new Track 
                        { 
                            Name = "FromGpx", 
                            VehicleName = vehicleName,
                            IsActive = true,
                            Created = System.DateTime.Now, 
                            CreatedBy = "Feng.Map" 
                        };
                    rep.Save(track);

                    foreach (var trk in tracks)
                    {
                        // Populate track data objects.
                        foreach (var trkSeg in trk.Segs)
                        {
                            TrackPoint tp = new TrackPoint
                            {
                                Track = track,
                                Latitude = Convert.ToDouble(trkSeg.Latitude),
                                Longitude = Convert.ToDouble(trkSeg.Longitude),
                                GpsTime = string.IsNullOrEmpty(trkSeg.Time) ? System.DateTime.Now : Convert.ToDateTime(trkSeg.Time),
                                MessageTime = System.DateTime.Now,
                                VehicleName = vehicleName,
                            };
                            rep.Save(tp);
                        }
                    }

                    foreach (var wpt in waypoints)
                    {
                        WayPoint wp = new WayPoint
                        {
                            Track = track,
                            Latitude = Convert.ToDouble(wpt.Latitude),
                            Longitude = Convert.ToDouble(wpt.Longitude),
                            GpsTime = System.DateTime.Now,
                            MessageTime = System.DateTime.Now,
                            IsActive = true,
                        };
                        rep.Save(wp);
                    }

                    rep.Save(track);

                    rep.CommitTransaction();
                }
                catch (Exception)
                {
                    rep.RollbackTransaction();
                }
            }
        }

        //public static void ConvertGpx(string gpxData)
        //{

        //    gpxType[] rs = GpxImporter.importGPXFile(gpxData);
        //    if (rs == null)
        //        return;

        //    using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<TrackPoint>())
        //    {
        //        rep.BeginTransaction();

        //        foreach (var r in rs)
        //        {
        //            Track track = new Track { Name = "FromGpx" };

        //            foreach (var trk in r.trk)
        //            {
        //                List<PointLatLng> points = new List<PointLatLng>();

        //                foreach (var seg in trk.trkseg)
        //                {
        //                    foreach (var p in seg.trkpt)
        //                    {
        //                        if (p.lat != 0.0m && p.lon != 0.0m)
        //                        {
        //                            points.Add(new PointLatLng((double)p.lat, (double)p.lon));

        //                            TrackPoint tp = new TrackPoint { Track = track, Latitude = (double)p.lat, Longitude = (double)p.lon, GpsTime = p.time };
        //                            rep.Save(tp);
        //                        }
        //                    }
        //                }

        //                foreach (var p in r.wpt)
        //                {
        //                    if (p.lat != 0.0m && p.lon != 0.0m)
        //                    {
        //                        points.Add(new PointLatLng((double)p.lat, (double)p.lon));

        //                        WayPoint wp = new WayPoint { Track = track, Latitude = (double)p.lat, Longitude = (double)p.lon, GpsTime = p.time };
        //                        rep.Save(wp);
        //                    }
        //                }
        //            }

        //            rep.Save(track);

        //            rep.CommitTransaction();
        //        }
        //    }
        //}
    }
}
