namespace CommonModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ServerContext : DbContext
    {
        public ServerContext(string str)
            : base(str)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<IPAddress> IPAddresses { get; set; }
        public virtual DbSet<kfList> kfLists { get; set; }
        public virtual DbSet<opLog> opLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IPAddress>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<IPAddress>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<kfList>()
                .Property(e => e.remoteIP)
                .IsUnicode(false);

            modelBuilder.Entity<kfList>()
                .Property(e => e._operator)
                .IsUnicode(false);

            modelBuilder.Entity<kfList>()
                .Property(e => e.desc)
                .IsUnicode(false);

            modelBuilder.Entity<opLog>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<opLog>()
                .Property(e => e.testdatetime)
                .IsUnicode(false);

            modelBuilder.Entity<opLog>()
                .Property(e => e._operator)
                .IsUnicode(false);

            modelBuilder.Entity<opLog>()
                .Property(e => e.desc)
                .IsUnicode(false);
        }
    }
}
