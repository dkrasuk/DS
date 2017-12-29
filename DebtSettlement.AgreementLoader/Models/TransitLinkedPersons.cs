using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtSettlement.AgreementLoader.Models
{
    internal class TransitLinkedPersons
    {
        public int? LinkId { get; set; }

        public int? LincedPersonId { get; set; }

        public int? PersonId { get; set; }

        public int? LinkTypeId { get; set; }

        public int? AgreemId { get; set; }

        public int? LinkedAgreemId { get; set; }

        public TransitPersons Persons { get; set; }
    }
}
