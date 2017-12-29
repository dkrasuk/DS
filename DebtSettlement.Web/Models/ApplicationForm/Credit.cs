using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class Credit
    {
        [Display(Name = "Номер сделки")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public List<int> AgreemNumber { get; set; }

        [Display(Name = "Портфель")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Portfolio { get; set; }

        public List<SelectListItem> PortfolioList { get; set; }

        [Display(Name = "Outstanding, $")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public double? Outstanding { get; set; }

        [Display(Name = "Fees, $")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public double? Fees { get; set; }

        [Display(Name = "Тип DS")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string DSType { get; set; }

        [Display(Name = "DPD")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public int? DPD { get; set; }

        public List<SelectListItem> DSTypeList { get; set; }

        public Credit()
        {
            PortfolioList = new List<SelectListItem>();
            DSTypeList = new List<SelectListItem>();
        }

    }
}