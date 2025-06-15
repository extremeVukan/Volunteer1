namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dalogT")]
    public partial class dalogT
    {
        [Key]
        public int log_id { get; set; }

        [StringLength(50)]
        public string username { get; set; }

        [StringLength(50)]
        public string log_type { get; set; }

        public DateTime? action_date { get; set; }

        [StringLength(50)]
        public string action_table { get; set; }
    }
}
