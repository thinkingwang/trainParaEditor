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

namespace JCModel
{
    public partial class JCContext : ModelContext
    {
        public JCContext(string conStr)
            : base(conStr)
        {
        }

        public virtual DbSet<JCCarList> CarLists { get; set; }
        public virtual DbSet<Detect> Detects { get; set; }
        public virtual DbSet<CRH_wheel> CRH_wheel { get; set; }
        public virtual DbSet<ProcData> ProcDatas { get; set; }
        public virtual DbSet<General> Generals { get; set; }
        public virtual DbSet<ProfileAdjust> ProfileAdjusts { get; set; }
        public virtual DbSet<ProfileDetectResult> ProfileDetectResults { get; set; }
        public virtual DbSet<ProfileDetectResult_real> ProfileDetectResult_real { get; set; }
        public virtual DbSet<threshold> thresholds { get; set; }
        public virtual DbSet<TrainType> TrainTypes { get; set; }
        public virtual DbSet<WhmsTime> WhmsTimes { get; set; }

        public virtual DbSet<EngineLib> EngineLibs { get; set; }

        public virtual DbSet<WheelPos> WheelPoses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<CarList>();
        }

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
            var carNew = new JCCarList()
            {
                testDateTime = time,
                posNo = 0,
                carNo = "",
                carNo2 = "",
                direction = false
            };
            var cars = (from v in CarLists where v.testDateTime.Equals(time) orderby v.posNo select v).ToList();
            if (cars.Any())
            {
                carNew.posNo = Convert.ToByte(cars.ElementAt(cars.Count() - 1).posNo + 1);
            }
            CarLists.Add(carNew);
        }


        public override void CsRecovery(DateTime time)
        {
            Database.ExecuteSqlCommand(string.Format(
                "update CSSequ set status1=0 ,status2=0 ,status3=0£¬status4=0  where testDateTime='{0}'",
                time));
            SaveChanges();
        }

        public override void TsRecovery(DateTime time)
        {
            Database.ExecuteSqlCommand(string.Format(
                "update Sequ set status1=0 ,status2=0 ,status3=0,status4=0,status5=0,status6=0   where testDateTime='{0}'",
                time));
            Database.ExecuteSqlCommand(string.Format(
                "update Detect set isValid=0 where testDateTime='{0}'",
                time));
            SaveChanges();
        }

        public override CarList GetCarList(Expression<Func<CarList, bool>> predicate)
        {
            return CarLists.FirstOrDefault(predicate);
        }

        public override void AddCarList(CarList carlist)
        {
            var car = carlist as JCCarList;
            if (car == null)
            {
                return;
            }
            Set<JCCarList>().AddOrUpdate(car);
        }

        public override void RemoveCarList(CarList carlist)
        {
            var car = carlist as JCCarList;
            if (car == null)
            {
                return;
            }
            Set<JCCarList>().Remove(car);
        }
    }
}