using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class Decision
    {
        public string DraftDecision { get; set; }

        public double? FinEffect { get; set; }

        public double? CashOutstanding { get; set; }

        public double? CashOutstandingLiquidation { get; set; }

        public double? CostGIS { get; set; }

        public double? LiquidationValue { get; set; }
    }
}
