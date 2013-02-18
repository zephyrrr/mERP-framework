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
    public class SharpMapMarker : GMap.NET.WindowsForms.GMapMarker
    {
        private SharpMap.Map myMap;
        private GMap.NET.WindowsForms.GMapControl m_gMapControl;
        private SharpMap.Layers.VectorLayer m_shapeLayer;
        private ShapeFile m_shapeFileData;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gMapControl"></param>
        public SharpMapMarker(GMap.NET.WindowsForms.GMapControl gMapControl)
            : base(GMap.NET.PointLatLng.Zero)
        {
            m_gMapControl = gMapControl;

            myMap = new SharpMap.Map(new System.Drawing.Size(m_gMapControl.Width, m_gMapControl.Height));

            m_shapeLayer = new SharpMap.Layers.VectorLayer("SharpMap layer" + Guid.NewGuid());
            //m_shapeLayer.Style.Outline = new System.Drawing.Pen(System.Drawing.Color.Green, 1f);
            m_shapeLayer.Style.EnableOutline = false;
            m_shapeLayer.Style.Line = new System.Drawing.Pen(System.Drawing.Color.Blue, 1f);

            //myMap.Center = new SharpMap.Geometries.Point(121.54399, 29.868336);

            DisableRegionCheck = true;
            IsHitTestVisible = false;
        }

        public VectorLayer Layer
        {
            get { return m_shapeLayer; }
        }
        public void Load(string fileName)
        {
            Load(fileName, System.Text.Encoding.GetEncoding("gb2312"));
        }

        public void Load(string fileName, System.Text.Encoding encoding)
        {
            try
            {
                m_shapeFileData = new SharpMap.Data.Providers.ShapeFile(fileName);
                m_shapeFileData.Encoding = encoding;

                m_shapeLayer.CoordinateTransformation = Transform2Mercator(m_shapeFileData.CoordinateSystem);
                m_googleMapCoordinateTransformation = Transform2Mercator(m_shapeFileData.CoordinateSystem);

                //m_shapeLayer.DataSource = m_shapeFileData;
                //if (myMap.Layers.IndexOf(m_shapeLayer) == -1)
                //{
                    //myMap.Layers.Add(m_shapeFileLayer);
                //}
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        //public void ShowAddress(IList<string> addresses)
        //{
        //    ShowFeature(new Predicate<System.Data.DataRow>(p => addresses.Contains(p[MapHelper.AddrColumnName].ToString())));
        //}

        public void Clear()
        {
            if (myMap.Layers.IndexOf(m_shapeLayer) != -1)
            {
                m_shapeLayer.DataSource = new GeometryFeatureProvider(new List<SharpMap.Geometries.Geometry>());
            }
        }

        public void ShowFeature(Predicate<System.Data.DataRow> p = null)
        {
            List<SharpMap.Geometries.Geometry> geometries = new List<SharpMap.Geometries.Geometry>();

            m_shapeFileData.Open();
            for(uint i=0; i<m_shapeFileData.GetFeatureCount(); ++i)
            {
                SharpMap.Data.FeatureDataRow featureDataRow = m_shapeFileData.GetFeature(i);
                if (p == null || p(featureDataRow))
                {
                    geometries.Add(m_shapeFileData.GetGeometryByID(i));
                }
            }
            m_shapeFileData.Close();

            m_shapeLayer.DataSource = new GeometryFeatureProvider(geometries);
            if (myMap.Layers.IndexOf(m_shapeLayer) == -1)
            {
                myMap.Layers.Add(m_shapeLayer);
            }

            //myMap.ZoomToExtents();
        }

        //public SharpMapMarker(string fileName, GMap.NET.WindowsForms.GMapControl gMapControl)
        //    : this(gMapControl)
        //{
        //    Load(fileName);
        //}

        private static ICoordinateTransformation Transform2Mercator(ICoordinateSystem source)
        {
            CoordinateSystemFactory cFac = new CoordinateSystemFactory();

            List<ProjectionParameter> parameters = new List<ProjectionParameter>();
            parameters.Add(new ProjectionParameter("latitude_of_origin", 0));
            parameters.Add(new ProjectionParameter("central_meridian", 0));
            parameters.Add(new ProjectionParameter("false_easting", 0));
            parameters.Add(new ProjectionParameter("false_northing", 0));
            IProjection projection = cFac.CreateProjection("Mercator", "Mercator_2SP", parameters);

            IProjectedCoordinateSystem coordsys = cFac.CreateProjectedCoordinateSystem("Mercator",
                                                                                       source as IGeographicCoordinateSystem,
                                                                                       projection, LinearUnit.Metre,
                                                                                       new AxisInfo("East",
                                                                                                    AxisOrientationEnum.East),
                                                                                       new AxisInfo("North",
                                                                                                    AxisOrientationEnum.
                                                                                                        North));

            return new CoordinateTransformationFactory().CreateFromCoordinateSystems(source, coordsys);
        }

        // http://www.sharpgis.net/post/2007/07/27/The-Microsoft-Live-Maps-and-Google-Maps-projection.aspx
        private static ICoordinateTransformation Transform2Mercator2(ICoordinateSystem source)
        {
            string wkt = @"PROJCS[""Mercator Spheric"", GEOGCS[""WGS84basedSpheric_GCS"", DATUM[""WGS84basedSpheric_Datum"", 
        SPHEROID[""WGS84based_Sphere"", 6378137, 0], TOWGS84[0, 0, 0, 0, 0, 0, 0]], PRIMEM[""Greenwich"", 0, AUTHORITY[""EPSG"", ""8901""]], 
        UNIT[""degree"", 0.0174532925199433, AUTHORITY[""EPSG"", ""9102""]], AXIS[""E"", EAST], AXIS[""N"", NORTH]], PROJECTION[""Mercator""], 
        PARAMETER[""False_Easting"", 0], PARAMETER[""False_Northing"", 0], PARAMETER[""Central_Meridian"", 0], PARAMETER[""Latitude_of_origin"", 0],
        UNIT[""metre"", 1, AUTHORITY[""EPSG"", ""9001""]], AXIS[""East"", EAST], AXIS[""North"", NORTH]]";

            CoordinateSystemFactory cFac = new CoordinateSystemFactory();
            ICoordinateSystem g = (ICoordinateSystem)CoordinateSystemWktReader.Parse(wkt); ;
            
            return new CoordinateTransformationFactory().CreateFromCoordinateSystems(source, g);
        }

        private GMap.NET.PureProjection m__mercatorProjection = new GMap.NET.Projections.MercatorProjection();
        ICoordinateTransformation m_googleMapCoordinateTransformation;

        public override void OnRender(System.Drawing.Graphics g)
        {
            try
            {
                if (myMap.Layers.Count == 0)
                {
                    base.OnRender(g);
                    return;
                }

                var right = m__mercatorProjection.FromPixelToLatLng(new GMap.NET.GPoint(m_gMapControl.Size.Width, 0), (int)m_gMapControl.Zoom);
                var left = m__mercatorProjection.FromPixelToLatLng(new GMap.NET.GPoint(0, 0), (int)m_gMapControl.Zoom);

                var right1 = GeometryTransform.TransformPoint(new SharpMap.Geometries.Point(right.Lng, right.Lat), m_googleMapCoordinateTransformation.MathTransform);
                var left1 = GeometryTransform.TransformPoint(new SharpMap.Geometries.Point(left.Lng, left.Lat), m_googleMapCoordinateTransformation.MathTransform);

                myMap.Size = new System.Drawing.Size(m_gMapControl.Width, m_gMapControl.Height);

                myMap.Zoom = right1.X - left1.X;

                // 不是Center，具体怎样还没细看
                //var p = GoogleMapOffset.GetOffseted(m_gMapControl.CurrentPosition);
                var p = m_gMapControl.FromLocalToLatLng(m_gMapControl.Size.Width / 2, m_gMapControl.Size.Height / 2);

                var p1 = GeometryTransform.TransformPoint(new SharpMap.Geometries.Point(p.Lng, p.Lat), m_googleMapCoordinateTransformation.MathTransform);

                myMap.Center = p1;

                var image = myMap.GetMap();
                g.DrawImageUnscaled(image, 0, 0);

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
