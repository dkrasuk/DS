using AutoMapper;
using DebtSettlement.Model.DTO.ApplicationForm;
using DebtSettlement.Web.Models.ApplicationForm;

namespace DebtSettlement.Web
{
    /// <summary>
    /// Class AutoMapperConfig.
    /// </summary>
    public static class AutoMapperConfig
    {
        /// <summary>
        /// Registers the mappings.
        /// </summary>
        public static void RegisterMappings()
        {
            Mapper.CreateMap<ApplicationFormViewModel, ApplicationForm>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.Client, Models.ApplicationForm.Client>();
            Mapper.CreateMap<Models.ApplicationForm.Client, Model.DTO.ApplicationForm.Client>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.Credit, Models.ApplicationForm.Credit>();
            Mapper.CreateMap<Models.ApplicationForm.Credit, Model.DTO.ApplicationForm.Credit>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.CollateralOtherParameters, Models.ApplicationForm.CollateralOtherParameters>();
            Mapper.CreateMap<Models.ApplicationForm.CollateralOtherParameters, Model.DTO.ApplicationForm.CollateralOtherParameters>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.ActiveOtherParameters, Models.ApplicationForm.ActiveOtherParameters>();
            Mapper.CreateMap<Models.ApplicationForm.ActiveOtherParameters, Model.DTO.ApplicationForm.ActiveOtherParameters>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.Job, Models.ApplicationForm.Job>();
            Mapper.CreateMap<Models.ApplicationForm.Job, Model.DTO.ApplicationForm.Job>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.Liability, Models.ApplicationForm.Liability>();
            Mapper.CreateMap<Models.ApplicationForm.Liability, Model.DTO.ApplicationForm.Liability>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.Income, Models.ApplicationForm.Income>();
            Mapper.CreateMap<Models.ApplicationForm.Income, Model.DTO.ApplicationForm.Income>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.Address, Models.ApplicationForm.Address>();
            Mapper.CreateMap<Models.ApplicationForm.Address, Model.DTO.ApplicationForm.Address>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.ReasonToDenyDS, Models.ApplicationForm.ReasonToDenyDS>();
            Mapper.CreateMap<Models.ApplicationForm.ReasonToDenyDS, Model.DTO.ApplicationForm.ReasonToDenyDS>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.Finance, Models.ApplicationForm.Finance>();
            Mapper.CreateMap<Models.ApplicationForm.Finance, Model.DTO.ApplicationForm.Finance>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.Decision, Models.ApplicationForm.Decision>();
            Mapper.CreateMap<Models.ApplicationForm.Decision, Model.DTO.ApplicationForm.Decision>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.Status, Models.ApplicationForm.Status>();
            Mapper.CreateMap<Models.ApplicationForm.Status, Model.DTO.ApplicationForm.Status>();

            Mapper.CreateMap<Model.DTO.ApplicationForm.Collateral, Models.ApplicationForm.Collateral>();
            Mapper.CreateMap<Models.ApplicationForm.Collateral, Model.DTO.ApplicationForm.Collateral>();
        }
    }
}