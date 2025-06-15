namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("zhuguanT")]
    public partial class zhuguanT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int S_id { get; set; }

        [StringLength(50)]
        public string Sname { get; set; }

        [StringLength(50)]
        public string Semail { get; set; }
    }
}
