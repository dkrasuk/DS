using System.Web.Mvc;
using DebtSettlement.Web.Models.Task;
using System;
using System.Collections.Generic;
using DebtSettlement.Web.Helpers;
//using DebtSettlement.AgreementLoader.Interface;
using System.Linq;
using System.Web;
using AlfaBank.Logger;
using Newtonsoft.Json;
using DebtSettlement.Web.Filters;

using HR.Client.Interface;

using DebtSettlement.Web.Models.ApplicationForm;
using AutoMapper;
using DebtSettlement.AgreementLoader.Interface;
using DebtSettlement.AgreementLoader;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DebtSettlement.BusinessLayer;
using DebtSettlement.BusinessLayer.Services.Interfaces;

namespace DebtSettlement.Web.Controllers
{
    /// <summary>
    /// Class TaskController.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    [RoutePrefix("debtsettlement")]
    public class DebtSettlementController : Controller
    {
        private ICollateralServices CollateralService;
        private IDebtSettlementService DebtSettlementService;
        private IDictionaryService DictionaryService;
        private static ILogger _logger;

        public DebtSettlementController(
            ICollateralServices _collateralService, IDebtSettlementService _debtSettlementService, IDictionaryService _dictionaryService, ILogger logger)
        {
            CollateralService = _collateralService;
            DebtSettlementService = _debtSettlementService;
            DictionaryService = _dictionaryService;
            _logger = logger;
        }

        /// <summary>
        /// Creates the task.
        /// </summary>
        /// <returns>ViewResult.</returns>
        [HttpGet]
        public async Task<ViewResult> CreateDebtSettlement(int agreementId)
        {
            var model = new ApplicationFormViewModel();

            var agreementDto = await DebtSettlementService.GetAgreementInfo(agreementId);

            model.AgreementId = agreementDto.AgreementId;
            model.Client = new Models.ApplicationForm.Client { City = (!string.IsNullOrWhiteSpace(agreementDto.Client.City) ? agreementDto.Client.City : "-"), FIO = agreementDto.Client.FIO, INN = agreementDto.Client.INN, Region = (!string.IsNullOrWhiteSpace(agreementDto.Client.Region) ? agreementDto.Client.Region : "-") };
            model.Credit = new Models.ApplicationForm.Credit { AgreemNumber = agreementDto.Credit.AgreemNumber, Outstanding = agreementDto.Credit.Outstanding, DPD = agreementDto.Credit.DPD, Interest = agreementDto.Credit.Interest, Principal = agreementDto.Credit.Principal};
            model.Job = new Models.ApplicationForm.Job { Position = agreementDto.Job.Position, WorkPlace = agreementDto.Job.WorkPlace };
            model.Address = new Models.ApplicationForm.Address { RegistrationAddress = agreementDto.Address.RegistrationAddress, ResidentialAddress = agreementDto.Address.ResidentialAddress };
            model.Guarantors = agreementDto.Guarantors;

            var collateralsDto = await CollateralService.GetCollaterals(Convert.ToString(agreementId));
            var activesDto = await CollateralService.GetActives(Convert.ToString(agreementId));
            model.Collaterals = Mapper.Map<List<Model.DTO.ApplicationForm.Collateral>, List<Models.ApplicationForm.Collateral>>(collateralsDto);
            model.Actives = Mapper.Map<List<Model.DTO.ApplicationForm.Collateral>, List<Models.ApplicationForm.Collateral>>(activesDto);

            await InitializeLists(model);

            return View("CreateDebtSettlement", model);
        }

        [HttpGet]
        public async Task<ActionResult> CreateORK()
        {
            var model = new ApplicationFormViewModel();

            model.Client = new Client { MacroSegment = "12" , Portfolio = "Херня"};
            model.Finance = new Finance {TypeDS = "2"};
            model.Status = new Status { LegalStage = "3", CurrentTool = "2"};

            await InitializeLists(model);

            return View("ORKDeptSettlement", model);
        }

        [HttpGet]
        public ActionResult DSCreated()
        {
            return View("DSCreated");
        }

