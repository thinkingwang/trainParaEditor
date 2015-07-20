using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Table("Detect")]
    public partial class Detect
    {
        public Detect()
        {
            CarLists = new HashSet<CarList>();
            ProcDatas = new HashSet<ProcData>();
            ProfileDetectResults = new HashSet<ProfileDetectResult>();
            ProfileDetectResult_real = new HashSet<ProfileDetectResult_real>();
            Sequs = new HashSet<Sequ>();
        }

        [Key]
        public DateTime testDateTime { get; set; }

        [StringLength(50)]
        public string engNum { get; set; }

        [StringLength(50)]
        public string engBNum { get; set; }

        public bool engineDirection { get; set; }

        public short? bugNum { get; set; }

        public bool? isView { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? inSpeed { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? outSpeed { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? waterTemp { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? temperature { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? liquidTemp { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? arrayATemp { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? arrayBTemp { get; set; }

        public short? redWheelNum { get; set; }

        public short? yellowWheelNum { get; set; }

        public short? blueWheelNum { get; set; }

        public short? greenWheelNum { get; set; }

        public byte? isValid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? wheelSize { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? wheelSizeB { get; set; }

        public bool? IsTypical { get; set; }

        public short? AxleNum { get; set; }

        public short? scratchLevel3Num { get; set; }

        public short? scratchLevel2Num { get; set; }

        public short? scratchLevel1Num { get; set; }

        public short? scratchNum { get; set; }

        public short? M_LJ_Num { get; set; }

        public short? M_TmMh_Num { get; set; }

        public short? M_LyHd_Num { get; set; }

        public short? M_LwHd_Num { get; set; }

        public short? M_Ncj_Num { get; set; }

        public DateTime? outDateTime { get; set; }

        [StringLength(50)]
        public string procUser { get; set; }

        public short? M_LyGd_Num { get; set; }

        public short? M_Qr_Num { get; set; }

        public short? videoScratchNum { get; set; }

        public virtual ICollection<CarList> CarLists { get; set; }

        public virtual ICollection<ProcData> ProcDatas { get; set; }

        public virtual ICollection<ProfileDetectResult> ProfileDetectResults { get; set; }

        public virtual ICollection<ProfileDetectResult_real> ProfileDetectResult_real { get; set; }

        public virtual ICollection<Sequ> Sequs { get; set; }

        public virtual WhmsTime WhmsTime { get; set; }
    }
}
