using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class Collateral
    {

        public string CollateralId { get; set; }

        [Display(Name = "Тип залога")]
        public string CollateralType { get; set; }

        public List<SelectListItem> CollateralTypeList { get; set; }

        [Display(Name = "Описание залога")]
        public string CollateralDescription { get; set; }

        [Display(Name = "Стоимость залога, $")]
        public string CollateralValue { get; set; }

        [Display(Name = "% покрытия к Outstanding")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string PersentOutstanding { get; set; }

        [Display(Name = "Сумма погашения, $")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^[0-9]{1,13}([,][0-9]{1,2})?$", ErrorMessage = "Неверный формат числа")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string PaymentSum { get; set; }

        public bool? isAdditionalProperty { get; set; }

        public Collateral()
        {
            CollateralTypeList = new List<SelectListItem>();
        }    
    }


}
