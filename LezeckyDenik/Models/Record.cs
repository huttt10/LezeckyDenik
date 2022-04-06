using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using LezeckyDenik.Models.StaticData;
using LezeckyDenik.Utility;

namespace LezeckyDenik.Models
{
    public partial class Record
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Název")]
        public string Name { get; set; }

        [Display(Name = "Obtížnost")]
        public string Difficulty { get; set; }

        private int _modifyDifficulty;
        public int ModifyDifficulty
        {  
            get {
                _modifyDifficulty = ConverterDifficulty.GetIntFromDifficultyString(Difficulty);
                return _modifyDifficulty; 
            }
            
            set { _modifyDifficulty = value; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Datum vylezení")]
        public DateTime DateRecord { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }

        [NotMapped]
        public List<SelectListItem> ChooseDifficulty { get; set; }

        public Record()
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


