namespace WHOperation.EF.WHO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pi_Det_Remote
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long pi_Line { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(12)]
        public string pi_NO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(18)]
        public string pi_PART { get; set; }

        [Key]
        [Column(Order = 3)]
        public string pi_mfgr_part { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(12)]
        public string pi_LOT { get; set; }

        [Required]
        [StringLength(12)]
        public string pi_WecNumber { get; set; }

        [Key]
        [Column(Order = 5)]
        public string pi_PO { get; set; }

        [Key]
        [Column(Order = 6)]
        public string pi_mfgr { get; set; }

        public decimal pi_QTY { get; set; }

        public decimal pi_Print_QTY { get; set; }

        public decimal pi_ttlQTY { get; set; }

        public decimal pi_mpq { get; set; }

        [StringLength(6)]
        public string pi_SITE { get; set; }

        [StringLength(128)]
        public string pi_PALLET { get; set; }

        [StringLength(128)]
        public string pi_CARTON_NO { get; set; }

        [StringLength(50)]
        public string pi_carton_prefix { get; set; }

        public int? pi_carton_from { get; set; }

        public int? pi_carton_to { get; set; }

        [StringLength(128)]
        public string pi_DateCode { get; set; }

        [StringLength(128)]
        public string pi_LotNumber { get; set; }

        [StringLength(128)]
        public string pi_char1 { get; set; }

        [StringLength(128)]
        public string pi_char2 { get; set; }

        [StringLength(128)]
        public string pi_char3 { get; set; }

        public decimal? pi_num1 { get; set; }

        public decimal? pi_num2 { get; set; }

        public int? pi_int1 { get; set; }

        public int? pi_int2 { get; set; }

        public DateTime? pi_cre_date { get; set; }

        [StringLength(128)]
        public string pi_cre_userid { get; set; }

        public DateTime? pi_update_date { get; set; }

        [StringLength(128)]
        public string pi_edituser_id { get; set; }

        [StringLength(128)]
        public string pi_user_ip { get; set; }

        [StringLength(256)]
        public string pi_remark { get; set; }
    }
}
