namespace WHOperation.EF.PI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PI_DET
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(12)]
        public string PI_NO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PI_LINE { get; set; }

        [StringLength(12)]
        public string PI_CARTON_NO { get; set; }

        [StringLength(6)]
        public string PI_SITE { get; set; }

        [StringLength(18)]
        public string PI_PART { get; set; }

        [StringLength(10)]
        public string PI_CO { get; set; }

        [StringLength(50)]
        public string PI_DESC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PI_QTY { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(12)]
        public string PI_LOT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PI_NW { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PI_K200_NW { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PI_GW { get; set; }

        [StringLength(4)]
        public string PI_SBU { get; set; }

        [StringLength(15)]
        public string PI_REC_NO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PI_PRICE { get; set; }

        [StringLength(12)]
        public string PI_PALLET { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PI_PO_price { get; set; }

        [StringLength(18)]
        public string PI_CONTRACT { get; set; }

        public int? PI_SEQ { get; set; }

        public int? PI_SEQ_CL { get; set; }

        [StringLength(4)]
        public string PI_IQC { get; set; }

        [StringLength(8)]
        public string PI_PO { get; set; }

        [StringLength(16)]
        public string PI_Taxcode { get; set; }

        [StringLength(8)]
        public string PI_ConnCode { get; set; }

        [StringLength(8)]
        public string pi_user { get; set; }

        public DateTime? pi_cre_time { get; set; }

        public int? pi_ver { get; set; }

        [StringLength(8)]
        public string pi_mfgr { get; set; }

        [StringLength(100)]
        public string pi_mfgr_part { get; set; }

        [StringLength(100)]
        public string pi_mfgr_name { get; set; }

        [StringLength(3)]
        public string pi_Lic_req { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pi_ori_PO_price { get; set; }

        [StringLength(8)]
        public string pi_PO_curr { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pi_curr_rate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pi_us_rate { get; set; }

        [StringLength(50)]
        public string pi_vend { get; set; }

        public decimal? PI_Print_QTY { get; set; }
    }
}
