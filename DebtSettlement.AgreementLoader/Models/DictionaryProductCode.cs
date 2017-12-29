using System.Collections.Generic;

namespace DebtSettlement.AgreementLoader.Models
{
    internal class DictionaryProductCode
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the product code.
        /// </summary>
        /// <value>The product code.</value>
        public string ProductCode { get; set; }

        /// <summary>
        /// Gets or sets the agreements.
        /// </summary>
        /// <value>The agreements.</value>
        public virtual ICollection<TransitAgreems> Agreements { get; set; }
    }
}
