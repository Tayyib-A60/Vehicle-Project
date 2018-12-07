using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Vega.Models
{
    public class Feature
    {
        [Required]
        [StringLength(255)] 
        public string FeatureName { get; set; }
        [Key]
        public int FeatureId { get; set; }
    }
}