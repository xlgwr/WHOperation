namespace WHOperation.EF.WHO
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class dbWHOperation : DbContext
    {
        public dbWHOperation()
            : base("data source=142.2.70.81;initial catalog=dbWHOperation;user id=appuser;Password=application;MultipleActiveResultSets=True;App=EntityFramework")//"name=dbWHOperation")
        {
        }
        public virtual DbSet<PI_Print> PI_Print { get; set; }
        public virtual DbSet<PIMLVendorTemplate> PIMLVendorTemplate { get; set; }
        public virtual DbSet<PIMLVendorTemplateX> PIMLVendorTemplateX { get; set; }
        public virtual DbSet<PIMSMRBException> PIMSMRBException { get; set; }
        public virtual DbSet<sysMaster> sysMaster { get; set; }
        public virtual DbSet<vpi_sumPrintQty> vpi_sumPrintQty { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PI_Print>()
                .Property(e => e.PI_mpq)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PI_Print>()
                .Property(e => e.PI_QTY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PI_Print>()
                .Property(e => e.PI_Print_QTY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PI_Print>()
                .Property(e => e.PI_PO_price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PI_Print>()
                .Property(e => e.pi_num1)
                .HasPrecision(18, 7);

            modelBuilder.Entity<PI_Print>()
                .Property(e => e.pi_num2)
                .HasPrecision(18, 7);

            modelBuilder.Entity<PIMLVendorTemplate>()
                .Property(e => e.isDefault)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PIMLVendorTemplateX>()
                .Property(e => e.isDefault)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PIMSMRBException>()
                .Property(e => e.TransID)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PIMSMRBException>()
                .Property(e => e.RecQty)
                .HasPrecision(18, 5);

            modelBuilder.Entity<vpi_sumPrintQty>()
                .Property(e => e.sumPrintQty)
                .HasPrecision(38, 0);
        }
    }
}
