namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_receiving_2
    {
        [Key]
        [StringLength(8)]
        public string si_site { get; set; }

        [StringLength(8)]
        public string si__do { get; set; }

        [StringLength(8)]
        public string si__cac { get; set; }

        [StringLength(10)]
        public string prh_rcp_date { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HKAmt { get; set; }

        [StringLength(18)]
        public string prh_part { get; set; }
    }
}
