using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LezeckyDenik.Models
{
    public class Article
    {        
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        [Required]
        public bool Published { get; set; } = false;

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }
    }
}
