using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class Liability
    {
        [Display(Name = "Выявленные обязательства Клиента в др. финансово-кредитных учереждениях")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(4000, ErrorMessage = "Превышение максимального кол-ва символов (4000)")]
        public string OtherLiability { get; set; }

        [Display(Name = "Источник информации о выявленных обязательствах")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string SourceOfOtherLiability { get; set; }

        public List<SelectListItem> SourceOfOtherLiabilityList { get; set; }
        public Liability()
        {
            SourceOfOtherLiabilityList = new List<SelectListItem>();
        }
    }
}
