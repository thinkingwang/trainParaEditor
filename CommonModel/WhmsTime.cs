using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Table("WhmsTime")]
    public partial class WhmsTime
    {
        [Key]
        public DateTime tychoTime { get; set; }

        [Column("whmsTime")]
        public DateTime whmsTime1 { get; set; }

        public virtual Detect Detect { get; set; }
    }
}
