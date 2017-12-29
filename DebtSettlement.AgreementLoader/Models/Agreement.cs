using System;
using System.Collections.Generic;

namespace DebtSettlement.AgreementLoader.Models
{
    public class Agreement
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Inn.
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the full name of the person.
        /// </summary>
        /// <value>The full name of the person.</value>
        public string PersonFullName { get; set; }

        /// <summary>
        /// Gets or sets the open date.
        /// </summary>
        /// <value>The open date.</value>
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// Gets or sets the close date.
        /// </summary>
        /// <value>The close date.</value>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// Gets or sets the product code identifier.
        /// </summary>
        /// <value>The product code identifier.</value>
        public string ProductCode { get; set; }

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
        /// Gets or sets the assigned field user fullName.
        /// </summary>
        /// <value>The assigned field user.</value>
        public string AssignedFieldUserFullName { get; set; }

        /// <summary>
        /// Gets or sets the assigned legal user fullName.
        /// </summary>
        /// <value>The assigned legal user.</value>
        public string AssignedLegalUserFullName { get; set; }

        /// <summary>
        /// Gets or sets the assigned collector fullName.
        /// </summary>
        /// <value>The assigned collector.</value>
        public string AssignedCollectorFullName { get; set; }

        /// <summary>
        /// Gets or sets the processes.
        /// </summary>
        /// <value>The processes.</value>
        public IEnumerable<ProcessDTO> Processes { get; set; }
    }
}
