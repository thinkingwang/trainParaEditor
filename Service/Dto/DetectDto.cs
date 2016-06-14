using System;
using System.Linq;

namespace Service.Dto
{
    public class DetectDto : Dto
    {
        public static string GetEngNum(string time)
        {
            var engNum =
                ThrContext.Database.SqlQuery<string>(string.Format("select engNum from Detect where testDateTime='{0}'",
                    time));
            return engNum.FirstOrDefault();
        }

        public static short GetAxleSum(string time)
        {
            var engNum =
                ThrContext.Database.SqlQuery<short>(string.Format("select AxleNum from Detect where testDateTime='{0}'",
                    time));
            return engNum.FirstOrDefault();
        }

        public static short? GetWaterLevel(string time)
        {
            try
            {
                var engNum =
                    ThrContext.Database.SqlQuery<byte?>(string.Format("select waterLevel from Detect where testDateTime='{0}'",
                        time));
                return engNum.FirstOrDefault();
            }
            catch(Exception e)
            {
                return -1;
            }
        }

        public static void SetWaterLevel(string time, int? waterLevel)
        {
            var waterLevelOrg = GetWaterLevel(time);
            if (waterLevelOrg == waterLevel)
            {
                return;
            }
            Dto.Nlogger.Trace("对检测时间为：" + time + "，进行了液位报警调整操作，调整前报警级别为：" + waterLevelOrg + ",调整后报警级别为：" + waterLevel);
            ThrContext.Database.ExecuteSqlCommand(
                string.Format("update Detect set waterLevel={0} where testDateTime='{1}'",
                    waterLevel, time));
            ThrContext.SaveChanges();
        }

        public static void SetAxleSum(string time, int axleSum)
        {
            var sumOrg  = GetAxleSum(time);
            if (sumOrg == axleSum)
            {
                return;
            }
            Dto.Nlogger.Trace("对检测时间为：" + time + "，进行了轴数调整操作，调整前轴数为：" + sumOrg + ",调整后轴数为：" + axleSum);
            ThrContext.Database.ExecuteSqlCommand(string.Format(
                "update Detect set AxleNum={0} where testDateTime='{1}'", axleSum,
                time));
            ThrContext.SaveChanges();
        }
    }
}