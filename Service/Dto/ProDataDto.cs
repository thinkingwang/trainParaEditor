using System;
using System.Data.SqlClient;
using CommonModel;

namespace Service.Dto
{
    public class ProDataDto:Dto
    {
        private readonly ProcData _procData;
        static ProDataDto()
        {
        }
        private ProDataDto(ProcData th)
        {
            _procData = th;
        }
        /// <summary>
        /// 新建指定车型
        /// </summary>
        /// <param name="testDateTime"></param>
        public static void NewProData(DateTime testDateTime)
        {
            var item = new ProcData() { CommitTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")), testDateTime = testDateTime, NeedProc = 1 };
            thrContext.Set<ProcData>().Add(item);
            thrContext.SaveChanges();
            Nlogger.Trace("新建指定车型，指定时刻为：" + testDateTime);
        }

        public static int uphistory2(string testTime)
        {
           return bwContext.Database.ExecuteSqlCommand(@"EXEC [dbo].[uphistory2] @time", new SqlParameter("@time", testTime));
           //return bwContext.uphistory2(testTime);
        }
    }
}