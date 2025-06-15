namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ACTapply_T
    {
        [Key]
        public int Aqueue_ID { get; set; }

        public int? Vol_ID { get; set; }

        public int? Act_ID { get; set; }

        [StringLength(50)]
        public string Act_Name { get; set; }

        [StringLength(50)]
        public string Act_place { get; set; }

        public int? Need { get; set; }

        public int? Qty { get; set; }

        [StringLength(50)]
        public string Holder { get; set; }
    }
}
