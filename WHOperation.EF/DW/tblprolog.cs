namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblprolog")]
    public partial class tblprolog
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string Site { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime pro_date { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string process { get; set; }

        public DateTime? start_time { get; set; }

        public DateTime? end_time { get; set; }
    }
}
