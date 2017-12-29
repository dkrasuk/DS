using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtSettlement.AgreementLoader.Models
{
    internal class TransitDelinquency
    {
        public int AgreemId { get; set; }

        public int? DPD { get; set; }

        public virtual TransitAgreems TransitAgreem { get; set; }
    }
}
