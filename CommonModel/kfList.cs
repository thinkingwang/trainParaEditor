namespace CommonModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("kfList")]
    public partial class kfList
    {
        [Key]
        [Column(Order = 0)]
        public DateTime getTime { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string remoteIP { get; set; }

        public DateTime testdatetime { get; set; }

        public DateTime? acceptTime { get; set; }

        [Column("operator")]
        [StringLength(20)]
        public string _operator { get; set; }

        [StringLength(200)]
        public string desc { get; set; }
    }
}
