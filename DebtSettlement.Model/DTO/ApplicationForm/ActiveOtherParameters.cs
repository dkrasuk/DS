using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class ActiveOtherParameters
    {
        public string ActivesSelected { get; set; }

        public int? OwnershipShare { get; set; }

        public string SourceInformationOfActive { get; set; }

        public string Comment { get; set; }

    }
}
