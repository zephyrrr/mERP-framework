using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Map.Obsolete
{
    public interface PureGoogleMapChinaOffsetCache2
    {
        bool PutOffsetToCache(GMap.NET.GPoint pos, int zoom, System.Drawing.Size offset);

        System.Drawing.Size? GetOffsetFromCache(GMap.NET.GPoint pos, int zoom);
    }
}
