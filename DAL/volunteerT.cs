namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("volunteerT")]
    public partial class volunteerT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Aid { get; set; }

        [StringLength(50)]
        public string AName { get; set; }

        [StringLength(11)]
        public string Atelephone { get; set; }

        [StringLength(30)]
        public string email { get; set; }

        [StringLength(50)]
        public string Act_Time { get; set; }
    }
}
