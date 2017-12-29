using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class Job
    {
        [Display(Name = "Место работы/Сфера деятельности")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string WorkPlace { get; set; }

        [Display(Name = "Должность")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Position { get; set; }

        [Display(Name = "Источник информации")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Source { get; set; }

        public List<SelectListItem> SourceList { get; set; }

        public Job()
        {
            SourceList = new List<SelectListItem>();
        }
    }


}

