using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Table("ProfileAdjust")]
    public partial class ProfileAdjust
    {
        [Key]
        public byte position { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Lj { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LyHd { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LyGd { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LwHd { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LwHd2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal QR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Ncj { get; set; }
    }
}
