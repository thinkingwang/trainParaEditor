using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using CommonModel;

namespace JCModel
{
    public partial class JCContext : ModelContext
    {
        public JCContext(string conStr)
            : base(conStr)
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<JCCarList> CarLists { get; set; }
        public virtual DbSet<CRH_wheel> CRH_wheel { get; set; }
        public virtual DbSet<Detect> Detects { get; set; }
        public virtual DbSet<ProcData> ProcDatas { get; set; }
        public virtual DbSet<ProfileAdjust> ProfileAdjusts { get; set; }
        public virtual DbSet<ProfileDetectResult> ProfileDetectResults { get; set; }
        public virtual DbSet<ProfileDetectResult_real> ProfileDetectResult_real { get; set; }
        public virtual DbSet<JCSequ> Sequs { get; set; }
        public virtual DbSet<threshold> thresholds { get; set; }
        public virtual DbSet<TrainType> TrainTypes { get; set; }
        public virtual DbSet<WhmsTime> WhmsTimes { get; set; }

        public virtual DbSet<EngineLib> EngineLibs { get; set; }

        public virtual DbSet<WheelPos> WheelPoses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JCCarList>()
                .Property(e => e.carNo)
                .IsUnicode(false);

            modelBuilder.Entity<JCCarList>()
                .Property(e => e.carNo2)
                .IsUnicode(false);

            modelBuilder.Entity<CRH_wheel>()
                .Property(e => e.trainType)
                .IsUnicode(false);

            modelBuilder.Entity<Detect>()
                .Property(e => e.engNum)
                .IsUnicode(false);

            modelBuilder.Entity<Detect>()
                .Property(e => e.engBNum)
                .IsUnicode(false);

            modelBuilder.Entity<Detect>()
                .Property(e => e.inSpeed)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Detect>()
                .Property(e => e.outSpeed)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Detect>()
                .Property(e => e.waterTemp)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Detect>()
                .Property(e => e.temperature)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Detect>()
                .Property(e => e.liquidTemp)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Detect>()
                .Property(e => e.arrayATemp)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Detect>()
                .Property(e => e.arrayBTemp)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Detect>()
                .Property(e => e.wheelSize)
                .HasPrecision(6, 2);

            modelBuilder.Entity<Detect>()
                .Property(e => e.wheelSizeB)
                .HasPrecision(6, 2);

            modelBuilder.Entity<Detect>()
                .Property(e => e.procUser)
                .IsUnicode(false);

            modelBuilder.Entity<Detect>()
                .HasOptional(e => e.WhmsTime)
                .WithRequired(e => e.Detect)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ProcData>()
                .Property(e => e.info)
                .IsUnicode(false);

            modelBuilder.Entity<ProfileAdjust>()
                .Property(e => e.Lj)
                .HasPrecision(3, 1);

            modelBuilder.Entity<ProfileAdjust>()
                .Property(e => e.LyHd)
                .HasPrecision(3, 1);

            modelBuilder.Entity<ProfileAdjust>()
                .Property(e => e.LyGd)
                .HasPrecision(3, 1);

            modelBuilder.Entity<ProfileAdjust>()
                .Property(e => e.LwHd)
                .HasPrecision(3, 1);

            modelBuilder.Entity<ProfileAdjust>()
                .Property(e => e.LwHd2)
                .HasPrecision(3, 1);

            modelBuilder.Entity<ProfileAdjust>()
                .Property(e => e.QR)
                .HasPrecision(3, 1);

            modelBuilder.Entity<ProfileAdjust>()
                .Property(e => e.Ncj)
                .HasPrecision(3, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.Lj)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.TmMh)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.LyHd)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.LyGd)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.LwHd)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.LwHd2)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.QR)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.Ncj)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.LjCha_Zhou)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.LjCha_ZXJ)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.LjCha_Che)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.LjCha_Bz)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult>()
                .Property(e => e.xmlFile)
                .IsUnicode(false);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.Lj)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.TmMh)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.LyHd)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.LyGd)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.LwHd)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.LwHd2)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.QR)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.Ncj)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.LjCha_Zhou)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.LjCha_ZXJ)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.LjCha_Che)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.LjCha_Bz)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.xmlFile)
                .IsUnicode(false);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.carNo)
                .IsUnicode(false);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.Lj_user)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.Lj_AVG)
                .HasPrecision(5, 1);

            modelBuilder.Entity<ProfileDetectResult_real>()
                .Property(e => e.LwHd2_AVG)
                .HasPrecision(5, 1);

            modelBuilder.Entity<threshold>()
                .Property(e => e.trainType)
                .IsUnicode(false);

            modelBuilder.Entity<threshold>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<threshold>()
                .Property(e => e.standard)
                .HasPrecision(8, 2);

            modelBuilder.Entity<threshold>()
                .Property(e => e.up_level3)
                .HasPrecision(8, 2);

            modelBuilder.Entity<threshold>()
                .Property(e => e.up_level2)
                .HasPrecision(8, 2);

            modelBuilder.Entity<threshold>()
                .Property(e => e.up_level1)
                .HasPrecision(8, 2);

            modelBuilder.Entity<threshold>()
                .Property(e => e.low_level3)
                .HasPrecision(8, 2);

            modelBuilder.Entity<threshold>()
                .Property(e => e.low_level2)
                .HasPrecision(8, 2);

            modelBuilder.Entity<threshold>()
                .Property(e => e.low_level1)
                .HasPrecision(8, 2);

            modelBuilder.Entity<threshold>()
                .Property(e => e.desc)
                .IsUnicode(false);

            modelBuilder.Entity<threshold>()
                .Property(e => e.precision)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TrainType>()
                .Property(e => e.trainType1)
                .IsUnicode(false);

            modelBuilder.Entity<TrainType>()
                .Property(e => e.format)
                .IsUnicode(false);
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
    }
}