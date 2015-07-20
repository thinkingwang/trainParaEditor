using System;
using System.Linq;
using CommonModel;

namespace Service.Dto
{
    public class DetectDto : Dto
    {


        /// <summary>
        ///     车组号编辑
        /// </summary>
        /// <param name="sourceText"></param>
        /// <param name="no"></param>
        public static void Editor(string sourceText, string no)
        {
            DateTime source = DateTime.Parse(sourceText);
            Detect detect = thrContext.Set<Detect>().FirstOrDefault(m => m.testDateTime.Equals(source));
            if (detect != null)
            {
                detect.engNum = no;
                thrContext.SaveChanges();
                Nlogger.Trace("对操作对象（时刻作为主键）：" + detect.testDateTime + "，进行了车组号编辑操作");
            }
        }

        public static string GetEngNum(string time)
        {
            DateTime source = DateTime.Parse(time);
            Detect detect = thrContext.Set<Detect>().FirstOrDefault(m => m.testDateTime.Equals(source));
            return detect == null ? "" : detect.engNum;
        }
    }
}