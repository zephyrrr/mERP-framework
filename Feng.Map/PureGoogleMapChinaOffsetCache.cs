using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Map
{
    [CLSCompliant(false)]
    public interface PureGoogleMapChinaOffsetCache
    {
        bool PutOffsetToCache(Int32 key, GMap.NET.GPoint offset);

        GMap.NET.GPoint? GetOffsetFromCache(Int32 key);
    }
}
