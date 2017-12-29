using System.Collections.Generic;

namespace DebtSettlement.AgreementLoader.Models
{
    internal class TransitPersons
    {
        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        /// <value>The person identifier.</value>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the Inn.
        /// </summary>
        public string Inn { get; set; }

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
        /// Gets or sets the name of the juridical.
        /// </summary>
        /// <value>The name of the juridical.</value>
        public string JuridicalName { get; set; }

        /// <summary>
        /// Gets or sets the fio.
        /// </summary>
        /// <value>The fio.</value>
        public string Fio { get; set; }


        /// <summary>
        /// Gets or sets the agreements.
        /// </summary>
        /// <value>The agreements.</value>
        public virtual ICollection<TransitAgreems> Agreements { get; set; }
        public virtual ICollection<TransitContactsAdress> ContactsAdress { get; set; }
        public virtual ICollection<TransitLinkedPersons> LinkedPersons { get; set; }
    }
}
