namespace CommonModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IPAddress")]
    public partial class IPAddress
    {
        [Key]
        [StringLength(15)]
        public string IP { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public byte Type { get; set; }

        public bool Status { get; set; }
    }
}
