namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_receiving3
    {
        [StringLength(10)]
        public string prh_rcp_date { get; set; }

        [StringLength(8)]
        public string prh_nbr { get; set; }

        [Key]
        [Column(Order = 0, TypeName = "date")]
        public DateTime DateKey { get; set; }

        public int? CalYear { get; set; }

        public int? CalMonth { get; set; }

        public int? CalDayofWeek { get; set; }

        [StringLength(8)]
        public string si__do { get; set; }

        [StringLength(8)]
        public string si__cac { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string si_site { get; set; }

        [StringLength(2)]
        public string pt_part_type { get; set; }

        [StringLength(8)]
        public string pt_conn { get; set; }

        public short? pt_seq { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pt_netwt { get; set; }

        [StringLength(24)]
        public string pt_desc1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HKAmt { get; set; }

        [StringLength(18)]
        public string prh_part { get; set; }

        [StringLength(8)]
        public string po_vend { get; set; }

        [StringLength(10)]
        public string po_ord_date { get; set; }

        [StringLength(8)]
        public string po_ship { get; set; }

        [StringLength(8)]
        public string po_cr_terms { get; set; }

        [StringLength(8)]
        public string po_buyer { get; set; }

        [StringLength(20)]
        public string po_shipvia { get; set; }

        [StringLength(20)]
        public string po_fob { get; set; }

        [StringLength(3)]
        public string po_curr { get; set; }

        [StringLength(1)]
        public string po_type { get; set; }
    }
}
