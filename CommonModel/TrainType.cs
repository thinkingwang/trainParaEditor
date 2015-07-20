using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Table("TrainType")]
    public partial class TrainType
    {
        [Column("trainType")]
        [Required]
        [StringLength(20)]
        public string trainType1 { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int trainNoFrom { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int trainNoTo { get; set; }

        [Required]
        [StringLength(20)]
        public string format { get; set; }
    }
}
