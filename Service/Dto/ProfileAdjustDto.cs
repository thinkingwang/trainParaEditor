using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommonModel;
using CommonModel.Common;

namespace Service.Dto
{
    public class ProfileAdjustDto : Dto
    {
        private ProfileAdjust _profileDetectResult;
        /// <summary>
        /// 获取外形微调表所有数据
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ProfileAdjustDto> GetAll()
        {
            var result = new List<ProfileAdjustDto>();
            var data = from v in ThrContext.Set<ProfileAdjust>()
                       select v;
            foreach (var thresholdse in data)
            {
                result.Add(new ProfileAdjustDto(thresholdse));
            }
            return result;
        }

        private ProfileAdjustDto(ProfileAdjust tt)
        {
            _profileDetectResult = tt;
        }
        [DisplayName(@"轮号")]
        public wheel position
        {
            get { return (wheel)_profileDetectResult.position; }
        }
        [DisplayName(@"轮径")]
        public decimal Lj
        {
            get { return _profileDetectResult.Lj; }
            set
            {
                Nlogger.Trace("编辑表ProfileAdjust的Lj字段，初始为：" + _profileDetectResult.Lj + ",修改后为：" + value);
                _profileDetectResult.Lj = value;
                ThrContext.SaveChanges();
            }
        }
        [DisplayName(@"轮缘厚度")]
        public decimal LyHd
        {
            get { return _profileDetectResult.LyHd; }
            set
            {
                Nlogger.Trace("编辑表ProfileAdjust的axleNum字段，初始为：" + _profileDetectResult.LyHd + ",修改后为：" + value);
                _profileDetectResult.LyHd = value;
                ThrContext.SaveChanges();
            }
        }
        [DisplayName(@"轮缘高度")]
        public decimal LyGd
        {
            get { return _profileDetectResult.LyGd; }
            set
            {
                Nlogger.Trace("编辑表ProfileAdjust的LyGd字段，初始为：" + _profileDetectResult.LyGd + ",修改后为：" + value);
                _profileDetectResult.LyGd = value;
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"轮辋宽度")]
        public decimal LwHd
        {
            get { return _profileDetectResult.LwHd; }
            set
            {
                Nlogger.Trace("编辑表ProfileAdjust的LwHd字段，初始为：" + _profileDetectResult.LwHd + ",修改后为：" + value);
                _profileDetectResult.LwHd = value;
                ThrContext.SaveChanges();
            }
        }
        [DisplayName(@"轮辋厚度")]
        public decimal LwHd2
        {
            get { return _profileDetectResult.LwHd2; }
            set
            {
                Nlogger.Trace("编辑表ProfileAdjust的LwHd2字段，初始为：" + _profileDetectResult.LwHd2 + ",修改后为：" + value);
                _profileDetectResult.LwHd2 = value;
                ThrContext.SaveChanges();
            }
        }
        [DisplayName(@"QR值")]
        public decimal QR
        {
            get { return _profileDetectResult.QR; }
            set
            {
                Nlogger.Trace("编辑表ProfileAdjust的QR字段，初始为：" + _profileDetectResult.QR + ",修改后为：" + value);
                _profileDetectResult.QR = value;
                ThrContext.SaveChanges();
            }
        }
        [DisplayName(@"内侧距")]
        public decimal Ncj
        {
            get { return _profileDetectResult.Ncj; }
            set
            {
                Nlogger.Trace("编辑表ProfileAdjust的Ncj字段，初始为：" + _profileDetectResult.Ncj + ",修改后为：" + value);
                _profileDetectResult.Ncj = value;
                ThrContext.SaveChanges();
            }
        }
    }
}