namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class wst055f2
    {
        [Key]
        public long wst055f2_id { get; set; }

        [StringLength(8)]
        public string wst055f2_comp { get; set; }

        public int? wst055f2_seq { get; set; }

        public DateTime? wst055f2_date { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? wst055f2_mfg_bal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? wst055f2_edi_bal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? wst055f2_tr_rmopen { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? wst055f2_tr_rmrecv { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? wst055f2_tr_rmtr { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? wst055f2_tr_fgopen { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? wst055f2_tr_fgship { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? wst055f2_tr_fgtr { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? wst055f2_edi_abal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? wst055f2_var_qty { get; set; }
    }
}
