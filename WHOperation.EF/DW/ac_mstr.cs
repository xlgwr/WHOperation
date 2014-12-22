namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ac_mstr
    {
        [Key]
        [StringLength(8)]
        public string ac_code { get; set; }

        [StringLength(40)]
        public string ac_desc { get; set; }

        [StringLength(40)]
        public string ac_desc2 { get; set; }

        [StringLength(8)]
        public string ac_curr { get; set; }

        [StringLength(8)]
        public string ac_type { get; set; }

        [StringLength(8)]
        public string ac_active { get; set; }
    }
}
