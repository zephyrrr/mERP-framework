using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Utils
{
    public static class RouteHelper
    {
        public static double GetDistance(TrackPoint p1, TrackPoint p2)
        {
            return Math.Sqrt(Math.Pow((p1.Longitude - p2.Longitude), 2) + Math.Pow((p1.Latitude - p2.Latitude), 2));
        }
        private static Dictionary<string, System.Text.RegularExpressions.Regex> s_routeCheckRegexs = new Dictionary<string, System.Text.RegularExpressions.Regex>();
        public static bool IsInRoute(string route, string street)
        {
            if (!s_routeCheckRegexs.ContainsKey(route))
            {
                s_routeCheckRegexs[route] = new System.Text.RegularExpressions.Regex(route, System.Text.RegularExpressions.RegexOptions.Compiled);
            }
            var r = s_routeCheckRegexs[route];
            var success = r.Match(street + ".*");
            return success.Success;
        }

        //public static TimeSpan? GetTimeLeftAccordRoute(Track nowTrack)
        //{
        //    if (string.IsNullOrEmpty(nowTrack.Route.Name))
        //        return null;
        //    try
        //    {
        //        using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<Route>())
        //        {
        //            var route = rep.Get<Route>(nowTrack.Route.Name);
        //            return new TimeSpan(0, route.Time, 0);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return null;
        //}

        private static Dictionary<string, Track> m_routeSampleTracks = new Dictionary<string, Track>();
        public static TimeSpan? GetTimeLeftAccordSampleTrack(Track nowTrack)
        {
            if (string.IsNullOrEmpty(nowTrack.Route.Name))
                return null;

            Track sampleTrack = null;
            if (!m_routeSampleTracks.ContainsKey(nowTrack.Route.Name))
            {
                try
                {
                    using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<Route>())
                    {
                        var route = rep.Get<Route>(nowTrack.Route.Name);
                        if (route.SampleTrackData != null)
                        {
                            sampleTrack = Feng.Windows.Utils.SerializeHelper.DeserializeBson<Track>(route.SampleTrackData);
                        }
                        m_routeSampleTracks[nowTrack.Route.Name] = sampleTrack;

                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                sampleTrack = m_routeSampleTracks[nowTrack.Route.Name];
            }

            if (sampleTrack == null)
                return null;

            if (nowTrack.WayPoints.Count >= sampleTrack.WayPoints.Count)
                return TimeSpan.Zero;

            var searchStartTime = nowTrack.WayPoints.Count == 0 ? 
                sampleTrack.TrackPoints[0].GpsTime : sampleTrack.WayPoints[nowTrack.WayPoints.Count - 1].GpsTime;
            var searchEndTime = sampleTrack.WayPoints[nowTrack.WayPoints.Count].GpsTime;
            var allTimes = Utils.ConvertHelper.ListToArray<TrackPoint, DateTime>(sampleTrack.TrackPoints, (x) =>
                {
                    return x.GpsTime;
                });
            var idx1 = Array.BinarySearch<DateTime>(allTimes, searchStartTime);
            var idx2 = Array.BinarySearch<DateTime>(allTimes, searchEndTime);
            if (idx1 < 0) idx1 = ~idx1;
            if (idx2 < 0) idx2 = ~idx2;
            idx1 = Math.Max(0, idx1);
            idx2 = Math.Min(idx2 + 1, sampleTrack.TrackPoints.Count);
            if (idx2 <= idx1)
                return TimeSpan.Zero;

            TrackPoint minCloseTp = null;
            double minDist = double.MaxValue;
            if (nowTrack.TrackPoints.Count == 0)
            {
                minCloseTp = sampleTrack.TrackPoints[0];
            }
            else
            {
                TrackPoint nowTp = nowTrack.TrackPoints[nowTrack.TrackPoints.Count - 1];
                for (int i = idx1; i < idx2; ++i)
                {
                    var dist = GetDistance(sampleTrack.TrackPoints[i], nowTp);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        minCloseTp = sampleTrack.TrackPoints[i];
                    }
                }
            }

            var lastTime = sampleTrack.TrackPoints[idx2 - 1].GpsTime;
            return lastTime - minCloseTp.GpsTime;
        }

        private static void GenerateRouteSampleTrack(Route route)
        {
            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<Route>())
            {
                Track sampleTrack = null;
                if (string.IsNullOrEmpty(route.Name) || !route.SampleTrack.HasValue)
                    return;

                sampleTrack = rep.Get<Track>(route.SampleTrack.Value);

                foreach (var i in sampleTrack.TrackPoints)
                    i.Track = null;
                foreach (var i in sampleTrack.WayPoints)
                    i.Track = null;
                route.SampleTrackData = Feng.Windows.Utils.SerializeHelper.SerializeBson(sampleTrack);
            }
        }

        private static void GenerateRouteDirectionReals(Route route, IRepository rep)
        {
            Dictionary<string, Route> s_routes = null;
            if (s_routes == null)
            {
                var routes = rep.List<Route>();
                s_routes = new Dictionary<string, Route>();
                foreach (var i in routes)
                {
                    s_routes[i.Name] = i;
                }
            }
            string realRoute = route.DirectionExpression;
            if (string.IsNullOrEmpty(route.DirectionExpression))
            {
                route.DirectionReal = null;
                return;
            }

            string[] subRoute = route.DirectionExpression.Split(new char[] {'|', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < subRoute.Length; ++i)
            {
                string s = subRoute[i].Trim();
                if (s_routes.ContainsKey(s))
                {
                    if (string.IsNullOrEmpty(s_routes[s].DirectionReal))
                    {
                        GenerateRouteDirectionReals(s_routes[s], rep);
                    }
                    if (string.IsNullOrEmpty(s_routes[s].DirectionReal))
                    {
                        throw new ArgumentException(string.Format("There is no route named of {0}", subRoute[i]));
                    }
                    realRoute = realRoute.Replace(subRoute[i], s_routes[subRoute[i]].DirectionReal);
                }
            }
            route.DirectionReal = realRoute;
        }

        //public static void GenerateRouteRealDirection(string routeName)
        //{
        //    using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<Route>())
        //    {
        //        Route route = rep.Get<Route>(routeName);
        //        if (route == null)
        //            return;

        //        GenerateRouteRealDirection(route, rep);
        //    }
        //}
        public static void GenerateRouteSampleTracks(string routeName = null)
        {
            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<Route>())
            {
                IList<Route> list;
                if (string.IsNullOrEmpty(routeName))
                {
                    list = rep.List<Route>();
                }
                else
                {
                    list = new List<Route> { rep.Get<Route>(routeName) };
                }
                foreach (var i in list)
                {
                    GenerateRouteSampleTrack(i);

                    try
                    {
                        rep.BeginTransaction();
                        rep.Update(i);
                        rep.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        rep.RollbackTransaction();
                        ServiceProvider.GetService<IExceptionProcess>().ProcessWithNotify(ex);
                    }
                }
            }
        }
        public static void GenerateRouteRealDirection()
        {
            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<Route>())
            {
                var list = rep.List<Route>();
                foreach (var i in list)
                {
                    using (IRepository rep2 = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<Route>())
                    {
                        GenerateRouteDirectionReals(i, rep2);
                    }

                    try
                    {
                        rep.BeginTransaction();
                        rep.Update(i);
                        rep.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        rep.RollbackTransaction();
                        ServiceProvider.GetService<IExceptionProcess>().ProcessWithNotify(ex);
                    }
                }
            }
        }

        //public static void GenerateDirectionsAccordRoute()
        //{
        //    using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<Route>())
        //    {
        //        var list = rep.List<Route>();

        //        try
        //        {
        //            rep.BeginTransaction();
        //            foreach (var i in list)
        //            {
        //                if (string.IsNullOrWhiteSpace(i.DirectionReal))
        //                    continue;

        //                string[] ss = i.DirectionReal.Split(new char[] { '-', '|', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
        //                foreach (string s in ss)
        //                {
        //                    string name = s.Trim();
        //                    if (rep.Get<Direction>(name) != null)
        //                    {
        //                        continue;
        //                    }

        //                    Direction item = new Direction();
        //                    item.Name = name;
        //                    item.ClientId = 0;
        //                    item.OrgId = 0;
        //                    item.Created = System.DateTime.Now;
        //                    item.CreatedBy = "admin";
        //                    item.IsActive = true;
        //                    item.Distance = -1;
        //                    item.Time = -1;
        //                    item.StartAddress = "Undefined";
        //                    item.EndAddress = "Undfined";

        //                    rep.Save(item);
        //                }
        //            }
        //            rep.CommitTransaction();
        //        }
        //        catch (Exception ex)
        //        {
        //            rep.RollbackTransaction();
        //            ServiceProvider.GetService<IExceptionProcess>().ProcessWithNotify(ex);
        //        }
        //    }
        //}
    }
}
