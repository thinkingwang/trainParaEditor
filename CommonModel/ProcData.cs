using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Table("ProcData")]
    public partial class ProcData
    {
        [Key]
        [Column(Order = 0)]
        public DateTime CommitTime { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime testDateTime { get; set; }

        public byte NeedProc { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool? UseNewWheelSize { get; set; }

        [StringLength(50)]
        public string info { get; set; }

        public virtual Detect Detect { get; set; }
    }
}
