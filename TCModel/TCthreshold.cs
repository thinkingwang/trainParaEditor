using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonModel;

namespace TCModel
{
    [Serializable]
    [Table("thresholds")]
    public partial class TCthreshold:threshold
    {
        [Key]
        [Column(Order = 1)]
        public byte powerType { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public override string name { get; set; }

    }
}