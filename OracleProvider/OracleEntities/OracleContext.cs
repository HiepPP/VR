namespace OracleProvider.OracleEntities
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

        public virtual DbSet<BILL> BILLs { get; set; }
        public virtual DbSet<PARTNER> PARTNERs { get; set; }
        public virtual DbSet<PARTNER_TRUCK> PARTNER_TRUCK { get; set; }
        public virtual DbSet<TRUCK> TRUCKs { get; set; }
        public virtual DbSet<VOYAGE> VOYAGEs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BILL>()
                .Property(e => e.BILL_NO)
                .IsUnicode(false);

            modelBuilder.Entity<BILL>()
                .Property(e => e.REMARK)
                .IsUnicode(false);

            modelBuilder.Entity<BILL>()
                .Property(e => e.CREATED_BY)
                .IsUnicode(false);

            modelBuilder.Entity<BILL>()
                .Property(e => e.STATUS)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.ADDRESS)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.TAXCODE)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.TEL)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.FAX)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.CREATED_BY)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.STATUS)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.TRK_LIST_IMP)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER>()
                .Property(e => e.IS_TRK_IMP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER_TRUCK>()
                .Property(e => e.REMARK)
                .IsUnicode(false);

            modelBuilder.Entity<PARTNER_TRUCK>()
                .Property(e => e.CREATED_BY)
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
                .Property(e => e.CREATED_BY)
                .IsUnicode(false);

            modelBuilder.Entity<TRUCK>()
                .Property(e => e.REMOOC_NO)
                .IsUnicode(false);

            modelBuilder.Entity<TRUCK>()
                .Property(e => e.TRK_TYPE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TRUCK>()
                .Property(e => e.CERT_NO)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.CALL_SEQ)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.DEC_NO)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.BILL_NO)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.CREATED_BY)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.SERVICELANE)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.BAR_VSL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.GRT)
                .HasPrecision(12, 4);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.WHARFMARK)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.DRAFT_ARRIVE)
                .HasPrecision(4, 2);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.DRAFT_AFTARRIVE)
                .HasPrecision(4, 2);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.DRAFT_DEPART)
                .HasPrecision(4, 2);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.DRAFT_AFTDEPART)
                .HasPrecision(4, 2);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.EQUIPMENT)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.AGENT)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.CONTACT)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.NOTES)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.START_OPER_TIME)
                .HasPrecision(6);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.END_OPER_TIME)
                .HasPrecision(6);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.CAPTAIN)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.DWT)
                .HasPrecision(10, 2);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.VSL_MOD)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.USE_WB)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.IS_CONTAINER)
                .IsUnicode(false);

            modelBuilder.Entity<VOYAGE>()
                .Property(e => e.TML_HOLD)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
