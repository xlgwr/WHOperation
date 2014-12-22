namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Document_Definition_5
    {
        [Key]
        public int ROW_INDEX { get; set; }

        [StringLength(4000)]
        public string invno { get; set; }
    }
}
