using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using CommonModel;

namespace JCModel
{
    [Table("Sequ")]
    public partial class JCSequ : Sequ
    {
        public byte? status5 { get; set; }

        public byte? status6 { get; set; }
    }
}
