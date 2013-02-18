using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GMap.NET;

namespace GMap.NET.WindowsForms.Markers
{
    [CLSCompliant(false)]
    public class GMapMarkerCircle : GMap.NET.WindowsForms.GMapMarker
    {
        //public Pen Pen;
        public Brush Brush;
        public GMapMarkerCircle(PointLatLng p)
            : base(p)
        {
            this.Brush = new SolidBrush(Color.Red);
        }

        public int Radius
        {
            get { return m_radius; }
            set { m_radius = value; }
        }

        private int m_radius = 5;
        public override void OnRender(Graphics g)
        {
            g.FillEllipse(this.Brush, base.LocalPosition.X - m_radius, base.LocalPosition.Y - m_radius, 2 * m_radius, 2 * m_radius);
        }
    }
}