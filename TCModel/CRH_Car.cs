using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCModel
{
    [Serializable]
    public class CRH_Car
    {
        [Key]
        [DisplayName("车型")]
        [StringLength(20)]
        [Column(Order = 0)]
        [ReadOnly(true)]
        public string trainType { get; set; }

        [Key]
        [Column(Order = 1)]
        [DisplayName("车厢序号")]
        [ReadOnly(true)]
        public byte carPos { get; set; }

        [DisplayName("车厢编号")]
        [StringLength(10)]
        [ReadOnly(true)]
        public string carNo { get; set; }

        [DisplayName("车轴编号")]
        [StringLength(10)]
        [ReadOnly(true)]
        public string axleType { get; set; }

        [DisplayName("方向")]
        [ReadOnly(true)]
        public bool dir { get; set; }

        [DisplayName("车厢类型")]
        public byte powerType { get; set; }
    }
}