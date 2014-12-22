namespace WHOperation.EF.WHO
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class dbWHOperation : DbContext
    {
        public dbWHOperation()
            : base("data source=142.2.70.81;initial catalog=dbWHOperation;user id=appuser;Password=information;MultipleActiveResultSets=True;App=EntityFramework")//"name=dbWHOperation")
        {
        }

        public virtual DbSet<PIMLVendorTemplate> PIMLVendorTemplate { get; set; }
        public virtual DbSet<PIMLVendorTemplateX> PIMLVendorTemplateX { get; set; }
        public virtual DbSet<PIMSMRBException> PIMSMRBException { get; set; }
        public virtual DbSet<sysMaster> sysMaster { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
        }
    }
}
