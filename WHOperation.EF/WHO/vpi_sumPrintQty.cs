namespace WHOperation.EF.WHO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vpi_sumPrintQty
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(12)]
        public string PI_NO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(18)]
        public string PI_PART { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string pi_mfgr_part { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(12)]
        public string PI_LOT { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string PI_PO { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string pi_mfgr { get; set; }

        [Key]
        [Column(Order = 6)]
        public decimal PI_QTY { get; set; }

        public decimal? sumPrintQty { get; set; }
    }
}
