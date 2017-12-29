using System;

namespace DebtSettlement.AgreementLoader.Models
{
    internal class TransitContactsAdress
    {
        /// <summary>
        /// Gets or sets the Adress identifier.
        /// </summary>
        public int AdressId { get; set; }

        /// <summary>
        /// Gets or sets the Person identifier.
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or sets the Region identifier.
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// Gets or sets the Settlement.
        /// </summary>
        public string Settlement { get; set; }

        /// <summary>
        /// Gets or sets the AdressFull.
        /// </summary>
        public string AdressFull { get; set; }

        /// <summary>
        /// Gets or sets the Work.
        /// </summary>
        public string Work { get; set; }

        public TransitPersons Person { get; set; }

        public DictionaryRegions Region { get; set; }

    }
}
