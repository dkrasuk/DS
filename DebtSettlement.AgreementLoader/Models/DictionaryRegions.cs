using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtSettlement.AgreementLoader.Models
{
    internal class DictionaryRegions
    {
        public int RegionId { get; set; }

        public string Region { get; set; }

        public virtual ICollection<TransitContactsAdress> ContactsAdress { get; set; }
    }
}
