using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    public partial class CRH_wheel
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string trainType { get; set; }

        [Key]
        [Column(Order = 1)]
        public byte axleNo { get; set; }

        [Key]
        [Column(Order = 2)]
        public byte wheelNo { get; set; }

        public byte axlePos { get; set; }

        public byte wheelPos { get; set; }
    }
}
