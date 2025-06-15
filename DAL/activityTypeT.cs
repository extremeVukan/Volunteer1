namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("activityTypeT")]
    public partial class activityTypeT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int activity_ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Beizhu { get; set; }
    }
}
