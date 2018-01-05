using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class Status
    {
        [Display(Name = "Есть ли другие продукты в Банке")]
        public bool HaveAnotherBankProduct{ get; set; }

        [Display(Name = "Юридическая стадия")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string LegalStage { get; set; }

        public List<SelectListItem> LegalStageList { get; set; }

        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime? Date { get; set; }

        [Display(Name = "Юридический статус")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string LegalStatus { get; set; }

        [Display(Name = "Открыто банкротство/Статус")]
        public bool StatusOfBankruptcy { get; set; }

        [Display(Name = "Открыто банкротство/Дата")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime? OpenDateOfBankruptcy { get; set; }

        [Display(Name = "Есть ли риск потери залога/Риск")]
        public bool RiskOfLoss { get; set; }

        [Display(Name = "Есть ли риск потери залога/Дата")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime? DateRiskOfLoss { get; set; }

        [Display(Name = "Если Да, то какие действия планируются и сроки")]
        [StringLength(2000, ErrorMessage = "Превышение максимального кол-ва символов (2000)")]
        public string ActionsPlanned { get; set; }

        [Display(Name = "Текущий инструмент")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string CurrentTool { get; set; }

        public List<SelectListItem> CurrentToolList { get; set; }

        [Display(Name = "Аргументация")]
        [StringLength(2000, ErrorMessage = "Превышение максимального кол-ва символов (2000)")]
        public string Argumentation { get; set; }

        [Display(Name = "Заключение СБ (при наличии запроса)")]
        [StringLength(2000, ErrorMessage = "Превышение максимального кол-ва символов (2000)")]
        public string ConclusionSecurity { get; set; }

        [Display(Name = "История переговоров с должником и текущий актуальный статус")]
        [StringLength(2000, ErrorMessage = "Превышение максимального кол-ва символов (2000)")]
        public string HistoryTalks { get; set; }

        public Status()
        {
            CurrentToolList = new List<SelectListItem>();
            LegalStageList = new List<SelectListItem>();
        }
    }
}