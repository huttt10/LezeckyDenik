using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LezeckyDenik.Models
{
    public class Training
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Datum provedení tréninku")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Zaměření")]
        public string Focus { get; set; }

        [Required]
        [Display(Name = "Popis")]
        public string Description { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Délka tréninku")]
        public DateTime Time { get; set; }

        [Display(Name = "Poznámka")]
        public string? Note { get; set; }

        [Display(Name = "Splněno")]
        public bool IsDone { get; set; }


        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }

        [NotMapped]
        public List<SelectListItem> ChooseFocus { get; set; }

        public Training()
        {
            ChooseFocus = new List<SelectListItem>();
            ChooseFocus.Add(new SelectListItem { Text = "Síla", Value = "Síla" });
            ChooseFocus.Add(new SelectListItem { Text = "Vytrvalostní aerobní (AE) trénink", Value = "Vytrvalostní aerobní (AE) trénink" });
            ChooseFocus.Add(new SelectListItem { Text = "Vytrvalostní anaerobní (AN) trénink", Value = "Vytrvalostní anaerobní (AN) trénink" });
            ChooseFocus.Add(new SelectListItem { Text = "Vytrvalostní anaerobní - aerobní (AN/AE) trénink", Value = "Vytrvalostní anaerobní - aerobní (AN/AE) trénink" });
            ChooseFocus.Add(new SelectListItem { Text = "Všeobecně vytrvalostní trénink", Value = "Všeobecně vytrvalostní trénink" });
        }
    }
}
