namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class fact_invbal
    {
        public DateTime? dw_date { get; set; }

        [StringLength(8)]
        public string dw_site { get; set; }

        [StringLength(18)]
        public string dw_part { get; set; }

        public short? dw_seq { get; set; }

        [StringLength(8)]
        public string dw_type { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dw_qty { get; set; }

        [StringLength(18)]
        public string dw_model { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dw_usage { get; set; }

        [StringLength(18)]
        public string dw_lot { get; set; }

        [StringLength(8)]
        public string dw_loc { get; set; }

        [Key]
        public int dw_id { get; set; }
    }
}
