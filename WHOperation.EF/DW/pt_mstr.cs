namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pt_mstr
    {
        [Key]
        [StringLength(18)]
        public string pt_part { get; set; }

        [StringLength(24)]
        public string pt_desc1 { get; set; }

        [StringLength(24)]
        public string pt_desc2 { get; set; }

        [StringLength(2)]
        public string pt_part_type { get; set; }

        [StringLength(2)]
        public string pt_um { get; set; }

        [StringLength(8)]
        public string pt_conn { get; set; }

        public int? pt__seq { get; set; }

        public decimal? pt_net_wt { get; set; }
    }
}
