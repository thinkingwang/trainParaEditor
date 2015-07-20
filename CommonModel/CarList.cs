using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Table("CarList")]
    public partial class CarList
    {
        [Key]
        [Column(Order = 0)]
        public DateTime testDateTime { get; set; }

        [Key]
        [Column(Order = 1)]
        public byte posNo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string carNo { get; set; }

        [StringLength(20)]
        public string carNo2 { get; set; }

        public virtual Detect Detect { get; set; }
    }
}
