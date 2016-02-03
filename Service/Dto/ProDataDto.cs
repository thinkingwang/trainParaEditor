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
            var item = new ProcData()
            {
                CommitTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                testDateTime = testDateTime,
                NeedProc = 1
            };
            ThrContext.Set<ProcData>().Add(item);
            ThrContext.SaveChanges();
            Nlogger.Trace("重新分析探伤数据，目标时刻为：" + testDateTime);
        }

        public static void uphistory2(string testTime)
        {
            using (var sqlclient = new SqlConnection(Dto.connectStringBase))
            {
               sqlclient.Open();
               SqlCommand cmd = new SqlCommand(String.Format(@"EXEC [dbo].[uphistory2] '{0}'", testTime),sqlclient);
                cmd.ExecuteNonQuery();
            }
        }
    }
}