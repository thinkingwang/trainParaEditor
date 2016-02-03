using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonModel;
using CommonModel.Common;
using JCModel;

namespace Service.Dto
{
    public class ProfileDetectResultDto : Dto
    {
        private static  string _picturePath = @"D:\WPMS\var\Supervisor\Storage\WhprM";
        private ProfileDetectResult _profileDetectResult;

        static ProfileDetectResultDto()
        {
            if (Document != null)
            {
                var node = Document.SelectSingleNode(string.Format("/config/add[@key='{0}']/@value", "ProfilePath"));
                if (node != null)
                {
                    _picturePath = node.Value;
                }
            }
        }

        private ProfileDetectResultDto(ProfileDetectResult tt)
        {
            _profileDetectResult = tt;
        }
        /// <summary>
        /// 获得门限表中指定车型的所有数据
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static IEnumerable<ProfileDetectResultDto> GetProfileDetectResultDtos(DateTime time)
        {
            var result = new List<ProfileDetectResultDto>();
            var data = from v in ThrContext.Set<ProfileDetectResult>()
                       where v.testDateTime.Equals(time)
                       select v;
            foreach (var thresholdse in data)
            {
                ThrContext.Entry(thresholdse).Reload();
                result.Add(new ProfileDetectResultDto(thresholdse));
            }
            return result;
        }

        /// <summary>
        /// 获得门限表中指定车型的所有数据
        /// </summary>
        /// <param name="time"></param>
        /// <param name="axel"></param>
        /// <param name="wheel"></param>
        /// <param name="desTime"></param>
        /// <param name="desaxel"></param>
        /// <param name="deswheel"></param>
        /// <returns></returns>
        public static void CopySingleProfileDetectResultDto(DateTime time, int axel, byte wheel, DateTime desTime, int desaxel, byte deswheel)
        {
            var source =
                ThrContext.Set<ProfileDetectResult>().FirstOrDefault(
                    m => m.testDateTime.Equals(time) && m.axleNo.Equals(axel) && m.wheelNo.Equals(wheel));
            var des =
                ThrContext.Set<ProfileDetectResult>().FirstOrDefault(
                    m => m.testDateTime.Equals(desTime) && m.axleNo.Equals(desaxel) && m.wheelNo.Equals(deswheel));
            if (source == null||des == null)
            {
                return;
            }
            des.LwHd = source.LwHd;
            des.LyGd = source.LyGd;
            des.LyHd = source.LyHd;
            des.QR = source.QR;
            des.TmMh = source.TmMh;
            des.Ncj = source.Ncj;
            des.Lj = source.Lj;
            ThrContext.SaveChanges();
            ThrContext.Profile(desTime);
            Nlogger.Trace("对操作对象（时刻作为主键）：" + time + "轮号为：" + axel + "，进行了单轮外形补缺操作，源时刻为：" + desTime + "轮号为：" + desaxel);
        }

        /// <summary>
        /// 获得门限表中指定车型的所有数据
        /// </summary>
        /// <param name="time"></param>
        /// <param name="axel"></param>
        /// <param name="wheel"></param>
        /// <param name="desTime"></param>
        /// <param name="desaxel"></param>
        /// <param name="deswheel"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static void CopySpecialProfileDetectResultDto(DateTime time, int axel, byte wheel, DateTime desTime,
            int desaxel, byte deswheel, List<SelectColumn> columns)
        {
            var source =
                ThrContext.Set<ProfileDetectResult>().FirstOrDefault(
                    m => m.testDateTime.Equals(time) && m.axleNo.Equals(axel) && m.wheelNo.Equals(wheel));
            var des =
                ThrContext.Set<ProfileDetectResult>().FirstOrDefault(
                    m => m.testDateTime.Equals(desTime) && m.axleNo.Equals(desaxel) && m.wheelNo.Equals(deswheel));
            if (source == null || des == null)
            {
                return;
            }
            foreach (var selectColumn in columns)
            {
                switch (selectColumn)
                {
                    case SelectColumn.TmMh:
                        des.TmMh = source.TmMh;
                        break;
                    case SelectColumn.LyGd:
                        des.LyGd = source.LyGd;
                        break;
                    case SelectColumn.LyHd:
                        des.LyHd = source.LyHd;
                        break;
                    case SelectColumn.LwHd:
                        des.LwHd = source.LwHd;
                        break;
                    case SelectColumn.QR:
                        des.QR = source.QR;
                        break;
                    case SelectColumn.Ncj:
                        des.Ncj = source.Ncj;
                        break;
                    case SelectColumn.Lj:
                        des.Lj = source.Lj;
                        break;
                }
            }
            ThrContext.Set<ProfileDetectResult>().AddOrUpdate(des);
            ThrContext.SaveChanges();
            ThrContext.Profile_LjCha(desTime);
            ThrContext.Profile(desTime);
            Nlogger.Trace("对操作对象（时刻作为主键）：" + time + "轮号为：" + axel + "，进行了特定项外形补缺操作，源时刻为：" + desTime + "轮号为：" + desaxel);
        }

