using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtSettlement.BusinessLayer.Services.Interfaces
{
    public interface ICollateralServices
    {
        Task<List<DebtSettlement.Model.DTO.ApplicationForm.Collateral>> GetCollaterals(string agreementId);
        Task<List<DebtSettlement.Model.DTO.ApplicationForm.Collateral>> GetActives(string agreementId);
        Task<DebtSettlement.Model.DTO.ApplicationForm.Collateral> GetDetailCollateral(string collateralId);

    }
}
