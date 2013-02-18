using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    [Class(Table = "SD_Track", OptimisticLock = OptimisticLockMode.Version)]
    public class Track : BaseDataEntity
    {
        [Property(NotNull = true, Length = 50)]
        public virtual string Name
        {
            get;
            set;
        }

        [ManyToOne(ForeignKey = "FK_Track_Route", NotNull = false)]
        public virtual Route Route
        {
            get;
            set;
        }

        [Property(NotNull = false)]
        public virtual DateTime? StartTime
        {
            get;
            set;
        }

        [Property(NotNull = false)]
        public virtual DateTime? EndTime
        {
            get;
            set;
        }

        [Property(NotNull = true, Length = 20)]
        public virtual string VehicleName
        {
            get;
            set;
        }

        [Bag(0, Cascade = "none", Inverse = true, OrderBy="GpsTime")]
        [Key(1, Column = "Track")]
        [OneToMany(2, ClassType = typeof(TrackPoint), NotFound = NotFoundMode.Ignore)]
        public virtual IList<TrackPoint> TrackPoints
        {
            get;
            set;
        }

        [Bag(0, Cascade = "none", Inverse = true, OrderBy="GpsTime")]
        [Key(1, Column = "Track")]
        [OneToMany(2, ClassType = typeof(WayPoint), NotFound = NotFoundMode.Ignore)]
        public virtual IList<WayPoint> WayPoints
        {
            get;
            set;
        }

        [Property(NotNull = false, Length = int.MaxValue)]
        public virtual string Gpx
        {
            get;
            set;
        }
    }
}
