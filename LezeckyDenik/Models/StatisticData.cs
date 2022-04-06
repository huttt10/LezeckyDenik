using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LezeckyDenik.Models
{
    public class StatisticData
    {
        [Key]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }

        public string Average { get; set; }

        public string Highest { get; set; }        

        public int Count { get; set; }
    }
}
