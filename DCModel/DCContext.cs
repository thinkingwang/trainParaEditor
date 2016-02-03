using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using CommonModel;

namespace DCModel
{
    public partial class DCContext : ModelContext
    {
        public DCContext(string conStr)
            : base(conStr)
        {
        }

        public virtual DbSet<CarList> CarLists { get; set; }
        public virtual DbSet<Detect> Detects { get; set; }
        public virtual DbSet<CRH_wheel> CRH_wheel { get; set; }
        public virtual DbSet<General> Generals { get; set; }
        public virtual DbSet<ProcData> ProcDatas { get; set; }
        public virtual DbSet<ProfileAdjust> ProfileAdjusts { get; set; }
        public virtual DbSet<ProfileDetectResult> ProfileDetectResults { get; set; }
        public virtual DbSet<ProfileDetectResult_real> ProfileDetectResult_real { get; set; }
        public virtual DbSet<threshold> thresholds { get; set; }
        public virtual DbSet<TrainType> TrainTypes { get; set; }
        public virtual DbSet<WhmsTime> WhmsTimes { get; set; }


        public override IEnumerable<CarList> GetCarLists(DateTime time)
        {
            var datas = from a in CarLists where a.testDateTime.Equals(time) select a;
            return datas.ToList();
        }


        public override void DeleteCarList(DateTime time, int index)
        {
            var datas = from a in CarLists where a.testDateTime.Equals(time) select a;
            CarLists.Remove(
                            (from v in CarLists where v.testDateTime.Equals(time) select v)
                                .ToList().ElementAt(index));
        }

        public override void InsertCarList(DateTime time)
        {
            var carNew = new CarList()
            {
                testDateTime = time,
                posNo = 0,
                carNo = "",
                carNo2 = ""
            };
            var cars = (from v in CarLists where v.testDateTime.Equals(time) orderby v.posNo select v).ToList();
            if (cars.Any())
            {
                carNew.posNo = Convert.ToByte(cars.ElementAt(cars.Count() - 1).posNo + 1);
            }
            CarLists.Add(carNew);
        }

        public override CarList GetCarList(Expression<Func<CarList, bool>> predicate)
        {
            return CarLists.FirstOrDefault(predicate);
        }

        public override void AddCarList(CarList carlist)
        {
            CarLists.AddOrUpdate(carlist);
        }

        public override void RemoveCarList(CarList carlist)
        {
            CarLists.Remove(carlist);
        }
    }
}
