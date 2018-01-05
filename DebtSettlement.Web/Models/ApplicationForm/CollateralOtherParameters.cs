using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class CollateralOtherParameters
    {
        /// <summary>
        /// Gets or sets the CollateralsSelected.
        /// </summary>
        /// <value>
        /// The collaterals.
        /// </value>
        public string Selected { get; set; }

        /// <summary>
        /// Gets or sets the CollateralOutstanding.
        /// </summary>
        /// <value>
        /// The collaterals.
        /// </value>
        [Display(Name = "% покрытия к Outstanding")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public double? CoverOutstanding { get; set; }

        /// <summary>
        /// Gets or sets the CollateralRepaymentAmount.
        /// </summary>
        /// <value>
        /// The collaterals.
        /// </value>
        [Display(Name = "Сумма погашения, $")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^[0-9]{1,13}([,][0-9]{1,2})?$", ErrorMessage = "Неверный формат числа")]
        public double? RepaymentAmount { get; set; }

        [Display(Name = "Другие активы для взыскания")]
        public bool OtherAssetsForCollection { get; set; }

        [Display(Name = "Проверка оценка УОРЗ")]
        public bool CheckEvaluation { get; set; }

        [Display(Name = "Подтверждение ОПЗН")]
        public bool Approve { get; set; }
    }
}