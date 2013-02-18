using System;
using System.Collections.Generic;
using System.Text;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.Converters.WellKnownText;

namespace Feng.Map
{
    [CLSCompliant(false)]
    public class GeobaseMapMarker : GMap.NET.WindowsForms.GMapMarker
    {
        private GMap.NET.WindowsForms.GMapControl m_gMapControl;

        public GeobaseMapMarker(GMap.NET.WindowsForms.GMapControl gMapControl, string mapFileName)
            : base(GMap.NET.PointLatLng.Zero)
        {
            m_gMapControl = gMapControl;

            if (!string.IsNullOrEmpty(mapFileName))
            {
                string fileName = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), mapFileName);
                if (System.IO.File.Exists(fileName))
                {
                    Telogis.GeoBase.Repositories.Repository.Default = new Telogis.GeoBase.Repositories.SimpleRepository(fileName);
                }
            }
            myMap = new Telogis.GeoBase.MapCtrl();

            DisableRegionCheck = true;
            IsHitTestVisible = false;
        }
        private Telogis.GeoBase.MapCtrl myMap;

        private GMap.NET.PureProjection m__mercatorProjection = new GMap.NET.Projections.MercatorProjection();
        public override void OnRender(System.Drawing.Graphics g)
        {
            try
            {
                var right = m__mercatorProjection.FromPixelToLatLng(new GMap.NET.GPoint(m_gMapControl.Size.Width, 0), (int)m_gMapControl.Zoom);
                var left = m__mercatorProjection.FromPixelToLatLng(new GMap.NET.GPoint(0, 0), (int)m_gMapControl.Zoom);

                //var right1 = GeometryTransform.TransformPoint(new SharpMap.Geometries.Point(right.Lng, right.Lat), m_googleMapCoordinateTransformation.MathTransform);
                //var left1 = GeometryTransform.TransformPoint(new SharpMap.Geometries.Point(left.Lng, left.Lat), m_googleMapCoordinateTransformation.MathTransform);

                //myMap.Size = new System.Drawing.Size(m_gMapControl.Width, m_gMapControl.Height);

                //myMap.Zoom = right1.X - left1.X;

                // 不是Center，具体怎样还没细看
                //var p = GoogleMapOffset.GetOffseted(m_gMapControl.CurrentPosition);
                var p = m_gMapControl.FromLocalToLatLng(m_gMapControl.Size.Width / 2, m_gMapControl.Size.Height / 2);

                //var p1 = GeometryTransform.TransformPoint(new SharpMap.Geometries.Point(p.Lng, p.Lat), m_googleMapCoordinateTransformation.MathTransform);

                myMap.Center = new Telogis.GeoBase.LatLon(p.Lat, p.Lng);
                myMap.Zoom = m_gMapControl.Zoom;

                var image = myMap.GetMap();
                if (image != null)
                {
                    g.DrawImageUnscaled(image, 0, 0);
                }

                //var image = myMap.GetMap();
                //var x = m_gMapControl.FromLatLngToLocal(new GMap.NET.PointLatLng(myMap.Envelope.Top, myMap.Envelope.Left));
                //var y = m_gMapControl.FromLatLngToLocal(new GMap.NET.PointLatLng(myMap.Envelope.Bottom, myMap.Envelope.Right));
                //g.DrawImage(image, x.X, x.Y, y.X - x.X, y.Y - x.Y);
            }
            catch (Exception)
            {
            }
        }
    }
}
