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

        [Display(Name = "Macro Segment")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string MacroSegment { get; set; }
        public List<SelectListItem> MacroSegmentList { get; set; }

        [Display(Name = "Portfolio")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Portfolio { get; set; }

        [Display(Name = "Sub Segment")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string SubSegment { get; set; }

        public List<SelectListItem> RegionList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public Client()
        {
            RegionList = new List<SelectListItem>();
            CityList = new List<SelectListItem>();
            MacroSegmentList = new List<SelectListItem>();
        }
    }
}