        [HttpGet]
        [Route("portfolioType")]
        public async Task<string> GetPortfolioType(string macroSegment)
        {
            var portfolio =
                await DictionaryService.GetDictionaryPortfolioSegmentByNameAsync("DS.PortfolioSegment", "1");
            var portfolioType = portfolio.Where(n => n.Id == macroSegment)?.FirstOrDefault();
            return new JavaScriptSerializer().Serialize(portfolioType);
        }

        public async Task InitializeLists(ApplicationFormViewModel model)
        {
            model.Job.SourceList = await PopulateDropDownList(DictionaryService, "DS.SourceInformation", model.Job.Source, "1");
            model.Credit.DSTypeList = await PopulateDropDownList(DictionaryService, "DS.TypeDS", model.Credit.DSType, "1");
            model.Liability.SourceOfOtherLiabilityList = await PopulateDropDownList(DictionaryService, "DS.SourceInformation", model.Liability.SourceOfOtherLiability, "1");
            model.Income.SourceInformationOfIncomesList = await PopulateDropDownList(DictionaryService, "DS.SourceInformation", model.Income.SourceInformationOfIncome, "1");
            model.ReasonToDenyDS.AbsenceOfDocumentsList = await PopulateDropDownList(DictionaryService, "DS.EstablishDoc", model.ReasonToDenyDS.AbsenceOfDocuments, "1");
            model.Address.MembershipInterestOnRegistrAddressList = await PopulateDropDownList(DictionaryService, "DS.PartProperty", model.Address.MembershipInterestOnRegistrAddress, "1");
            model.Address.MembershipInterestOnResidentAddressList = await PopulateDropDownList(DictionaryService, "DS.PartProperty", model.Address.MembershipInterestOnResidentAddress, "1");
            model.ActiveOtherParameters.SourceInformationOfActiveList = await PopulateDropDownList(DictionaryService, "DS.SourceInformation", model.ActiveOtherParameters.SourceInformationOfActive, "1");

            model.Finance.TypeDSList = await PopulateDropDownList(DictionaryService, "DS.TypeDS", model.Finance.TypeDS, "1");
            model.Status.LegalStageList = await PopulateDropDownList(DictionaryService, "DS.JurStage", model.Status.LegalStage, "1");
            model.Status.CurrentToolList = await PopulateDropDownList(DictionaryService, "DS.CurrentFunctional", model.Status.LegalStage, "1");
            model.Client.MacroSegmentList = await PopulateDropDownList(DictionaryService, "DS.PortfolioSegment", model.Client.MacroSegment, "1");
        }

