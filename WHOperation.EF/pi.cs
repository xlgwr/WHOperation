namespace WHOperation.EF.PI
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PI : DbContext
    {
        public PI()
            : base("data source=142.2.70.53;initial catalog=pi_hk;user id=pi;Password=;MultipleActiveResultSets=True;App=EntityFramework")
        {
        }

        public virtual DbSet<PI_DET> PI_DET { get; set; }
        public virtual DbSet<vpi_detWHO> vpi_detWHO { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PI_DET>()
                .Property(e => e.PI_QTY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PI_DET>()
                .Property(e => e.PI_NW)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PI_DET>()
                .Property(e => e.PI_K200_NW)
                .HasPrecision(18, 7);

            modelBuilder.Entity<PI_DET>()
                .Property(e => e.PI_GW)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PI_DET>()
                .Property(e => e.PI_PRICE)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PI_DET>()
                .Property(e => e.PI_PO_price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PI_DET>()
                .Property(e => e.pi_ori_PO_price)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PI_DET>()
                .Property(e => e.pi_curr_rate)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PI_DET>()
                .Property(e => e.pi_us_rate)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PI_DET>()
                .Property(e => e.PI_Print_QTY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<vpi_detWHO>()
                .Property(e => e.PI_QTY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<vpi_detWHO>()
                .Property(e => e.PI_Print_QTY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<vpi_detWHO>()
                .Property(e => e.PI_PO_price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<vpi_detWHO>()
                .Property(e => e.ttlQTY)
                .HasPrecision(38, 0);
        }
    }
}
