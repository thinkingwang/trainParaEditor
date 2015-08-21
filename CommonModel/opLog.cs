namespace CommonModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("opLog")]
    public partial class opLog
    {
        [Key]
        public DateTime opTime { get; set; }

        [StringLength(15)]
        public string IP { get; set; }

        [StringLength(19)]
        public string testdatetime { get; set; }

        public short? axleNo { get; set; }

        public byte? wheelNo { get; set; }

        [Column("operator")]
        [StringLength(20)]
        public string _operator { get; set; }

        [StringLength(200)]
        public string desc { get; set; }
    }
}