        protected async Task<List<SelectListItem>> PopulateDropDownList(IDictionaryService _dictionaryService, string dictionary, string selectedValue, string version)
        {
            var blankItem = new SelectListItem { Text = "", Value = "" };
            var typeList = new List<SelectListItem> { blankItem };
            try
            {
                var Type = await _dictionaryService.GetDictionaryByNameAsync(dictionary, version);
                if (Type != null)
                {
                    typeList.AddRange(Type.Select(x => new SelectListItem { Text = x.Name, Value = x.Id, Selected = (selectedValue == x.Id) ? true : false }));
                }
            }
            catch (Exception ex)
            {
                throw new DebtSettlementException($"Failed to get dictionary: {dictionary}", _logger);
            }
            return typeList;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View("List");
        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// Lists the partial.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ListPartial()
        {
            return View("ListPartial");
        }

        /*
        
        #region Private members
        private readonly IAgreementService _agreementService;
        private readonly ILogger _logger;
        private readonly ITaskSpreadsheetReader _spreadsheetReader;
        private readonly IDepartmentService _departmentService;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskController" /> class.
        /// </summary>
        /// <param name="agreementService">The agreement service.</param>
        /// <param name="spreadsheetReader">The spreadsheet reader.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="departmentService">The Department service.</param>
        public TaskController(IAgreementService agreementService,
            ITaskSpreadsheetReader spreadsheetReader, ILogger logger, IDepartmentService departmentService)
        {
            _agreementService = agreementService;

            _spreadsheetReader = spreadsheetReader;

            _logger = logger;

            _departmentService = departmentService;
        }

       

      

        /// <summary>
        /// Items the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult Item(Guid id)
        {
            return View();
        }
        /// <summary>
        /// Histories the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult History(Guid id)
        {
            return View();
        }

        /// <summary>
        /// Creates the task.
        /// </summary>
        /// <returns>ViewResult.</returns>
        [HttpGet]
        public ViewResult CreateTask(int agreementId = -1, string agreementProcessId = null)
        {
            var model = new TaskViewModel
            {
                GeneralActionTypesList = GetGeneralActions(0),
                LegalActionTypesList = GetLegalActions(0),
                TaskCategoryList = GetDepartments(0),
                AgreementId = agreementId.ToString(),
                AgreementProcessId = agreementProcessId,
            };

            return View("CreateTask", model);
        }

        /// <summary>
        /// Tasks the created.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult TaskCreated()
        {
            return View("TaskCreated");
        }

        /// <summary>
        /// Uploads the file with agreements.
        /// </summary>
        /// <param name="agreementIds">The agreement ids.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult UploadFileWithAgreements(string agreementIds = null)
        {
            return View("UploadFileWithAgreements", (object)agreementIds);
        }

        /// <summary>
        /// Uploads the specified file with agreements.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.String.</returns>
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var model = new UploadViewModel
            {
                Success = false,
                Message = null,
                AgreementContent = null
            };

            if (Request.Files.Count <= 0 || file == null ||
                Request.Files[0]?.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                model.Message = "Файл не найден.";

                return View("UploadFileWithAgreements", model);
            }

            List<TaskCreationResultDTO> agreements = null;

            try
            {
                agreements = _spreadsheetReader.ReadTasksFromStream(file.InputStream);
            }
            catch (Exception ex)
            {
                _logger.Warning(
                    $"Unable to read agreements from spreadsheet file. {ex.Message} {GetInnerException(ex)}", ex);
            }

            if (agreements == null || !agreements.Any())
            {
                model.Message = "Некорректный формат файла.";

                return View("UploadFileWithAgreements", model);
            }

            var taskCreationResult = JsonConvert.SerializeObject(agreements);

            model.Success = true;
            model.Message = $"Загружено {agreements.Count} {Helper.GetDeclension(agreements.Count, "договор", "договора", "договоров")}.";
            model.AgreementContent = taskCreationResult;

            return View("UploadFileWithAgreements", model);
        }

        /// <summary>
        /// Gets the task creation result.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>ActionResult.</returns>
        [DeleteFile]
        public ActionResult GetTaskCreationResult(string fileName)
        {
            return File(System.Web.Hosting.HostingEnvironment.MapPath($"~/App_Data/{fileName}"), "application/excel");
        }

        #region Private members
        /// <summary>
        /// Gets the inner exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>System.String.</returns>
        private static string GetInnerException(Exception exception)
        {
            var message = string.Empty;

            if (exception.InnerException != null)
            {
                message += $" {exception.InnerException.Message} {GetInnerException(exception.InnerException)}";
            }

            return message;
        }

        /// <summary>
        /// Gets the general actions.
        /// </summary>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns>List&lt;SelectListItem&gt;.</returns>
        private List<SelectListItem> GetGeneralActions(int selectedValue)
        {
            return _agreementService.GetGeneralActions().ToSelectListItems(selectedValue).ToList();
        }

        /// <summary>
        /// Gets the legal actions.
        /// </summary>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns>List&lt;SelectListItem&gt;.</returns>
        private List<SelectListItem> GetLegalActions(int selectedValue)
        {
            var tst = _agreementService.GetLegalActions();

            return _agreementService.GetLegalActions().ToSelectListItems(selectedValue).Where(x => x.Value != "10000").ToList();
        }

        /// <summary>
        /// Gets all departments.
        /// </summary>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns>List&lt;SelectListItem&gt;.</returns>
        private List<SelectListItem> GetDepartments(int selectedValue)
        {
            return _departmentService.GetAllDepartments().Select(t => new ActionType
            {
                Id = t.Id,
                Name = t.Name
            }).ToSelectListItems(selectedValue).ToList();
        }
        #endregion

        */
    }
}