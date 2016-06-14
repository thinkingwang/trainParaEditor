﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCModel
{
    [Serializable]
    [Table("EngineLib")]
    public partial class EngineLib
    {
        private static readonly NLog.Logger Nlogger = NLog.LogManager.GetLogger("Modifer");

        public EngineLib()
        {
            id = "";
        }

        [Key]
        [DisplayName(@"序列号"),ReadOnly(true)]
        public string id { get; set; }

        [DisplayName(@"名称")]
        public string name { get; set; }

        [Browsable(false)]
        public string fullName { get; set; }
        [Browsable(false)]
        public string factoryId { get; set; }
        [Browsable(false)]
        public Nullable<byte> typeId { get; set; }

        [DisplayName(@"轮径")]
        public Nullable<decimal> wheelSize { get; set; }

        [Browsable(false)]
        public Nullable<decimal> wheelSizeB { get; set; }

        [DisplayName(@"轴数")]
        public Nullable<byte> axleNum { get; set; }
    }
}
