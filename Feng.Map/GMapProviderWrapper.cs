using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Map
{
    [CLSCompliant(false)]
    public class GMapProviderWrapper : GMap.NET.MapProviders.GMapProvider
    {
        //public static GMapProviderWrapper Instance = new GMapProviderWrapper();

        private GMap.NET.MapProviders.GMapProvider m_provider = GMap.NET.MapProviders.GoogleChinaMapProvider.Instance;

        public GMapProviderWrapper(GMap.NET.MapProviders.GMapProvider provider)
            : base()
        {
            m_provider = provider;
        }

        //public GMap.NET.MapProviders.GMapProvider Provider
        //{
        //    get { return m_provider; }
        //    set { m_provider = value; }
        //}

        public override GMap.NET.PureProjection Projection
        {
            get
            {
                return GoogleMapChinaProjection.Instance2;
            }
        }

        private Guid m_id = Guid.NewGuid();
        public override Guid Id
        {
            get { return m_id; }
        }

        public override GMap.NET.PureImage GetTileImage(GMap.NET.GPoint pos, int zoom)
        {
            return m_provider.GetTileImage(pos, zoom);
        }

        public override string Name
        {
            get { return m_provider.Name; }
        }

        public override GMap.NET.MapProviders.GMapProvider[] Overlays
        {
            get { return m_provider.Overlays; }
        }

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_provider.OnInitialized();
        }
    }
}
