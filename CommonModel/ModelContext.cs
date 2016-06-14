using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace CommonModel
{
    public abstract class ModelContext : DbContext
    {
        protected ModelContext(string constr)
            : base(constr)
        {
        }

        public abstract IEnumerable<CarList> GetCarLists(DateTime time);
        public abstract void DeleteCarList(DateTime time, int index);
        public abstract void InsertCarList(DateTime time);
        public abstract void CsRecovery(DateTime time);
        public abstract void TsRecovery(DateTime time);

        public virtual string GetCarNo(ProfileDetectResult result)
        {
            var carNo =
                Database.SqlQuery<string>(string.Format("select dbo.GetCarNoByAxleNo('{0}',{1})",
                    result.testDateTime.ToString("yyyy-MM-dd HH:mm:ss"), result.axleNo));
            return carNo.FirstOrDefault();
        }

        public abstract CarList GetCarList(Expression<Func<CarList, bool>> predicate);
        public abstract void AddCarList(CarList carlist);
        public abstract void RemoveCarList(CarList carlist);

        public virtual int Profile(DateTime testTime)
        {
            var paras = new SqlParameter
            {
                ParameterName = "@testTime",
                Value = testTime
            };
            return Database.ExecuteSqlCommand("exec [dbo].[Profile] @testTime", paras);
        }

        /// <summary>
        ///     探伤检测数据重新分析
        /// </summary>
        /// <returns></returns>
        public virtual int SetVersion(string name, string version)
        {
            try
            {
                return Database.ExecuteSqlCommand(string.Format("exec [dbo].[SetVersion] '{0}','{1}'", name, version));
            }
            catch
            {
            }
            return -1;
        }

        public void Detach(object entity)
        {
            ((IObjectContextAdapter) this).ObjectContext.Detach(entity);
        }

        public virtual int RepairCarListForNewTrain(string testTime, string carNo, bool direction)
        {
            var para1 = new SqlParameter
            {
                ParameterName = "@in_testDateTime",
                Value = testTime
            };
            var para2 = new SqlParameter
            {
                ParameterName = "@in_carNo",
                Value = carNo
            };
            var para3 = new SqlParameter
            {
                ParameterName = "@in_direction",
                Value = direction
            };
            var para4 = new SqlParameter
            {
                ParameterName = "@out_Result",
                Direction = ParameterDirection.Output,
                Value = 0
            };
            Database.ExecuteSqlCommand(
                "exec [dbo].[RepairCarListForNewTrain] @in_testDateTime,@in_carNo,@in_direction,@out_Result OUTPUT",
                para1, para2, para3, para4);
            return Convert.ToInt32(para4.Value);
        }

        public virtual int Proc2(DateTime testTime)
        {
            var paras = new SqlParameter
            {
                ParameterName = "@testTime",
                Value = testTime.ToString("yyyy-MM-dd HH:mm:ss")
            };
            return Database.ExecuteSqlCommand("exec [dbo].[proc2] @testTime", paras);
        }

        public virtual int Proc1(DateTime testTime)
        {
            var paras = new SqlParameter
            {
                ParameterName = "@testTime",
                Value = testTime.ToString("yyyy-MM-dd HH:mm:ss")
            };
            return Database.ExecuteSqlCommand("exec [dbo].[proc1] @testTime", paras);
        }

        public virtual int Reanalysis(DateTime testTime)
        {
            return
                Database.ExecuteSqlCommand(string.Format("exec [dbo].[Reanalysis] '{0}'",
                    testTime.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        public virtual int Profile_LjCha(DateTime testTime)
        {
            return
                Database.ExecuteSqlCommand(string.Format("exec [dbo].[Profile_LjCha] '{0}'",
                    testTime.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        public virtual int proc_caShangDataFill(DateTime thisTime, DateTime lastTime)
        {
            var paras = new SqlParameter[3];
            paras[0] = new SqlParameter
            {
                ParameterName = "@in_ThisTestDateTime",
                Value = thisTime
            };
            paras[1] = new SqlParameter
            {
                ParameterName = "@in_LastTestDateTime",
                Value = lastTime
            };

            paras[2] = new SqlParameter
            {
                ParameterName = "@out_Result",
                Value = -1,
                Direction = ParameterDirection.Output
            };
            Database.ExecuteSqlCommand(
                "exec [dbo].[proc_caShangDataFill] @in_ThisTestDateTime,@in_LastTestDateTime,@out_Result OUTPUT", paras);
            return Convert.ToInt32(paras[2].Value);
        }

        public virtual int proc_tanShangDataFill(DateTime thisTime, DateTime lastTime)
        {
            var paras = new SqlParameter[3];
            paras[0] = new SqlParameter
            {
                ParameterName = "@in_ThisTestDateTime",
                Value = thisTime
            };
            paras[1] = new SqlParameter
            {
                ParameterName = "@in_LastTestDateTime",
                Value = lastTime
            };

            paras[2] = new SqlParameter
            {
                ParameterName = "@out_Result",
                Value = -1,
                Direction = ParameterDirection.Output
            };
            Database.ExecuteSqlCommand(
                "exec [dbo].[proc_tanShangDataFill] @in_ThisTestDateTime,@in_LastTestDateTime,@out_Result OUTPUT", paras);
            return Convert.ToInt32(paras[2].Value);
        }

        public virtual int proc_BatchDatafillByLastTime(DateTime thisTime, DateTime lastTime)
        {
            var paras = new SqlParameter[3];
            paras[0] = new SqlParameter
            {
                ParameterName = "@in_ThisTestDateTime",
                Value = thisTime
            };
            paras[1] = new SqlParameter
            {
                ParameterName = "@in_LastTestDateTime",
                Value = lastTime
            };

            paras[2] = new SqlParameter
            {
                ParameterName = "@out_Result",
                Value = -1,
                Direction = ParameterDirection.Output
            };
            Database.ExecuteSqlCommand(
                "exec [dbo].[proc_BatchDatafillByLastTime] @in_ThisTestDateTime,@in_LastTestDateTime,@out_Result OUTPUT",
                paras);
            return Convert.ToInt32(paras[2].Value);
        }
    }
}