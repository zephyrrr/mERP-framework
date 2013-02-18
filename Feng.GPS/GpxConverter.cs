using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    public static class GpxConverter
    {
        public static string ConvertToGpx(Track track)
        {
            gpx.gpxType gpx = new gpx.gpxType();
            gpx.creator = track.VehicleName;
            gpx.metadata = new global::gpx.metadataType();
            gpx.metadata.time = track.StartTime.HasValue ? DateTime.MinValue : track.StartTime.Value;
            gpx.metadata.timeSpecified = track.StartTime.HasValue;
            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<TrackPoint>())
            {
                rep.Attach(track);
                gpx.trk = new gpx.trkType[1];
                gpx.trk[0] = new global::gpx.trkType();
                gpx.trk[0].number = "1";
                gpx.trk[0].trkseg = new gpx.trksegType[1];
                gpx.trk[0].trkseg[0] = new global::gpx.trksegType();
                gpx.trk[0].trkseg[0].trkpt = new gpx.wptType[track.TrackPoints.Count];
                for(int i=0; i<track.TrackPoints.Count; ++i)
                {
                    gpx.trk[0].trkseg[0].trkpt[i] = new global::gpx.wptType();
                    gpx.trk[0].trkseg[0].trkpt[i].ele = (decimal)track.TrackPoints[i].Altitude;
                    gpx.trk[0].trkseg[0].trkpt[i].eleSpecified = true;
                    gpx.trk[0].trkseg[0].trkpt[i].lat = (decimal)track.TrackPoints[i].Latitude;
                    gpx.trk[0].trkseg[0].trkpt[i].lon = (decimal)track.TrackPoints[i].Longitude;
                    gpx.trk[0].trkseg[0].trkpt[i].time = track.TrackPoints[i].GpsTime;
                    gpx.trk[0].trkseg[0].trkpt[i].timeSpecified = true;
                    gpx.trk[0].trkseg[0].trkpt[i].magvar = (decimal)track.TrackPoints[i].Heading;
                    gpx.trk[0].trkseg[0].trkpt[i].magvarSpecified = true;
                }
            }

            var xmlSerialize = new System.Xml.Serialization.XmlSerializer(typeof(gpx.gpxType));
            var stream = new System.IO.StringWriter();
            xmlSerialize.Serialize(stream, gpx);
            stream.Close();
            return stream.ToString();
        }

        public static Track ConvertToTrack(string gpxString)
        {
            gpx.gpxType gpx = null;
            try
            {
                var xmlSerialize = new System.Xml.Serialization.XmlSerializer(typeof(gpx.gpxType));
                var stream = new System.IO.StringReader(gpxString);
                gpx = (gpx.gpxType)xmlSerialize.Deserialize(stream);
            }
            catch(Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
                return null;
            }

            Track track = new Track();
            track.TrackPoints = new List<TrackPoint>();
            track.StartTime = gpx.metadata.timeSpecified ? (DateTime?)gpx.metadata.time : null;

            track.TrackPoints = new List<TrackPoint>();
            foreach (var i in gpx.trk)
            {
                foreach (var j in i.trkseg)
                {
                    foreach (var k in j.trkpt)
                    {
                        var p = new TrackPoint();
                        p.Altitude = k.eleSpecified ? (double)k.ele : 0;
                        p.Latitude = (double)k.lat;
                        p.Longitude = (double)k.lon;
                        p.GpsTime = k.timeSpecified ? k.time : DateTime.MinValue;
                        p.MessageTime = p.GpsTime;
                        p.VehicleName = gpx.creator;
                        p.Track = track;

                        track.TrackPoints.Add(p);
                    }
                }
            }
            return track;
        }
    }
}
