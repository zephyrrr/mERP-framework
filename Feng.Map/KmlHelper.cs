using System;
using System.Reflection;
using System.Xml;
using System.IO;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;

namespace Feng.Map
{
    public static class KmlHelper
    {
        private static void AddNamespace(Element element, string prefix, string uri)
        {
            // The Namespaces property is marked as internal.
            PropertyInfo property = typeof(Element).GetProperty(
                "Namespaces",
                BindingFlags.Instance | BindingFlags.NonPublic);

            var namespaces = (XmlNamespaceManager)property.GetValue(element, null);
            namespaces.AddNamespace(prefix, uri);
        }

        public static void GenerateTourKml(Track track, string kmlFilePath)
        {
            //var animationUpdate = new SharpKml.Dom.GX.AnimatedUpdate();
            //animationUpdate.Duration = 6.5;
            //animationUpdate.Update = new Update();
            //animationUpdate.Update.AddUpdate(new ChangeCollection
            //{
            //    new IconStyle { TargetId = "iconstyle", Scale = 10.0 }
            //});
            var tour = new SharpKml.Dom.GX.Tour();
            tour.Name = "Play";
            tour.Playlist = new SharpKml.Dom.GX.Playlist();
            //tour.Playlist.AddTourPrimitive(animationUpdate);

            var trackPoint = TrackPointDao.GetTrackPoints(track);
            if (trackPoint.Count == 0)
                return;

            for (int i = 0; i < trackPoint.Count; ++i)
            {
                var p = trackPoint[i];
                if (p.Heading == 0)
                    continue;

                var flyTo = new SharpKml.Dom.GX.FlyTo();
                if (i == trackPoint.Count - 1)
                    flyTo.Duration = 10;
                else
                {
                    var ts = trackPoint[i + 1].GpsTime - p.GpsTime;
                    flyTo.Duration = Math.Max(1, ts.TotalSeconds / 5);
                }
                flyTo.View = new Camera
                {
                    Longitude = p.Longitude,
                    Latitude = p.Latitude,
                    Altitude = p.Altitude,
                    Heading = p.Heading,
                    Tilt = 80,
                    Roll = 0
                };
                flyTo.Mode = SharpKml.Dom.GX.FlyToMode.Smooth;
                tour.Playlist.AddTourPrimitive(flyTo);

                //var wait = new SharpKml.Dom.GX.Wait();
                //wait.Duration = 1;
                //tour.Playlist.AddTourPrimitive(wait);
            }

            var document = new Document();
            document.Name = "PlayTrack " + track.ID.ToString();
            document.Open = true;

            document.AddStyle(
                new Style
                {
                    Id = "style0",
                    Icon = new IconStyle
                    {
                        Id = "iconstyle",
                        Scale = 1.0
                    }
                });

            document.AddFeature(
                new Placemark
                {
                    Id = "Track Start " + track.ID.ToString(),
                    Name = "Start",
                    StyleUrl = new Uri("#style0", UriKind.Relative),
                    Geometry =
                        new Point
                        {
                            Coordinate = new Vector(trackPoint[0].Latitude, trackPoint[0].Longitude, trackPoint[0].Altitude)
                        }
                });

            document.AddFeature(tour);

            SharpKml.Dom.GX.Track t = new SharpKml.Dom.GX.Track();
            t.Id = "Track Track " + track.ID.ToString();
            for (int i = 0; i < trackPoint.Count; ++i)
            {
                var p = trackPoint[i];

                t.AddWhen(p.GpsTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                t.AddCoordinate(new Vector(p.Latitude, p.Longitude, p.Altitude));
            }
            Placemark placemark = new Placemark();
            placemark.Name = "All";
            placemark.Geometry = t;
            document.AddFeature(placemark);

            // Quick way to save the output
            var kml = new Kml { Feature = document };
            AddNamespace(kml, "gx", "http://www.google.com/kml/ext/2.2");

            KmlFile.Create(kml, false).Save(kmlFilePath);
        }
    }
}
