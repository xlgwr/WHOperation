namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_receiving
    {
        [StringLength(8)]
        public string prh_nbr { get; set; }

        public short? prh_line { get; set; }

        [StringLength(8)]
        public string prh_receiver { get; set; }

        [StringLength(10)]
        public string prh_rcp_date { get; set; }

        [StringLength(8)]
        public string prh_site { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int si_id { get; set; }

        [StringLength(18)]
        public string prh_part { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HKAmt { get; set; }
    }
}
