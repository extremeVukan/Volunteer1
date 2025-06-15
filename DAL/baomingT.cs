namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("baomingT")]
    public partial class baomingT
    {
        [Key]
        [StringLength(50)]
        public string username { get; set; }

        [StringLength(50)]
        public string activity_ID { get; set; }

        [StringLength(255)]
        public string place { get; set; }

        [StringLength(50)]
        public string telephone { get; set; }

        public DateTime? applytime { get; set; }

        [StringLength(50)]
        public string shenhe { get; set; }
    }
}
