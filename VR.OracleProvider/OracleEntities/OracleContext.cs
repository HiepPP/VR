namespace VR.OracleProvider.OracleEntities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OracleContext : DbContext
    {
        public OracleContext()
            : base("name=OracleContext")
        {
        }

        public virtual DbSet<APPUSER> APPUSERs { get; set; } 
        public virtual DbSet<BILL> BILLs { get; set; }
        public virtual DbSet<PARTNER> PARTNERs { get; set; }
        public virtual DbSet<PARTNER_TRUCK> PARTNER_TRUCK { get; set; }
        public virtual DbSet<TRUCK> TRUCKs { get; set; }
        public virtual DbSet<VOYAGE> VOYAGEs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("SYSTEM");
            modelBuilder.Entity<BILL>()
                .Property(e => e.BILL_NO)
                .IsUnicode(false);

            modelBuilder.Entity<BILL>()
                .Property(e => e.REMARK)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.ADDRESS)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.TEL)
                .IsUnicode(false);

            //modelBuilder.Entity<PARTNER>()
            //    .Property(e => e.FAX)
            //    .IsUnicode(false);
            //modelBuilder.Entity<PARTNER>()


        

            modelBuilder.Entity<PARTNER_TRUCK>()
                .Property(e => e.REMARK)
                .IsUnicode(false);

            modelBuilder.Entity<TRUCK>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<TRUCK>()
                .Property(e => e.TRK_NO)
                .IsUnicode(false);

            modelBuilder.Entity<TRUCK>()
                .Property(e => e.LICENSE_NO)
                .IsUnicode(false);

            modelBuilder.Entity<TRUCK>()
                .Property(e => e.DRIVER_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<TRUCK>()
                .Property(e => e.DRIVER_ID)
                .IsUnicode(false);

            modelBuilder.Entity<TRUCK>()
                .Property(e => e.REMOOC_NO)
                .IsUnicode(false);

            modelBuilder.Entity<TRUCK>()
                .Property(e => e.TRK_TYPE);
                //.IsFixedLength()
                //.IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.BILL_NO)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.CODE)
                .IsUnicode(false);

        }
    }
}
