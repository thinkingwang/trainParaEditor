using System;
using System.Linq;
using System.Windows.Forms;
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
            Detect detect = ThrContext.Set<Detect>().FirstOrDefault(m => m.testDateTime.Equals(source));
            if (detect != null)
            {
                detect.engNum = no;
                ThrContext.SaveChanges();
                Nlogger.Trace("对操作对象（时刻作为主键）：" + detect.testDateTime + "，进行了车组号编辑操作");
            }
            var result = MessageBox.Show(@"是否要进行重新分析？", @"确认对话框", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }
            Dto.ThrContext.Reanalysis(source);
        }

        public static string GetEngNum(string time)
        {
            DateTime source = DateTime.Parse(time);
            Detect detect = ThrContext.Set<Detect>().FirstOrDefault(m => m.testDateTime.Equals(source));
            return detect == null ? "" : detect.engNum;
        }
    }
}