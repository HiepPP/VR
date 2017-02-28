namespace VR.DAL.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class VRContext : DbContext
    {
        public VRContext()
            : base("name=VRContext")
        {
        }

        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<ExcelDetail> ExcelDetails { get; set; }
        public virtual DbSet<FileExcel> FileExcels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Configuration>()
                .Property(e => e.TruckType)
                .IsUnicode(false);

            modelBuilder.Entity<Configuration>()
                .Property(e => e.NetWeight)
                .IsUnicode(false);

            modelBuilder.Entity<Configuration>()
                .Property(e => e.MaxSeat)
                .IsUnicode(false);

            modelBuilder.Entity<Configuration>()
                .Property(e => e.DateCheck)
                .IsUnicode(false);

            modelBuilder.Entity<Configuration>()
                .Property(e => e.GCNStamp)
                .IsUnicode(false);

            modelBuilder.Entity<Configuration>()
                .Property(e => e.WeightLimit)
                .IsUnicode(false);

            modelBuilder.Entity<Configuration>()
                .Property(e => e.GrossWeight)
                .IsUnicode(false);

            modelBuilder.Entity<Configuration>()
                .Property(e => e.TowedWeight)
                .IsUnicode(false);

            modelBuilder.Entity<Configuration>()
                .Property(e => e.GCNDate)
                .IsUnicode(false);

            modelBuilder.Entity<Configuration>()
                .Property(e => e.AxleNo)
                .IsUnicode(false);
         

            modelBuilder.Entity<ExcelDetail>()
                .Property(e => e.TruckNo)
                .IsUnicode(false);

            modelBuilder.Entity<ExcelDetail>()
                .Property(e => e.ChassisNo)
                .IsUnicode(false);

            modelBuilder.Entity<ExcelDetail>()
                .Property(e => e.DriverId)
                .IsUnicode(false);

            modelBuilder.Entity<ExcelDetail>()
                .Property(e => e.LicenseNo)
                .IsUnicode(false);

          
        }
    }
}
