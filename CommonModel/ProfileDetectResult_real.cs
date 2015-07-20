using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    public partial class ProfileDetectResult_real
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

        public byte? pos { get; set; }

        public byte? carPos { get; set; }

        [StringLength(20)]
        public string carNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Lj_user { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Lj_AVG { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LwHd2_AVG { get; set; }

        public virtual Detect Detect { get; set; }
    }
}
