namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ftn_mst
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string ftn_comp { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(18)]
        public string ftn_nbr { get; set; }

        [StringLength(8)]
        public string ftn_site { get; set; }

        [StringLength(18)]
        public string ftn_part { get; set; }

        public int? ftn_qty { get; set; }

        [StringLength(18)]
        public string ftn_wo { get; set; }

        [StringLength(18)]
        public string ftn_lot { get; set; }

        [StringLength(18)]
        public string ftn_lot5 { get; set; }

        public int? ftn_CTN { get; set; }

        public DateTime? ftn_cre_date { get; set; }

        public DateTime? ftn_con_date { get; set; }

        public DateTime? ftn_apr_date { get; set; }

        public DateTime? ftn_pas_date { get; set; }

        public DateTime? ftn_rej_date { get; set; }

        [StringLength(8)]
        public string ftn_cre_user { get; set; }

        [StringLength(8)]
        public string ftn_con_user { get; set; }

        [StringLength(8)]
        public string ftn_apr_user { get; set; }

        [StringLength(8)]
        public string ftn_pas_user { get; set; }

        [StringLength(8)]
        public string ftn_rej_user { get; set; }

        [StringLength(8)]
        public string ftn_loc { get; set; }

        [StringLength(30)]
        public string ftn_po { get; set; }

        [StringLength(8)]
        public string ftn_status { get; set; }
    }
}
