using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace CommonModel
{
    public abstract class ModelContext : DbContext
    {
        protected ModelContext(string constr)
            : base(constr)
        {

        }


        public virtual int Profile(DateTime testTime)
        {
            var paras = new SqlParameter()
            {
                ParameterName = "@testTime",
                Value = testTime
            };
            return Database.SqlQuery<int>("exec [dbo].[Profile] @testTime", paras).First();
        }
        public virtual int Profile_LjCha(DateTime testTime)
        {
            var paras = new SqlParameter()
            {
                ParameterName = "@testTime",
                Value = testTime
            };
            return Database.ExecuteSqlCommand("exec [dbo].[Profile_LjCha] @testTime", paras);
        }
        public virtual  int proc_caShangDataFill(DateTime thisTime, DateTime lastTime)
        {
            var paras = new SqlParameter[3];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@in_ThisTestDateTime",
                Value = thisTime
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@in_LastTestDateTime",
                Value = lastTime
            };

            paras[2] = new SqlParameter()
            {
                ParameterName = "@out_Result",
                Value = -1,
                Direction = ParameterDirection.Output
            };
            Database.ExecuteSqlCommand("exec [dbo].[proc_caShangDataFill] @in_ThisTestDateTime,@in_LastTestDateTime,@out_Result OUTPUT", paras);
            return Convert.ToInt32(paras[2].Value);
        }
        public virtual int proc_tanShangDataFill(DateTime thisTime, DateTime lastTime)
        {
            var paras = new SqlParameter[3];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@in_ThisTestDateTime",
                Value = thisTime
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@in_LastTestDateTime",
                Value = lastTime
            };

            paras[2] = new SqlParameter()
            {
                ParameterName = "@out_Result",
                Value = -1,
                Direction = ParameterDirection.Output
            };
            Database.ExecuteSqlCommand("exec [dbo].[proc_tanShangDataFill] @in_ThisTestDateTime,@in_LastTestDateTime,@out_Result OUTPUT", paras);
            return Convert.ToInt32(paras[2].Value);
        }
        public virtual int proc_BatchDatafillByLastTime(DateTime thisTime, DateTime lastTime)
        {
            var paras = new SqlParameter[3];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@in_ThisTestDateTime",
                Value = thisTime
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@in_LastTestDateTime",
                Value = lastTime
            };

            paras[2] = new SqlParameter()
            {
                ParameterName = "@out_Result",
                Value = -1,
                Direction = ParameterDirection.Output
            };
            Database.ExecuteSqlCommand("exec [dbo].[proc_BatchDatafillByLastTime] @in_ThisTestDateTime,@in_LastTestDateTime,@out_Result OUTPUT", paras);
            return Convert.ToInt32(paras[2].Value);
        }
        //public abstract IEnumerable<GetTrainResult> GetTrain(DateTime testDateTime);
        //public abstract IEnumerable<DetectRawStateDto> GetRowStateList(GetTrainResult result);
        //public abstract IEnumerable<DetectFlawStateDto> GetFlawStateList(GetTrainResult result);
    }
}