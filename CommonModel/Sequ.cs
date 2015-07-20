using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Table("Sequ")]
    public partial class Sequ
    {
        [Key]
        [Column(Order = 0)]
        public DateTime testDateTime { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short axleNo { get; set; }

        [Key]
        [Column(Order = 2)]
        public byte wheelNo { get; set; }

        public byte? status1 { get; set; }

        public byte? status2 { get; set; }

        public byte? status3 { get; set; }

        public byte? status4 { get; set; }


        public virtual Detect Detect { get; set; }
    }
}
