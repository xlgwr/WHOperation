namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class qpl_mstr
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(34)]
        public string qpl_key { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(18)]
        public string qpl_part { get; set; }

        [StringLength(8)]
        public string qpl_site { get; set; }

        [StringLength(8)]
        public string qpl_mfgr { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string qpl_mfgr_part { get; set; }

        [StringLength(1)]
        public string qpl_appr_st { get; set; }

        [StringLength(40)]
        public string qpl_rmk { get; set; }

        [StringLength(3)]
        public string qpl_rec_type { get; set; }

        [StringLength(40)]
        public string qpl_cust_part { get; set; }
    }
}
