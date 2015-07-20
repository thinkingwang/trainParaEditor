using System.ComponentModel.DataAnnotations.Schema;
using CommonModel;

namespace JCModel
{
    [Table("Sequ")]
    public partial class JCCarList : CarList
    {
        public bool direction { get; set; }
    }
}