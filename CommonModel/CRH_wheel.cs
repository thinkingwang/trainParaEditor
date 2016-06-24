using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Serializable]
    public partial class CRH_wheel
    {
        private static readonly NLog.Logger Nlogger = NLog.LogManager.GetLogger("Modifer");
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        [ReadOnly(true), DisplayName(@"≥µ–Õ")]
        public string trainType { get; set; }

        [Key]
        [Column(Order = 1)]
        [ReadOnly(true), DisplayName(@"÷·–Ú∫≈")]
        public byte axleNo { get; set; }

        [Key]
        [Browsable(false)]
        [Column(Order = 2)]
        public byte wheelNo { get; set; }
        
        [NotMapped]
        [ReadOnly(true), DisplayName(@"¬÷Œª÷√")]
        public string wheelNoStr {
            get { return wheelNo == 0 ? "◊Û" : "”“"; } }

        [DisplayName(@"÷·Œª")]
        public byte axlePos { get; set; }

        [DisplayName(@"¬÷Œª")]
        public byte wheelPos { get; set; }
    }
}
