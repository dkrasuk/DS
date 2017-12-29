using System.IO;

namespace DebtSettlement.AgreementLoader.Models
{
    /// <summary>
    /// Class SearchResult.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets the agreement identifier.
        /// </summary>
        /// <value>The agreement identifier.</value>
        public int AgreementId { get; set; }

        /// <summary>
        /// Gets or sets the agreement number.
        /// </summary>
        /// <value>The agreement number.</value>
        public string AgreementNumber { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the patronymic.
        /// </summary>
        /// <value>The patronymic.</value>
        public string Patronymic { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>The product.</value>
        public string ProductCode { get; set; }

        /// <summary>
        /// Gets or sets the inn.
        /// </summary>
        /// <value>The inn.</value>
        public string Inn { get; set; }

        /// <summary>
        /// Gets or sets the DPD.
        /// </summary>
        /// <value>The DPD.</value>
        public int Dpd { get; set; }

        /// <summary>
        /// Gets or sets the outstanding.
        /// </summary>
        /// <value>The outstanding.</value>
        public float Outstanding { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }
    }
}
