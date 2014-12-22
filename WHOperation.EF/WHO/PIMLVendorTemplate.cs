namespace WHOperation.EF.WHO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIMLVendorTemplate")]
    public partial class PIMLVendorTemplate
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string VendorID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string TemplateID { get; set; }

        [Column(TypeName = "xml")]
        public string xmlVendorData { get; set; }

        [StringLength(10)]
        public string isDefault { get; set; }

        public byte[] templateImage { get; set; }
    }
}
