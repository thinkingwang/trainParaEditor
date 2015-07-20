using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Table("ProfileDetectResult")]
    public partial class ProfileDetectResult
    {
        [Key]
        [Column(Order = 0)]
        public DateTime testDateTime { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int axleNo { get; set; }

        [Key]
        [Column(Order = 2)]
        public byte wheelNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Lj { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TmMh { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LyHd { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LyGd { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LwHd { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LwHd2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? QR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Ncj { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LjCha_Zhou { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LjCha_ZXJ { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LjCha_Che { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LjCha_Bz { get; set; }

        [StringLength(50)]
        public string xmlFile { get; set; }

        public byte? Level_lj { get; set; }

        public byte? Level_tmmh { get; set; }

        public byte? Level_lyhd { get; set; }

        public byte? Level_lwhd { get; set; }

        public byte? Level_ncj { get; set; }

        public byte? Level_lygd { get; set; }

        public byte? Level_qr { get; set; }

        public byte? Level_LjCha_Zhou { get; set; }

        public byte? Level_LjCha_ZXJ { get; set; }

        public byte? Level_LjCha_Che { get; set; }

        public byte? Level_LjCha_Bz { get; set; }

        public virtual Detect Detect { get; set; }
    }
}
