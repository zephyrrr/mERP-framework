using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    [Class(Table = "SD_Way_Point", OptimisticLock = OptimisticLockMode.Version)]
    public class WayPoint : GpsData
    {
        public WayPoint()
        {
        }

        public WayPoint(GpsData that)
        {
            this.Accuracy = that.Accuracy;
            this.Altitude = that.Altitude;
            this.GpsTime = that.GpsTime;
            this.Heading = that.Heading;
            this.IsActive = that.IsActive;
            this.Latitude = that.Latitude;
            this.Longitude = that.Longitude;
            this.MessageTime = that.MessageTime;
            this.Speed = that.Speed;
            this.VehicleName = that.VehicleName;
        }
        ///<summary>
        /// 动作
        ///</summary>
        [Property(NotNull = false)]
        public virtual string Action
        {
            get;
            set;
        }

        [ManyToOne(ForeignKey = "FK_WayPoint_Track", NotNull = true)]
        public virtual Track Track
        {
            get;
            set;
        }

        //[Property(NotNull = false)]
        //public TimeSpan? TimeLeft
        //{
        //    get;
        //    set;
        //}

        //[Property(NotNull = false)]
        //public double? DistanceLeft
        //{
        //    get;
        //    set;
        //}

        //[Property(NotNull = true)]
        //public double TimeStay
        //{
        //    get;
        //    set;
        //}
    }
}
