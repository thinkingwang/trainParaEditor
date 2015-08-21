namespace CommonModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataCenterContext : DbContext
    {
        public DataCenterContext(string str)
            : base(str)
        {
        }

        public virtual DbSet<UploadFailedData> UploadFailedDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
