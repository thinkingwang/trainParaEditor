using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Serializable]
    [Table("TrainType")]
    public partial class TrainType
    {
        [DisplayName(@"车型")]
        [Column("trainType")]
        [Required]
        [StringLength(20)]
        public string trainType { get; set; }

        [DisplayName(@"车号开始数字")]
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int trainNoFrom { get; set; }

        [DisplayName(@"车号结束数字")]
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int trainNoTo { get; set; }

        [DisplayName(@"车型显示格式")]
        [Required]
        [StringLength(20)]
        public string format { get; set; }
    }
}
