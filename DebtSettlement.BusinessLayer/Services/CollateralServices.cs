using DebtSettlement.BusinessLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtSettlement.Model.DTO;
using CollateralService.ApiClient.Client;
using AlfaBank.Logger;
using DebtSettlement.Model.DTO.ApplicationForm;

namespace DebtSettlement.BusinessLayer.Services
{
    public class CollateralServices : ICollateralServices
    {
        private readonly ICollateralServiceWebAPI _collateralServiceWebApi;
        private readonly ILogger _logger;
        public CollateralServices(ICollateralServiceWebAPI collateralServiceWebApi, ILogger logger)
        {
            _collateralServiceWebApi = collateralServiceWebApi;
            _logger = logger;
        }

        public async Task<List<DebtSettlement.Model.DTO.ApplicationForm.Collateral>> GetCollaterals(string agreementId)
        {
            _logger.Info($"CollateralService.GetCollaterals [agreementId: {agreementId}]");

            var collateralsDto = new List<DebtSettlement.Model.DTO.ApplicationForm.Collateral>();
            if (agreementId == null)
            {
                return null;
            }
            await Task.Run(() =>
            {
                var collaterals = _collateralServiceWebApi.CreditAgreement.GetCollateralByCreditAgreement(agreementId).Where(n => n.IsActive == true && n.Deleted == false);
                if (collaterals.Count() > 0)
                {
                    foreach (var collateral in collaterals)
                    {
                        collateralsDto.Add(new DebtSettlement.Model.DTO.ApplicationForm.Collateral
                        {
                            CollateralId = collateral.Id,
                            CollateralDescription = string.IsNullOrEmpty(collateral.Description) ? collateral.ActualDescription : "",
                            CollateralType = collateral.Type?.Name,
                            CollateralValue = $"{collateral.Evaluation?.Value?.Value} {collateral.Evaluation?.Value?.Currency?.Name}",
                            isAdditionalProperty = collateral.IsAdditionalProperty
                        });
                    }
                }
            });
            return collateralsDto;
        }

        public async Task<DebtSettlement.Model.DTO.ApplicationForm.Collateral> GetDetailCollateral(string collateralId)
        {
            _logger.Info($"CollateralService.GetDetailCollateral [agreementId: {collateralId}]");

            var collateralDto = new DebtSettlement.Model.DTO.ApplicationForm.Collateral();
            if (collateralId == null)
            {
                return null;
            }
            await Task.Run(() =>
            {
                var collateral = _collateralServiceWebApi.Collateral.GetCollateralByCollateralId(collateralId);
                collateralDto = new DebtSettlement.Model.DTO.ApplicationForm.Collateral()
                {
                    CollateralId = collateral.Id,
                    CollateralType = collateral.Type?.Name,
                    CollateralDescription = string.IsNullOrEmpty(collateral.Description) ? collateral.ActualDescription : "",
                    CollateralValue = $"{collateral.Evaluation?.Value?.Value} {collateral.Evaluation?.Value?.Currency?.Name}",
                    isAdditionalProperty = collateral.IsAdditionalProperty
                };
            });
            return collateralDto;
        }
    }
}
