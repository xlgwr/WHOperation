namespace WHOperation.EF.WHO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIMSMRBException")]
    public partial class PIMSMRBException
    {
        [Key]
        [Column(Order = 0, TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal TransID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string DNNo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string DNDate { get; set; }

        [StringLength(20)]
        public string SupplierID { get; set; }

        [StringLength(20)]
        public string MfgrID { get; set; }

        [StringLength(10)]
        public string MG { get; set; }

        [StringLength(30)]
        public string PIMS { get; set; }

        [StringLength(50)]
        public string PartNumber { get; set; }

        [StringLength(100)]
        public string ReqMfgrPart { get; set; }

        [StringLength(100)]
        public string RecMfgrPart { get; set; }

        [StringLength(50)]
        public string CustPart { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RecQty { get; set; }

        [StringLength(20)]
        public string RIRNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdatedDate { get; set; }
    }
}
