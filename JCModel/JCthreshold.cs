using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonModel;

namespace JCModel
{
    [Serializable]
    [Table("thresholds")]
    public partial class JCthreshold : threshold
    {

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public override string name { get; set; }

    }
}