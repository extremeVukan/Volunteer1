namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ActivityT")]
    public partial class ActivityT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int activity_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string activity_Name { get; set; }

        [StringLength(50)]
        public string activity_type { get; set; }

        public DateTime addtime { get; set; }

        public DateTime stoptime { get; set; }

        [StringLength(50)]
        public string place { get; set; }

        public int? renshu { get; set; }

        [StringLength(200)]
        public string Image { get; set; }

        [StringLength(255)]
        public string descn { get; set; }

        public int? ShengyuNum { get; set; }

        [StringLength(50)]
        public string Holder { get; set; }

        [StringLength(50)]
        public string status { get; set; }
    }
}