        /// <summary>
        /// 复制指定车厢的所有数据
        /// </summary>
        /// <param name="time"></param>
        /// <param name="carNo1"></param>
        /// <param name="desTime"></param>
        /// <param name="carNo2"></param>
        /// <returns></returns>
        public static void CopyCarNoProfileDetectResultDto(DateTime time, string carNo1, DateTime desTime)
        {

            var posDes =
                ThrContext.GetCarList(m => m.testDateTime.Equals(desTime) && m.carNo.Equals(carNo1));
            if (posDes == null)
            {
                MessageBox.Show("目标检测车次没有该车厢号，如果要继续操作，请先进行车号补缺操作");
                return;
            }
            var pos =
                ThrContext.GetCarList(m => m.testDateTime.Equals(time) && m.carNo.Equals(carNo1));
            var sources = (from a in ThrContext.Set<ProfileDetectResult>()
                where a.testDateTime.Equals(time) && (a.axleNo >= pos.posNo*4 && a.axleNo <= (pos.posNo*4 + 3))
                orderby a.axleNo
                orderby a.wheelNo
                select a).ToList();
            if (pos == null)
            {
                MessageBox.Show("CarList表没有源车型对应的数据");
                return;
            }
            foreach (var profileDetectResult in sources)
            {
                var result = profileDetectResult.Copy() as ProfileDetectResult;
                if (result == null)
                {
                    continue;
                }
                result.testDateTime = desTime;
                result.axleNo = posDes.posNo*4 + result.axleNo%4;
                if (Dto.ServerConfig.Type != "DC")
                {
                    var carDes = posDes as JCCarList;
                    var carSou = pos as JCCarList;
                    if (carDes == null)
                    {
                        return;
                    }
                    if (!carDes.direction.Equals(carSou.direction))
                    {
                        result.wheelNo = Convert.ToByte(Math.Abs(result.wheelNo - 1));
                        result.axleNo = posDes.posNo*4 + (3 - result.axleNo%4);
                    }
                }
                ThrContext.Set<ProfileDetectResult>().AddOrUpdate(result);
            }
            //Nlogger.Trace("对操作对象（时刻作为主键）：" + time + "轮号为：" + axel + "，进行了特定项外形补缺操作，源时刻为：" + desTime + "轮号为：" + desaxel);

            ThrContext.SaveChanges();
            ThrContext.Profile_LjCha(desTime);
            ThrContext.Profile(desTime);
            MessageBox.Show(@"复制所选成功");
        }

        /// <summary>
        /// 外形补缺
        /// </summary>
        /// <param name="thisTimeText"></param>
        /// <param name="lastTimeText"></param>
        public static void Repair(string thisTimeText, string lastTimeText)
        {
            var thisTime = DateTime.Parse(thisTimeText);
            var lastTime = DateTime.Parse(lastTimeText);
            if (thisTime.ToString(CultureInfo.InvariantCulture).Equals(lastTime.ToString(CultureInfo.InvariantCulture)))
            {
                return;
            }
            Nlogger.Trace("对操作对象（时刻作为主键）：" + thisTimeText + "，进行了外形补缺操作，源时刻为：" + lastTimeText);
            var result = ThrContext.proc_BatchDatafillByLastTime(thisTime, lastTime);
            switch (result)
            {
                case 0:
                    MessageBox.Show(@"出错");
                    return;
                case 1:
                    MessageBox.Show(@"修复成功");
                    break;
                case 2:
                    MessageBox.Show(@"ProfileDetectResult表无上次日期数据");
                    return;
                case 3:
                    MessageBox.Show(@"ProfileDetectResult_real表无上次日期数据");
                    return;
            }
            if (ThrContext.Set<ProfileDetectResult>().FirstOrDefault(m => m.testDateTime.Equals(lastTime)) != null)
            {
                var time = ThrContext.Set<WhmsTime>().FirstOrDefault(m => m.tychoTime.Equals(lastTime));
                if (time == null)
                {
                    return;
                }
                var oldValue = time.whmsTime1.ToString("yyyyMMdd_HHmmss");
                var newValue = thisTime.ToString("yyyyMMdd_HHmmss");
                var sourcePath = _picturePath + @"\" + oldValue;
                if (!Directory.Exists(sourcePath))
                {
                    MessageBox.Show(sourcePath + @"目录不存在");
                    return;
                }
                var desPath = _picturePath + @"\" + newValue;
                var files = Directory.GetFiles(sourcePath);
                Directory.CreateDirectory(desPath);

                foreach (var file in files)
                {
                    var fileName = file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                    if (fileName.Contains(oldValue))
                    {
                        File.Copy(file, desPath + "\\" + fileName.Replace(oldValue, newValue),true);
                    }
                }
            }
            else
            {
                MessageBox.Show(@"数据库不存在该项：" + lastTime);
            }
        }

        [DisplayName(@"车厢号")]
        public string carNo
        {
            get
            {
                var carlist =
                    Dto.ThrContext.GetCarNo(_profileDetectResult);
                if (carlist == null)
                {
                    return "";
                }
                return carlist;
            }
        }

        [DisplayName(@"轴号")]
        public int axleNo
        {
            get { return _profileDetectResult.axleNo; }
        }
        [DisplayName(@"轮号")]
        public byte wheelNo
        {
            get { return _profileDetectResult.wheelNo; }
        }
        [DisplayName(@"踏面磨耗")]
        public decimal? TmMh
        {
            get { return _profileDetectResult.TmMh; }
        }
        [DisplayName(@"轮缘厚度")]
        public decimal? LyHd
        {
            get { return _profileDetectResult.LyHd; }
        }
        [DisplayName(@"轮缘高度")]
        public decimal? LyGd
        {
            get { return _profileDetectResult.LyGd; }
        }
        [DisplayName(@"轮辋宽度")]
        public decimal? LwHd
        {
            get { return _profileDetectResult.LwHd; }
            //set
            //{
            //    _profileDetectResult.LwHd = value;
            //    ThrContext.SaveChanges();
            //}
        }
        [DisplayName(@"QR值")]
        public decimal? QR
        {
            get { return _profileDetectResult.QR; }
        }
        [DisplayName(@"轮径")]
        public decimal? Lj
        {
            get { return _profileDetectResult.Lj; }
        }
        [DisplayName(@"内侧距")]
        public decimal? Ncj
        {
            get { return _profileDetectResult.Ncj; }
        }
    }
}