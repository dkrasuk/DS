using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class ActiveOtherParameters
    {
        /// <summary>
        /// Gets or sets the ActivesSelected.
        /// </summary>
        /// <value>
        /// The source of other actives.
        /// </value>
        public string ActivesSelected { get; set; }

        /// <summary>
        /// Gets or sets the OwnershipShare.
        /// </summary>
        /// <value>
        /// The collaterals.
        /// </value>
        [Display(Name = "% вледения")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Range(0, 100, ErrorMessage = "Диапазон числа (0-100)")]
        public int? OwnershipShare { get; set; }

        /// <summary>
        /// Gets or sets the SourceInformationOfActive.
        /// </summary>
        /// <value>
        /// The collaterals.
        /// </value>
        [Display(Name = "Источник информации (о наличии др.активов)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string SourceInformationOfActive { get; set; }

        /// <summary>
        /// Gets or sets the SourceInformationOfActiveList.
        /// </summary>
        /// <value>
        /// The collaterals.
        /// </value>
        public List<SelectListItem> SourceInformationOfActiveList { get; set; }

        [Display(Name = "Комментарий")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(2000, ErrorMessage = "Превышение максимального кол-ва символов (2000)")]
        public string Comment { get; set; }

        public ActiveOtherParameters()
        {
                SourceInformationOfActiveList = new List<SelectListItem>();
        }

    }
}