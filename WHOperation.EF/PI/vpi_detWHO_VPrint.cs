namespace WHOperation.EF.PI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vpi_detWHO_VPrint
    {
        public string pi_dateCode { get; set; }

        public string pi_lotNumber { get; set; }

        [StringLength(12)]
        public string PI_LOT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal NumOfCarton { get; set; }

        [Column(TypeName = "numeric")]
        public decimal NumOfLabel { get; set; }

        [Column(TypeName = "numeric")]
        public decimal NumOfAllCarton { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Remainder { get; set; }


        [Column(TypeName = "numeric")]
        public decimal PI_QTY { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal PI_PO_price { get; set; }

        [StringLength(18)]
        public string PI_PART { get; set; }
        [StringLength(100)]
        public string pi_mfgr_part { get; set; }


        [Key]
        [Column(Order = 0)]
        public decimal PI_Print_QTY { get; set; }


        [StringLength(12)]
        public string PI_PALLET { get; set; }

        [StringLength(12)]
        public string PI_CARTON_NO { get; set; }

        [StringLength(6)]
        public string PI_SITE { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "numeric")]
        public decimal ttlQTY { get; set; }

        [StringLength(8)]
        public string PI_PO { get; set; }

        [StringLength(8)]
        public string pi_mfgr { get; set; }        

        [Key]
        [Column(Order = 3)]
        [StringLength(12)]
        public string PI_NO { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PI_LINE { get; set; }

        public DateTime pi_cre_time { get; set; }
    }
}
