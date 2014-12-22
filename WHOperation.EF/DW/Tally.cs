namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tally")]
    public partial class Tally
    {
        [Key]
        public int N { get; set; }
    }
}
