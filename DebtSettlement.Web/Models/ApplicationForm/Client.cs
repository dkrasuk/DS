using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class Client
    {
        [Display(Name = "ИНН")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string INN { get; set; }

        [Display(Name = "ФИО")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string FIO { get; set; }

        [Display(Name = "Регион")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Region { get; set; }

        [Display(Name = "Город")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string City { get; set; }

        public List<SelectListItem> RegionList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public Client()
        {
            RegionList = new List<SelectListItem>();
            CityList = new List<SelectListItem>();
        }
    }
}