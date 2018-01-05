using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class Decision
    {
        [Display(Name = "Проект решения")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string DraftDecision { get; set; }

        [Display(Name = "Фин. Эффект")]
        public double? FinEffect { get; set; }

        [Display(Name = "Cash % to Outstanding")]
        public double? CashOutstanding { get; set; }

        [Display(Name = "Cash % to Outstanding (ликвидация)")]
        public double? CashOutstandingLiquidation { get; set; }

        [Display(Name = "Стоимость в ГИС (70%)")]
        public double? CostGIS { get; set; }

        [Display(Name = "Ликвидационная Стоимость")]
        public double? LiquidationValue { get; set; }
    }
}