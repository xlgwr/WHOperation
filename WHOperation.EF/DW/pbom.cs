namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pbom")]
    public partial class pbom
    {
        [StringLength(18)]
        public string Model { get; set; }

        [StringLength(18)]
        public string Comp { get; set; }

        public decimal? Usage { get; set; }

        [Key]
        [Column("Column 3")]
        [StringLength(50)]
        public string Column_3 { get; set; }
    }
}
