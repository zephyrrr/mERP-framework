using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace Feng.Map
{
    [CLSCompliant(false)]
    public class KmlOverlay : GMapOverlay
    {
        public KmlOverlay(GMapControl control, string id)
            : base(control, id)
        {
        }

        public static GMapRoute GetRouteFromKml(string fileName)
        {
            try
            {
                //XDocument root = XDocument.Load(new System.IO.StreamReader(fileName));
                XElement root = XElement.Load(new System.IO.StreamReader(fileName));
                IEnumerable<string> coordinates = from c in root.Descendants(XName.Get("coordinates", root.Name.NamespaceName))
                                                  select (string)c;
                foreach (string c in coordinates)
                {
                    List<PointLatLng> points = new List<GMap.NET.PointLatLng>();

                    string[] ss = c.Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string sss in ss)
                    {
                        string[] ss2 = sss.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        points.Add(new PointLatLng(Convert.ToDouble(ss2[1]), Convert.ToDouble(ss2[0])));
                    }

                    GMapRoute rt = new GMapRoute(points, string.Empty);
                    {
                        rt.Stroke = new Pen(Color.FromArgb(144, Color.Red));
                        rt.Stroke.Width = 5;
                        rt.Stroke.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    }
                    return rt;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return null;
        }

        public void Load(string fileName)
        {
            this.Routes.Clear();

            GMapRoute rt = GetRouteFromKml(fileName);
            if (rt != null)
            {
                this.Routes.Add(rt);
            }
        }
    }
}
