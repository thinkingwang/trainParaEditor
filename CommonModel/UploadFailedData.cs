namespace CommonModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UploadFailedData")]
    public partial class UploadFailedData
    {
        [Key]
        public DateTime testDateTime { get; set; }

        public short? errorCode { get; set; }
    }
}
