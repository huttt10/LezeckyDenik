using System.ComponentModel.DataAnnotations;

namespace LezeckyDenik.Models
{
    public class MainPage
    {
        [Key]
        public string Title { get; set; }

        public string? Content { get; set; }


    }
}
