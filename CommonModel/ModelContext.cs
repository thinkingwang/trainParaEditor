using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
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
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
            mappingCollection.GenerateViews(new List<EdmSchemaError>());

        }

        public abstract IEnumerable<CarList> GetCarLists(DateTime time);
        public abstract void DeleteCarList(DateTime time,int index);
        public abstract void InsertCarList(DateTime time);
        public abstract CarList GetCarList(Expression<Func<CarList, bool>> predicate);
        public abstract void AddCarList(CarList carlist);
        public abstract void RemoveCarList(CarList carlist);
        public virtual int Profile(DateTime testTime)
        {
            var paras = new SqlParameter()
            {
                ParameterName = "@testTime",
                Value = testTime
            };
            return Database.ExecuteSqlCommand("exec [dbo].[Profile] @testTime", paras);
        }
        public virtual int Reanalysis(DateTime testTime)
        {
            var paras = new SqlParameter()
            {
                ParameterName = "@testTime",
                Value = testTime
            };
            return Database.ExecuteSqlCommand("exec [dbo].[Reanalysis] @testTime", paras);
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