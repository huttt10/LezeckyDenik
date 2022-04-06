using LezeckyDenik.Models.StaticData;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExpressiveAnnotations.Attributes;
using LezeckyDenik.Utility;

namespace LezeckyDenik.Models
{
    public class Goal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Průměr")]
        [AssertThat("AverageInt <= HighestInt", ErrorMessage = "Průměr musí být menší než nejvýšší plánovaná obtížnost")]
        public string Average { get; set; }


        private int _averageInt;
        [NotMapped]
        public int AverageInt {
            get
            {
                _averageInt = ConverterDifficulty.GetIntFromDifficultyString(Average);
                return _averageInt;
            }

            set { _averageInt = value; }
        }

        [Required]        
        [Display(Name = "Nejvyšší")]
        [AssertThat("AverageInt <= HighestInt", ErrorMessage = "Průměr musí být menší než nejvýšší plánovaná obtížnost")]
        public string Highest { get; set; }

        private int _highestInt;
        [NotMapped]
        public int HighestInt {
            get
            {
                _highestInt = ConverterDifficulty.GetIntFromDifficultyString(Highest);
                return _highestInt;
            }

            set { _highestInt = value; }
        }

        [Required]
        [Display(Name = "Počet")]
        [Range(1, Int32.MaxValue)]
        public int Count { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMMM}", ApplyFormatInEditMode = true)]
        [Display(Name = "Cíl na měsíc")]
        public DateTime Month { get; set; }

        [Required]
        [Display(Name = "Dosáhnuto")]
        public bool Achieved { get; set; } = false;

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }

        [NotMapped]
        public List<SelectListItem> ChooseDifficulty { get; set; }

        public Goal()
        {
            ChooseDifficulty = new List<SelectListItem>();
            List<string> UIAA = ListOfUIAADifficulty.GetList();
            for (int i = 3; i < UIAA.Count; i++)
            {
                ChooseDifficulty.Add(new SelectListItem { Text = UIAA[i], Value = UIAA[i] });
            }
        }
    }
}
