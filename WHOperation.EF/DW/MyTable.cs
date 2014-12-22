namespace WHOperation.EF.DW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MyTable")]
    public partial class MyTable
    {
        [Key]
        public int mainkey { get; set; }

        public int? mainvalue { get; set; }
    }
}
