namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VolIdentifyT")]
    public partial class VolIdentifyT
    {
        [Key]
        public int Num { get; set; }

        [StringLength(50)]
        public string VID { get; set; }

        [StringLength(50)]
        public string VName { get; set; }

        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Province { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public int? EMPID { get; set; }
    }
}
