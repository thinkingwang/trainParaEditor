using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using CommonModel;

namespace JCModel
{
    [Table("CarList")]
    public partial class JCCarList : CarList
    {
        [DisplayName("方向")]
        public bool direction { get; set; }
    }
}