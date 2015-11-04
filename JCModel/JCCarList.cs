using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonModel;

namespace JCModel
{
    [Table("CarList")]
    public  class JCCarList :CarList
    {
        [DisplayName("方向")]
        [NotMapped]
        public bool direction { get; set; }
    }
}