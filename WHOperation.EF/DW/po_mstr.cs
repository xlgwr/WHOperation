namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class po_mstr
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string po_comp { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string po_nbr { get; set; }

        [StringLength(8)]
        public string po_vend { get; set; }

        [StringLength(8)]
        public string po_ship { get; set; }

        [Column(TypeName = "date")]
        public DateTime? po_ord_date { get; set; }

        [StringLength(40)]
        public string po_rmks { get; set; }

        [StringLength(8)]
        public string po_cr_terms { get; set; }

        [StringLength(8)]
        public string po_buyer { get; set; }

        [StringLength(20)]
        public string po_shipvia { get; set; }

        [StringLength(20)]
        public string po_fob { get; set; }

        [StringLength(8)]
        public string po_bill { get; set; }

        [StringLength(3)]
        public string po_confirm { get; set; }

        [StringLength(3)]
        public string po_curr { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? po_ex_rate { get; set; }

        [StringLength(8)]
        public string po_site { get; set; }

        [Column(TypeName = "date")]
        public DateTime? po_due_date { get; set; }

        [StringLength(1)]
        public string po_type { get; set; }

        [StringLength(8)]
        public string po_user_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? po_ent_ex { get; set; }

        [StringLength(8)]
        public string po__chr07 { get; set; }

        [StringLength(8)]
        public string po__chr08 { get; set; }

        [StringLength(8)]
        public string po__chr09 { get; set; }

        [StringLength(8)]
        public string po__chr10 { get; set; }
    }
}
