namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Time")]
    public partial class Time
    {
        [Key]
        public DateTime PK_Date { get; set; }

        [StringLength(50)]
        public string Date_Name { get; set; }

        public DateTime? Year { get; set; }

        [StringLength(50)]
        public string Year_Name { get; set; }

        public DateTime? Month { get; set; }

        [StringLength(50)]
        public string Month_Name { get; set; }

        public int? Day_Of_Year { get; set; }

        [StringLength(50)]
        public string Day_Of_Year_Name { get; set; }

        public int? Day_Of_Month { get; set; }

        [StringLength(50)]
        public string Day_Of_Month_Name { get; set; }

        public int? Month_Of_Year { get; set; }

        [StringLength(50)]
        public string Month_Of_Year_Name { get; set; }
    }
}
