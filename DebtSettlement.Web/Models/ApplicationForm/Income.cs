using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class Income
    {
        [Display(Name = "Установленные доходы (официальные/неофициальные)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Range(0, int.MaxValue, ErrorMessage = "Неверный формат числа")]
        public double? IncomeValue { get; set; }

        [Display(Name = "Источник доходов")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string SourceOfIncome { get; set; }

        [Display(Name = "Источник информации о доходах")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string SourceInformationOfIncome { get; set; }

        public List<SelectListItem> SourceInformationOfIncomesList { get; set; }

        [Display(Name = "Источник поступления средстви для DS")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string SourceOfFundsDS { get; set; }

        public Income()
        {
            SourceInformationOfIncomesList = new List<SelectListItem>();
        }        
    }
}
