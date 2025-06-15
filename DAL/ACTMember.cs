namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ACTMember")]
    public partial class ACTMember
    {
        [Key]
        public int NUM { get; set; }

        public int? ACTID { get; set; }

        [StringLength(50)]
        public string ACTNAME { get; set; }

        public int? Volunteerid { get; set; }

        [StringLength(50)]
        public string volunteer { get; set; }

        [StringLength(50)]
        public string PHONE { get; set; }

        [StringLength(50)]
        public string Time { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SignTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReturnTime { get; set; }
    }
}
