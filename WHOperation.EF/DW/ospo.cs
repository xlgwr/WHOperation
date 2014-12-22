namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ospo")]
    public partial class ospo
    {
        [StringLength(8)]
        public string Company { get; set; }

        [StringLength(8)]
        public string Buyer { get; set; }

        [StringLength(8)]
        public string Supplier { get; set; }

        [StringLength(12)]
        public string Order { get; set; }

        public short? Rev { get; set; }

        public short? Ln { get; set; }

        [StringLength(20)]
        public string PO_FOB { get; set; }

        [StringLength(3)]
        public string MPO { get; set; }

        [StringLength(20)]
        public string Buyer_Name { get; set; }

        [StringLength(8)]
        public string PO_level1 { get; set; }

        [StringLength(8)]
        public string PO_level2 { get; set; }

        [StringLength(8)]
        public string PO_level3 { get; set; }

        public DateTime? Cre_date { get; set; }

        public DateTime? Fax_date { get; set; }

        [StringLength(8)]
        public string DO { get; set; }

        [StringLength(8)]
        public string Site { get; set; }

        [StringLength(18)]
        public string Item_no { get; set; }

        [StringLength(1)]
        public string ABC { get; set; }

        [StringLength(24)]
        public string Desc1 { get; set; }

        [StringLength(24)]
        public string Desc2 { get; set; }

        public short? Pur_LT { get; set; }

        [StringLength(8)]
        public string MFGR { get; set; }

        [StringLength(30)]
        public string MFGR_Name { get; set; }

        [StringLength(30)]
        public string PO_QPL_PART { get; set; }

        [StringLength(30)]
        public string CUST_PART { get; set; }

        [StringLength(15)]
        public string VMI { get; set; }

        [Column("WO-ID")]
        [StringLength(16)]
        public string WO_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty_Order { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Open_Qty { get; set; }

        public DateTime? ETD_WEC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? QtyInTransit { get; set; }

        [StringLength(1)]
        public string SDC { get; set; }

        public DateTime? Orig_PRNeed { get; set; }

        public DateTime? LatestReqDate { get; set; }

        public DateTime? Lastest_ACO_Sent { get; set; }

        [Column("1st_SDC")]
        public DateTime? C1st_SDC { get; set; }

        public DateTime? Last_SDC { get; set; }

        public DateTime? Curr_SDC { get; set; }

        [StringLength(8)]
        public string WELLOP_PR { get; set; }

        [StringLength(8)]
        public string Message_For_Alert { get; set; }

        public float? DayFailed { get; set; }

        [StringLength(8)]
        public string ROHS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Proj_QOH { get; set; }

        [StringLength(8)]
        public string MRP_OAR { get; set; }

        [StringLength(40)]
        public string De_expdeite { get; set; }

        [StringLength(40)]
        public string Expedite { get; set; }

        [StringLength(40)]
        public string Cancel { get; set; }

        [StringLength(40)]
        public string UnChange { get; set; }

        [StringLength(250)]
        public string ACO_Hist { get; set; }

        public long id { get; set; }
    }
}
