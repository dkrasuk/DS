using System;

namespace DebtSettlement.AgreementLoader.Models
{
    internal class TransitAgreems
    {
        /// <summary>
        /// Gets or sets the agreement identifier.
        /// </summary>
        /// <value>The agreement identifier.</value>
        public int AgreementId { get; set; }

        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        /// <value>The person identifier.</value>
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or sets the agreement number.
        /// </summary>
        /// <value>The agreement number.</value>
        public string AgreementNumber { get; set; }

        /// <summary>
        /// Gets or sets the open date.
        /// </summary>
        /// <value>The open date.</value>
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// Gets or sets the planed close date.
        /// </summary>
        /// <value>The planed close date.</value>
        public DateTime? PlanedCloseDate { get; set; }

        /// <summary>
        /// Gets or sets the product code identifier.
        /// </summary>
        /// <value>The product code identifier.</value>
        public int? ProductCodeId { get; set; }

        /// <summary>
        /// Gets or sets the Outstanding identifier.
        /// </summary>
        public double? Outstanding { get; set; }

        /// <summary>
        /// Gets or sets the product code.
        /// </summary>
        /// <value>The product code.</value>
        public DictionaryProductCode ProductCode { get; set; }

        /// <summary>
        /// Gets or sets the person.
        /// </summary>
        /// <value>The person.</value>
        public TransitPersons Person { get; set; }

        /// <summary>
        /// Gets or sets the Oper_Workflow.
        /// </summary>
        /// <value>The Oper_Workflow.</value>
        public OperWorkflow OperWorkflow { get; set; }

        /// <summary>
        /// Gets or sets the Delinquency.
        /// </summary>
        public TransitDelinquency Delinquency { get; set; }
    }
}
