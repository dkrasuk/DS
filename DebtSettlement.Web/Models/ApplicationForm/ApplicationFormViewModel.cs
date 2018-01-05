using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DebtSettlement.Web.Models.ApplicationForm
{
    /// <summary>
    /// Application Form ViewModel
    /// </summary>
    public class ApplicationFormViewModel
    {
        /// <summary>
        /// Gets or sets the agreement identifier.
        /// </summary>
        /// <value>The agreement identifier.</value>
        [Display(Name = "Номер договора")]
        [Required]
        public int? AgreementId { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public Client Client { get; set; }

        /// <summary>
        /// Gets or sets the credit.
        /// </summary>
        /// <value>
        /// The credit.
        /// </value>
        public Credit Credit { get; set; }

        /// <summary>
        /// Gets or sets the collaterals.
        /// </summary>
        /// <value>
        /// The collaterals.
        /// </value>
        public List<Collateral> Collaterals { get; set; }

        /// <summary>
        /// Gets or sets the CollateralOtherParameters.
        /// </summary>
        /// <value>
        /// The collaterals.
        /// </value>
        public CollateralOtherParameters CollateralOtherParameters { get; set; }

        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        /// <value>
        /// The job.
        /// </value>
        public Job Job { get; set; }

        /// <summary>
        /// Gets or sets the liability.
        /// </summary>
        /// <value>
        /// The liability.
        /// </value>
        public Liability Liability { get; set; }

        /// <summary>
        /// Gets or sets the income.
        /// </summary>
        /// <value>
        /// The income.
        /// </value>
        public Income Income { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [other actives].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [other actives]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Наличие других активов")]
        public bool OtherActives { get; set; }

        /// <summary>
        /// Gets or sets the Actives.
        /// </summary>
        /// <value>
        /// The source of other actives.
        /// </value>
        public List<Collateral> Actives { get; set; }

        /// <summary>
        /// Gets or sets the ActiveOtherParameters.
        /// </summary>
        /// <value>
        /// The source of other actives.
        /// </value>
        public ActiveOtherParameters ActiveOtherParameters { get; set; }

        /// <summary>
        /// Gets or sets the reason to deny ds.
        /// </summary>
        /// <value>
        /// The reason to deny ds.
        /// </value>
        public ReasonToDenyDS ReasonToDenyDS { get; set; }

        /// <summary>
        /// Gets or sets the history of business negotiations.
        /// </summary>
        /// <value>
        /// The history of business negotiations.
        /// </value>
        [Display(Name = "История переговоров с должником и текущий актуальный статус")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(2000, ErrorMessage = "Превышение максимального кол-ва символов (2000)")]
        public string HistoryOfBusinessNegotiations { get; set; }

        /// <summary>
        /// Gets or sets the guarantors.
        /// </summary>
        /// <value>
        /// The guarantors.
        /// </value>
        [Display(Name = "Поручители")]
        [DataType(DataType.MultilineText)]
        public string Guarantors { get; set; }

        /// <summary>
        /// Gets or sets the Decision.
        /// </summary>
        /// <value>
        /// The guarantors.
        /// </value>
        public Decision Decision { get; set; }

        /// <summary>
        /// Gets or sets the Finance.
        /// </summary>
        /// <value>
        /// The guarantors.
        /// </value>
        public Finance Finance { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>
        /// The guarantors.
        /// </value>
        public Status Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationFormViewModel"/> class.
        /// </summary>
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
            CollateralOtherParameters = new CollateralOtherParameters();
            ActiveOtherParameters = new ActiveOtherParameters();
            Decision = new Decision();
            Finance = new Finance();
            Status = new Status();
        }

    }
}