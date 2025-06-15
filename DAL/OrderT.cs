namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderT")]
    public partial class OrderT
    {
        [Key]
        public int OrderID { get; set; }

        public int? UserID { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OrderDate { get; set; }

        [StringLength(50)]
        public string phone { get; set; }

        public int? ActID { get; set; }

        [StringLength(50)]
        public string Act_Name { get; set; }

        [StringLength(50)]
        public string Holder { get; set; }

        [StringLength(50)]
        public string status { get; set; }

        public int? EmpID { get; set; }
    }
}
