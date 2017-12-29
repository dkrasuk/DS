using System.Collections.Generic;

namespace DebtSettlement.AgreementLoader.Models
{
    internal class OperWorkflow
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the agreement identifier.
        /// </summary>
        /// <value>The agreement identifier.</value>
        public int AgreementId { get; set; }

        /// <summary>
        /// Gets or sets the assigned field user.
        /// </summary>
        /// <value>The assigned field user.</value>
        public string AssignedFieldUser { get; set; }

        /// <summary>
        /// Gets or sets the assigned legal user.
        /// </summary>
        /// <value>The assigned legal user.</value>
        public string AssignedLegalUser { get; set; }

        /// <summary>
        /// Gets or sets the assigned collector.
        /// </summary>
        /// <value>The assigned collector.</value>
        public string AssignedCollector { get; set; }

        /// <summary>
        /// Gets or sets the agreements.
        /// </summary>
        /// <value>The agreements.</value>
        public virtual TransitAgreems TransitAgreem { get; set; }
    }
}
