namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ad_mstr
    {
        [Key]
        [StringLength(8)]
        public string ad_addr { get; set; }

        [StringLength(28)]
        public string ad_name { get; set; }

        [StringLength(28)]
        public string ad_line1 { get; set; }

        [StringLength(28)]
        public string ad_line2 { get; set; }

        [StringLength(28)]
        public string ad_line3 { get; set; }

        [StringLength(20)]
        public string ad_city { get; set; }

        [StringLength(4)]
        public string ad_state { get; set; }

        [StringLength(10)]
        public string ad_zip { get; set; }

        [StringLength(8)]
        public string ad_type { get; set; }

        [StringLength(24)]
        public string ad_attn { get; set; }

        [StringLength(16)]
        public string ad_phone { get; set; }

        [StringLength(4)]
        public string ad_ext { get; set; }

        [StringLength(8)]
        public string ad_ref { get; set; }

        [StringLength(28)]
        public string ad_sort { get; set; }

        [StringLength(28)]
        public string ad_country { get; set; }

        [StringLength(8)]
        public string ad_user1 { get; set; }

        [StringLength(8)]
        public string ad_user2 { get; set; }

        [StringLength(8)]
        public string ad_userid { get; set; }
    }
}
