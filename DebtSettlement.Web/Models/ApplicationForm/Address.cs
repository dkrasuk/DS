using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class Address
    {
        [Display(Name = "Совпадает ли адрес проживания Клиента с адресом регистрации")]
        public bool MatchAddress { get; set; }

        [Display(Name = "Адрес регистрации Клиента")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string RegistrationAddress { get; set; }

        [Display(Name = "Доля владения Клиентом недвижимости по адресу проживания")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string MembershipInterestOnResidentAddress { get; set; }

        [Display(Name = "Доля владения Клиентом недвижимости по адресу регистрации")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string MembershipInterestOnRegistrAddress { get; set; }

        [Display(Name = "Адрес проживания Клиента")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string ResidentialAddress { get; set; }

        public List<SelectListItem> MembershipInterestOnResidentAddressList { get; set; }
        public List<SelectListItem> MembershipInterestOnRegistrAddressList { get; set; }
        public Address()
        {
            MembershipInterestOnResidentAddressList = new List<SelectListItem>();
            MembershipInterestOnRegistrAddressList = new List<SelectListItem>();
        }

    }
}
