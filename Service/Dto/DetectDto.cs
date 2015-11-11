using System;
using System.Linq;
using System.Windows.Forms;
using CommonModel;

namespace Service.Dto
{
    public class DetectDto : Dto
    {

        public static string GetEngNum(string time)
        {
            DateTime source = DateTime.Parse(time);
            Detect detect = ThrContext.Set<Detect>().FirstOrDefault(m => m.testDateTime.Equals(source));
            return detect == null ? "" : detect.engNum;
        }
    }
}