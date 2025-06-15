namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("adminT")]
    public partial class adminT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int admin_ID { get; set; }

        [StringLength(50)]
        public string admin_Name { get; set; }

        [StringLength(2)]
        public string sex { get; set; }

        public DateTime? birth_date { get; set; }

        public DateTime? hire_date { get; set; }

        [StringLength(50)]
        public string address { get; set; }

        [StringLength(50)]
        public string telephone { get; set; }

        public int? wages { get; set; }

        [Column(TypeName = "text")]
        public string resume { get; set; }
    }
}
