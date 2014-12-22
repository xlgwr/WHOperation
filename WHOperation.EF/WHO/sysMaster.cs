namespace WHOperation.EF.WHO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sysMaster")]
    public partial class sysMaster
    {
        [Key]
        [StringLength(10)]
        public string SystemID { get; set; }

        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(3)]
        public string verNo { get; set; }

        [StringLength(5)]
        public string NewVerNo { get; set; }
    }
}
