using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    public class ApplicationFormViewModel
    {
        /// <summary>
        /// Gets or sets the agreement identifier.
        /// </summary>
        /// <value>The agreement identifier.</value>
        [Display(Name = "Номер договора")]
        [Required]
        public int? AgreementId { get; set; }

        public Client Client { get; set; }

        public Credit Credit { get; set; }

        public List<Collateral> Collaterals { get; set; }

        public Job Job { get; set; }
        
        public Liability Liability { get; set; }
        
        public Income Income { get; set; }

        public Address Address { get; set; }

        [Display(Name = "Наличие других активов")]
        public bool OtherActives { get; set; }

        [Display(Name = "Источник информации (о наличии др.активов)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.MultilineText)]
        public string SourceOfOtherActives { get; set; }

        public List<Collateral> Actives { get; set; }

        public ReasonToDenyDS ReasonToDenyDS { get; set; }

        [Display(Name = "История переговоров с должником и текущий актуальный статус")]
        [DataType(DataType.MultilineText)]
        public string HistoryOfBusinessNegotiations { get; set; }

        [Display(Name = "Поручители")]
        [DataType(DataType.MultilineText)]
        public string Guarantors { get; set; }

        public ApplicationFormViewModel()
        {
            Job = new Job();
            Client = new Client();
            Credit = new Credit();
            Income = new Income();
            ReasonToDenyDS = new ReasonToDenyDS();
            Actives = new List<Collateral>();
            Collaterals = new List<Collateral>();
            Liability = new Liability();
            Address = new Address();
        }
      
    }
}