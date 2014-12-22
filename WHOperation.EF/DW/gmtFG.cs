namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("gmtFG")]
    public partial class gmtFG
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int zser { get; set; }

        [StringLength(60)]
        public string Znamec { get; set; }

        [StringLength(60)]
        public string zspecial { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal zprice { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Zunit_id { get; set; }

        [StringLength(16)]
        public string Zunit { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ZCountry_id { get; set; }

        [StringLength(16)]
        public string Zcountry { get; set; }
    }
}
