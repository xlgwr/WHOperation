namespace WHOperation.EF.WHO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PI_Print
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PI_Line { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(12)]
        public string PI_NO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(18)]
        public string PI_PART { get; set; }

        public decimal PI_mpq { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string pi_mfgr_part { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(12)]
        public string PI_LOT { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string PI_PO { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string pi_mfgr { get; set; }

        public decimal PI_QTY { get; set; }

        public decimal PI_Print_QTY { get; set; }

        [StringLength(6)]
        public string PI_SITE { get; set; }

        public decimal? PI_PO_price { get; set; }

        [StringLength(50)]
        public string pi_char1 { get; set; }

        [StringLength(50)]
        public string pi_char2 { get; set; }

        [StringLength(50)]
        public string pi_char3 { get; set; }

        public decimal? pi_num1 { get; set; }

        public decimal? pi_num2 { get; set; }

        public int? pi_int1 { get; set; }

        public int? pi_int2 { get; set; }

        public DateTime? pi_cre_date { get; set; }

        [StringLength(50)]
        public string pi_cre_userid { get; set; }

        public DateTime? pi_update_date { get; set; }

        [StringLength(50)]
        public string pi_edituser_id { get; set; }

        [StringLength(50)]
        public string pi_user_ip { get; set; }
    }
}
