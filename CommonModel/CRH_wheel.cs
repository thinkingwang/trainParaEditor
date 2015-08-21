using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Serializable]
    public partial class CRH_wheel
    {
        private static readonly NLog.Logger Nlogger = NLog.LogManager.GetLogger("Modifer");
        private byte _wheelPos;
        private byte _axlePos;

        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        [ReadOnly(true), DisplayName(@"车型")]
        public string trainType { get; set; }

        [Key]
        [Column(Order = 1)]
        [ReadOnly(true), DisplayName(@"轴序号")]
        public byte axleNo { get; set; }

        [Key]
        [Column(Order = 2)]
        [ReadOnly(true), DisplayName(@"轮位置")]
        public byte wheelNo { get; set; }

        [DisplayName(@"轴位")]
        public byte axlePos
        {
            get { return _axlePos; }
            set
            {
                Nlogger.Trace("编辑动车车轮(CRH_wheel)轴位，初始为：" + axlePos + ",修改后为：" + value);
                _axlePos = value;
            }
        }

        [DisplayName(@"轮位")]
        public byte wheelPos
        {
            get { return _wheelPos; }
            set
            {
                Nlogger.Trace("编辑动车车轮(CRH_wheel)轮位，初始为：" + wheelPos + ",修改后为：" + value);
                _wheelPos = value;
            }
        }
    }
}
