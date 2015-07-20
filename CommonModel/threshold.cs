using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    public partial class threshold
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string trainType { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? standard { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? up_level3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? up_level2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? up_level1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? low_level3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? low_level2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? low_level1 { get; set; }

        [StringLength(100)]
        public string desc { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? precision { get; set; }
    }
}
