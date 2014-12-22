namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class si_mstr
    {
        [Required]
        [StringLength(8)]
        public string si_site { get; set; }

        [StringLength(30)]
        public string si_desc { get; set; }

        [StringLength(4)]
        public string si_entity { get; set; }

        [StringLength(8)]
        public string si_last_fcid { get; set; }

        [StringLength(8)]
        public string si_comp { get; set; }

        [StringLength(8)]
        public string si__cac { get; set; }

        [StringLength(8)]
        public string si__fcid_cs { get; set; }

        [StringLength(8)]
        public string si__do { get; set; }

        [StringLength(8)]
        public string si__no_fc { get; set; }

        [Column(TypeName = "date")]
        public DateTime? si__fc_sent { get; set; }

        [StringLength(3)]
        public string si__show_cust_pn { get; set; }

        [Key]
        public int si_id { get; set; }

        public DateTime si_cre_date { get; set; }
    }
}
