using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class ReasonToDenyDS
    {
        [Display(Name = "Отсутствие правоустанавливающего документа")]
        public string AbsenceOfDocuments { get; set; }

        [Display(Name = "Наличие ареста в уголовном деле")]
        public bool PresenceOfArrest { get; set; } 

        [Display(Name = "Банкротство, реорганизация предприятий")]
        public bool BankruptsyReorganisation { get; set; } = false;

        [Display(Name = "Бракоразводный процесс (невозможность получить согласие супругов на сделку)")]
        public bool DivorceProceedings { get; set; } = false;

        [Display(Name = "Дети до 14 лет и собственники/наследники")]
        public bool Heirs { get; set; } = false;

        [Display(Name = "Отсутствие ипотекодателя (невозможность получения доверенности-пмж, тюрьма, в розыске)")]
        public bool AbsenceOfMortgagor { get; set; } = false;

        [Display(Name = "Наличие решения суда не в пользу Банка")]
        public bool JudgementInTheCase { get; set; } = false;

        [Display(Name = "Тяжелая болезнь клиента/смерть")]
        public bool DeadlyDisease { get; set; } = false;

        public List<SelectListItem> AbsenceOfDocumentsList { get; set; }

        public ReasonToDenyDS()
        {
            AbsenceOfDocumentsList = new List<SelectListItem>();
        }
    }
}
