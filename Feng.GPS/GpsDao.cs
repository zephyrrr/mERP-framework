using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    public class DirectionDao : MultiOrgEntityDao<Direction>
    {
        public static Direction GetDirection(string name)
        {
            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<Direction>())
            {
                var ret = rep.Get<Direction>(name);

                return ret;
            }
        }
    }
    public class TrackDao : MultiOrgEntityDao<Track>
    {
        public static IList<Track> GetTracks(string vehicleName, DateTime start, DateTime end)
        {
            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<TrackPoint>())
            {
                var ret = rep.List<Track>("from Track where VehicleName = :VehicleName and StartTime >= :startTime and EndTime <= :endTime and IsActive = true",
                    new Dictionary<string, object> { { "VehicleName", vehicleName }, { "startTime", start }, { "endTime", end } });

                return ret;
            }
        }

        public static Track GetTrack(long trackId)
        {
            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<TrackPoint>())
            {
                return rep.Get<Track>(trackId);
            }
        }
    }

    public class RouteDao : MultiOrgEntityDao<Route>
    {
        public static Route GetRoute(string routeName)
        {
            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<TrackPoint>())
            {
                var ret = rep.Get<Route>(routeName);

                return ret;
            }
        }
    }

    public class WayPointDao : BaseDao<WayPoint>
    {
        public static IList<WayPoint> GetWaypoints(Track track)
        {
            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<TrackPoint>())
            {
                var ret = rep.List<WayPoint>("from WayPoint g where g.Track = :Track and IsActive = 1 order by g.GpsTime asc",
                    new Dictionary<string, object> { { "Track", track } });

                return ret;
            }
        }
    }
    public class TrackPointDao : BaseDao<TrackPoint>
    {
        public static TrackPoint GetLastestTrackPoint(Track track)
        {
            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<TrackPoint>())
            {
                //var ret = (rep as Feng.NH.INHibernateRepository).Session.CreateCriteria<TrackPoint>()
                //        .Add(NHibernate.Criterion.Expression.Eq("Track", track))
                //        .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                //        .AddOrder(NHibernate.Criterion.Order.Desc("GpsTime"))
                //        .SetMaxResults(1)
                //        .List<TrackPoint>();
                var ret = rep.List<TrackPoint>("from TrackPoint where Track = :Track and IsActive = true order by GpsTime desc",
                    new Dictionary<string, object> { { "Track", track }, { "MaxResults", 1 } });
                if (ret.Count == 0)
                    return null;
                return ret[0];
            }
        }

        public static IList<TrackPoint> GetTrackPoints(DateTime start, DateTime end)
        {
            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<TrackPoint>())
            {
                var ret = rep.List<TrackPoint>("from TrackPoint where GpsTime >= :GPSTime_1 and GpsTime <= :GPSTime_2 and IsActive = 1 order by GpsTime asc",
                   new Dictionary<string, object> { { "GPSTime_1", start }, { "GPSTime_2", end } });

                return ret;
            }
        }

        public static IList<TrackPoint> GetTrackPoints(string vehicleName, DateTime start, DateTime end)
        {
            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<TrackPoint>())
            {
                var ret = rep.List<TrackPoint>("from TrackPoint where VehicleName = :VehicleName and GpsTime >= :GPSTime_1 and GpsTime <= :GPSTime_2 and IsActive = 1 order by GpsTime asc",
                    new Dictionary<string, object> { { "VehicleName", vehicleName }, { "GPSTime_1", start }, { "GPSTime_2", end } });

                return ret;
            }
        }

        public static IList<TrackPoint> GetTrackPoints(Track track)
        {
            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<TrackPoint>())
            {
                var ret = rep.List<TrackPoint>("from TrackPoint where Track = :Track and IsActive = true order by GpsTime asc",
                    new Dictionary<string, object> { { "Track", track } });

                return ret;
            }
        }
    }
}
