using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Table("CarList")]
    public partial class CarList
    {
        private static readonly NLog.Logger Nlogger = NLog.LogManager.GetLogger("Modifer");

        [DisplayName("检测时间"), ReadOnly(true)]
        [Key]
        [Column(Order = 0)]
        public DateTime testDateTime { get; set; }

        [DisplayName("位置"), ReadOnly(true)]
        [Key]
        [Column(Order = 1)]
        public byte posNo { get; set; }

        [DisplayName("车厢号")]
        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string carNo { get; set; }

        [Browsable(false)]
        [StringLength(20)]
        public string carNo2 { get; set; }

    }
}
