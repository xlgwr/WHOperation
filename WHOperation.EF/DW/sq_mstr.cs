namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sq_mstr
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string sq_type { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short sq_seq { get; set; }

        [StringLength(30)]
        public string sq_name { get; set; }

        [StringLength(8)]
        public string sq_unit { get; set; }

        [StringLength(10)]
        public string sq_unit_id { get; set; }

        [StringLength(30)]
        public string sq_spec { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? sq_price { get; set; }
    }
}
