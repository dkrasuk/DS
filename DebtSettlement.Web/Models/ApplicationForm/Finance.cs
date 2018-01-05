using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class Finance
    {
        [Display(Name = "CashTotal")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public double? CashTotal { get; set; }

        [Display(Name = "NPV")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public double? NPV { get; set; }

        [Display(Name = "Cash to Collateral Value")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public double? CashCollateralValue { get; set; }

        [Display(Name = "LTV")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public double? LTV { get; set; }

        [Display(Name = "Cash to Out")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public double? CashToOut { get; set; }

        [Display(Name = "Срок рассрочки, мес.")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public double? TermInstallments { get; set; }

        [Display(Name = "First Payment")]
        public double? FirstPayment { get; set; }

        [Display(Name = "FP to Value")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public double? FPValue { get; set; }

        [Display(Name = "Другие кредиты")]
        public bool OtherCredits { get; set; }

        [Display(Name = "Тип DS")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string TypeDS { get; set; }

        public List<SelectListItem> TypeDSList { get; set; }

        [Display(Name = "Отклонение")]
        public bool Reject { get; set; }

        [Display(Name = "Суть отклонения")]
        [StringLength(2000, ErrorMessage = "Превышение максимального кол-ва символов (2000)")]
        public string EssenceDeviation { get; set; }

        public Finance()
        {
                TypeDSList = new List<SelectListItem>();
        }
    }
}