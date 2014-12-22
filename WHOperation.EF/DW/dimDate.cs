namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dimDate")]
    public partial class dimDate
    {
        [Key]
        [Column(TypeName = "date")]
        public DateTime DateKey { get; set; }

        public int? CalYear { get; set; }

        public int? CalMonth { get; set; }

        public int? CalDayofWeek { get; set; }

        [StringLength(3)]
        public string EnglishMonthName { get; set; }
    }
}
