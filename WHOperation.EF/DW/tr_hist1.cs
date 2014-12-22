namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_hist1
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int tr_trnbr { get; set; }

        [StringLength(8)]
        public string tr_type { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? tr_date { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? tr_effdate { get; set; }

        [StringLength(8)]
        public string tr_site { get; set; }

        [StringLength(18)]
        public string tr_part { get; set; }

        [StringLength(18)]
        public string tr_serial { get; set; }

        [StringLength(8)]
        public string tr_loc { get; set; }

        [StringLength(8)]
        public string tr_ref { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? tr_qty_loc { get; set; }

        [StringLength(18)]
        public string tr_nbr { get; set; }

        public short? tr_line { get; set; }

        [StringLength(16)]
        public string tr_lot { get; set; }

        [StringLength(20)]
        public string tr_rmks { get; set; }

        [StringLength(16)]
        public string tr_program { get; set; }

        [StringLength(8)]
        public string tr_userid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string tr_comp { get; set; }
    }
}
