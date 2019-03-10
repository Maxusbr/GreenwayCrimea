namespace AdvantShop.Module.SimaLand.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("Module.SimalandCategory")]
    public partial class SimalandCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public int level { get; set; }

        public bool is_adult { get; set; }

        public bool is_leaf { get; set; }

        [Required]
        [StringLength(255)]
        public string path { get; set; }

        public bool custom { get; set; }

        [Required]
        [StringLength(255)]
        public string full_slug { get; set; }

        public int? CategoryId { get; set; }
        
    }
}
