using AutoMapper;
using AlfaBank.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using DebtSettlement.AgreementLoader.Interface;
using DebtSettlement.AgreementLoader.Models;
using DebtSettlement.BusinessLayer;
using DebtSettlement.BusinessLayer.Models;
using DebtSettlement.BusinessLayer.Services.Interfaces;
using DebtSettlement.Model.DTO;
using DebtSettlement.Model.Enums;
using DebtSettlement.Web.Attributes;
using DebtSettlement.Web.Models;
using DebtSettlement.Web.Models.Task;
using WorkflowEngine.Client.Models;
using WorkflowEngine.Client.Models.WorkflowProcess;
using AlfaBank.Common.Utils;

namespace DebtSettlement.Web.Controllers.Api
{
    /// <summary>
    /// Class TasksController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    //[Authorize]
    //[ApiAuthorizeFilter(Role = Common.Constants.Roles.DebtSettlementUser)]
    [RoutePrefix("api/tasks")]
    public class TasksController : ApiController
    {
        private readonly IDebtSettlementService _taskService;
        private readonly IHRService _hrService;
        private readonly IAgreementService _agreementService;
        private readonly ILogger _logger;
        //private readonly ITaskSpreadsheetWriter _spreadsheetWriter;
        
        private const string SuccessCreationResult = "Success";
        private const string ErrorCreationResult = "Error";
        private const string AutomaticValue = "auto";

        /// <summary>
        /// Initializes a new instance of the <see cref="TasksController" /> class.
        /// </summary>
        /// <param name="taskService">The task service.</param>
        /// <param name="hrService">The hr service.</param>
        /// <param name="agreementService">The agreement service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="spreadsheetWriter">The spreadsheet writer.</param>
        public TasksController(
            IDebtSettlementService taskService,
            IHRService hrService,
            IAgreementService agreementService,
            ILogger logger)
            //ITaskSpreadsheetWriter spreadsheetWriter)
        {
            _taskService = taskService;
            _hrService = hrService;
            _agreementService = agreementService;
            _logger = logger;
            //_spreadsheetWriter = spreadsheetWriter;
        }
        /*
        /// <summary>
        /// Get all tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        //[ODataQueryable]
        public async Task<IEnumerable<TaskDTO>> GetAll()
        {
            var userName = RequestContext.Principal.Identity.Name.FormatUserName();
            
            var odataFilter = Request.RequestUri.Query.Replace("::username", "'" + userName + "'");
            var myFilter = $"Initiator eq '{userName}' or Observer eq '{userName}' or Responsible eq '{userName}'";

            if (odataFilter.IndexOf("$filter", StringComparison.Ordinal) > -1)
            {
                var queryArray = odataFilter.Split('&');
                var filter = queryArray.Select(i =>
                {
                    if (i.StartsWith("$filter=") || i.StartsWith("?$filter="))
                    {
                        i = i + " and (" + myFilter + ")";
                    }
                    return i;
                });
                odataFilter = string.Join("&", filter);
            }
            else
            {
                odataFilter += "&$filter=" + myFilter;
            }

            return await _taskService.GetAllTasks(odataFilter);
        }

        /// <summary> 
        /// Get task by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ODataQueryable]
        public async Task<TaskDTO> Get(Guid id)
        {
            return await _taskService.GetTaskById(id);
        }
        /// <summary>
        /// Get all tasks assigned on current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("my")]
        //[ODataQueryable]
        public async Task<IEnumerable<TaskDTO>> GetAllMy()
        {
            var userName = RequestContext.Principal.Identity.Name.FormatUserName();

            var odataFilter = Request.RequestUri.Query.Replace("::username", "'" + userName + "'");
            var myFilter = $"Responsible eq '{userName}'";
            
            if (odataFilter.IndexOf("$filter", StringComparison.Ordinal) > -1)
            {
                var queryArray = odataFilter.Split('&');
                var filter = queryArray.Select(i =>
                {
                    if (i.StartsWith("$filter=") || i.StartsWith("?$filter="))
                    {
                        i = i + " and " + myFilter;
                    }
                    return i;
                });
                odataFilter = string.Join("&", filter);
            }
            else
            {
                odataFilter += "&$filter=" + myFilter;
            }


            return await _taskService.GetAllTasks(odataFilter);
        }
        /// <summary>
        /// Gets the users with responsible.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="agreementId">The agreement identifier.</param>
        /// <param name="departmentId">The department identifier.</param>
        /// <returns>IEnumerable&lt;System.Object&gt;.</returns>
        [Route("GetUsersWithResponsible")]
        [HttpGet]
        public IEnumerable<object> GetUsersWithResponsible(string term, string agreementId, string departmentId)
        {
            int.TryParse(departmentId, out var departmentIdInt);

            var result = new List<dynamic>();

            var responsiblePersons = new List<UserDTO>();

            var responsiblePersonsByAgreementProcesses = new List<UserDTO>();

            var findedUsersByTerm = _hrService.FindUsers(term).Where(t => t.IsBlock == 0);

            if (int.TryParse(agreementId, out var agreementIdInt))
            {
                var availableResponsibles = _agreementService.GetResponsiblePersonsByAgreement(agreementIdInt)
                    .Where(responsiblePersonFullName => responsiblePersonFullName != null).ToList();

                foreach (var responsible in availableResponsibles)
                {
                    var user = _hrService.GetUserByLogin(responsible);

                    if (user != null && user.IsBlock == 0 &&
                        (user.DepartmentId == departmentIdInt || departmentIdInt == 0))
                    {
                        responsiblePersons.Add(user);
                    }
                }

                var responsibleByProcesses = _agreementService.GetResponsiblePersonsByAgreementProcesses(agreementIdInt).ToList();

                foreach (var responsible in responsibleByProcesses)
                {
                    var user = _hrService.GetUserByLogin(responsible);

                    if (user != null && user.IsBlock == 0 && (user.DepartmentId == departmentIdInt || departmentIdInt == 0))
                    {
                        responsiblePersonsByAgreementProcesses.Add(user);
                    }
                }

                if (responsibleByProcesses.Any())
                {
                    responsiblePersonsByAgreementProcesses = responsiblePersonsByAgreementProcesses
                        .Where(t => !findedUsersByTerm.Contains(t) && !findedUsersByTerm.Contains(t)).ToList();
                }
            }

            findedUsersByTerm = findedUsersByTerm
                .Where(t => !responsiblePersons.Contains(t) && !responsiblePersonsByAgreementProcesses.Contains(t))
                .Where(t => t.DepartmentId == departmentIdInt || departmentIdInt == 0);
            
            foreach (var user in findedUsersByTerm)
            {
                result.Add(new { label = $"{user.FullName} / {user.Login}", category = "" });
            }

            foreach (var user in responsiblePersons)
            {
                result.Add(new { label = $"{user.FullName} / {user.Login}", category = "Ответственные по договору" });
            }

            foreach (var user in responsiblePersonsByAgreementProcesses)
            {
                result.Add(new { label = $"{user.FullName} / {user.Login}", category = "Ответственные по процессам договора" });
            }

            return result;
        }


        /// <summary>
        /// Duplicate task by id
        /// </summary>
        /// <param name="id">Duplicate task id</param>
        /// <returns></returns>
        [Route("{id}/duplicate")]
        [HttpPut]
        public async Task<IHttpActionResult> Duplicate(Guid id)
        {
            var newId = await _taskService.Duplicate(id);

            return Ok(new { id = newId });
        }

        /// <summary>
        /// Set new task state
        /// </summary>
        /// <param name="id">Task id</param>
        /// <param name="state">State model</param>
        /// <returns></returns>
        [Route("{id}/states")]
        [HttpPut]
        public async Task<IHttpActionResult> PutStates(Guid id, TaskStateModel state)
        {
            await _taskService.SetNewWorkflowState(id, state.StateName, state.Comment);

            return Ok();
        }

        /// <summary>
        /// Get history for task instance
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/history")]
        public async Task<List<ProcessHistoryItem>> GetProcessHistory(Guid id)
        {
            return await _taskService.GeTaskHistoryAsync(id);
        }

        /// <summary>
        /// Get history for task instance
        /// </summary>
        /// <param name="id"></param>
        /// <param name="historyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/history/{historyId}")]
        public async Task<ProcessHistoryItem> GetProcessHistory(Guid id, Guid historyId)
        {
             return await _taskService.GeTaskHistoryItemAsync(id, historyId);
        }

        /// <summary>
        /// Close tack by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await _taskService.Close(id, new Dictionary<string, object>());

            return Ok();
        }

        /// <summary>
        /// Auto close task
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [Route("autoClose")]
        [HttpPost]
        public async Task<IHttpActionResult> AutoClose(AgreementAction action)
        {
            await _taskService.AutoClose(action);

            return Ok();
        }

        /// <summary>
        /// Get available commands by task id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [Route("{taskId}/commands")]
        [HttpGet]
        public async Task<List<WorkflowCommand>> GetAvailableCommands(Guid taskId)
        {
            var userName = User.Identity.Name.FormatUserName();
            return await _taskService.GetAvailableCommandsAsync(taskId, userName);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="commandParams">The command parameters.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [Route("{taskId}/commands")]
        [HttpPut]
        public async Task<IHttpActionResult> ExecuteCommand(Guid taskId, ExecuteCommandModel commandParams)
        {
            commandParams.TaskId = taskId;
            var userName = User.Identity.Name.FormatUserName();
            await _taskService.ExecuteWorkflowCommandAsync(
                    taskId, commandParams.CommandName, userName, commandParams.Parameters);
            return Ok();
        }

        /// <summary>
        /// Creates the task.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ViewResult&gt;.</returns>
        [Route("")]
        [HttpPost]
        public async Task<string> CreateTask(TaskViewModel model)
        {
            var creationResult = await TryToCreateTask(model);

            if (!model.IsMultiTask && creationResult.First().Result != SuccessCreationResult)
            {
                throw new DebtSettlementException(creationResult.First().Description, _logger);
            }

            return string.Empty;
        }

        /// <summary>
        /// Creates the multiple task and get result.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        [Route("CreateMultipleTaskAndGetResult")]
        [HttpPost]
        public async Task<string> CreateMultipleTaskAndGetResult(TaskViewModel model)
        {
            var creationResult = await TryToCreateTask(model);

            var resultExcelFileName = $"{Guid.NewGuid()}.xlsx";

            var fileStream =
                new FileStream(System.Web.Hosting.HostingEnvironment.MapPath($"~/App_Data/{resultExcelFileName}"),
                    FileMode.Create);

            //_spreadsheetWriter.WriteTasksCreationResults(fileStream, creationResult);

            fileStream.Dispose();

            return resultExcelFileName;
        }

        /// <summary>
        /// Tries to create task.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;List&lt;TaskCreationResultDTO&gt;&gt;.</returns>
        private async Task<List<TaskCreationResultDTO>> TryToCreateTask(TaskViewModel model)
        {
            try
            {
                if (!model.IsMultiTask)
                {
                    return new List<TaskCreationResultDTO> {await CreateSingleTask(model)};
                }

                return await TryToCreateMultipleTasks(model);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message} {GetInnerException(ex)}", ex);

                throw;
            }
        }

        private async Task<TaskCreationResultDTO> CreateSingleTask(TaskViewModel model)
        {
            var modelDto = Mapper.Map<TaskViewModel, TaskDTO>(model);

            try
            {
                var preparationResult = _taskService.ValidateSingleCreationTask(modelDto,
                    model.AssignedResponsible);

                var taskCreationResult = await CreateTaskAndReturnResult(modelDto, true);

                taskCreationResult.Description = preparationResult;

                taskCreationResult.AgreementId = Convert.ToInt32(model.AgreementId);

                return taskCreationResult;
            }
            catch (Exception ex)
            {
                return new TaskCreationResultDTO
                {
                    AgreementId = Convert.ToInt32(model.AgreementId),
                    Result = ErrorCreationResult,
                    Description = $"{ex.Message} {GetInnerException(ex)}"
                };
            }
        }

        /// <summary>
        /// Tries to create multiple tasks.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;List&lt;TaskCreationResultDTO&gt;&gt;.</returns>
        private async Task<List<TaskCreationResultDTO>> TryToCreateMultipleTasks(TaskViewModel model)
        {
            var result = new List<TaskCreationResultDTO>();

            var agreementContent = JsonConvert.DeserializeObject<List<TaskCreationResultDTO>>(model.AgreementId);

            foreach (var agreement in agreementContent)
            {
                model.AgreementId = agreement.AgreementId.ToString();

                try
                {
                    var modelDto = Mapper.Map<TaskViewModel, TaskDTO>(model);

                    modelDto.AgreementId = agreement.AgreementId;

                    modelDto.AgreementProcessId = agreement.ProcessId;

                    TryToSetAutomaticResponsible(modelDto, model, agreement);

                    TryToSetObserverAsBossOfDepartment(modelDto, model);

                    var preparationResult = _taskService.ValidateMultipleCreationTask(modelDto,
                        model.AssignedResponsible);

                    var taskCreationResult = await CreateTaskAndReturnResult(modelDto, false);

                    taskCreationResult.Description = preparationResult;

                    taskCreationResult.AgreementId = agreement.AgreementId;

                    result.Add(taskCreationResult);
                }
                catch (Exception ex)
                {
                    result.Add(new TaskCreationResultDTO
                    {
                        AgreementId = agreement.AgreementId,
                        ProcessId = agreement.ProcessId,
                        Result = ErrorCreationResult,
                        Description = $"{ex.Message} {GetInnerException(ex)}"
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Tries to set observer as boss of department.
        /// </summary>
        /// <param name="modelDto">The model dto.</param>
        /// <param name="model">The model.</param>
        private void TryToSetObserverAsBossOfDepartment(TaskDTO modelDto, TaskViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Observer) || modelDto.Responsible == null)
            {
                return;
            }

            var user = _hrService.GetUserByLogin(modelDto.Responsible);

            if (user == null || user.IsBlock != 0)
            {
                return;
            }

            var departmentId = user.DepartmentId;

            var departmentBoss = _hrService.GetBossByDepartment(departmentId);

            modelDto.Observer = departmentBoss != null && departmentBoss.IsBlock == 0
                ? departmentBoss.Login
                : null;
        }

        /// <summary>
        /// Tries to set automatic responsible.
        /// </summary>
        /// <param name="modelDto">The model dto.</param>
        /// <param name="model">The model.</param>
        /// <param name="agreement">The agreement.</param>
        private void TryToSetAutomaticResponsible(TaskDTO modelDto, TaskViewModel model, TaskCreationResultDTO agreement)
        {
            if (model.Responsible != AutomaticValue)
            {
                return;
            }

            var currentAgreement = _agreementService.GetAgreement(agreement.AgreementId);

            switch (model.AssignedResponsible)
            {
                case AssignedUser.RS:
                    modelDto.Responsible = currentAgreement.AssignedCollector;
                    break;
                case AssignedUser.Field:
                    modelDto.Responsible = currentAgreement.AssignedFieldUser;
                    break;
                case AssignedUser.Legal:
                    modelDto.Responsible = GetLegalResponsible(agreement, currentAgreement);
                    break;
                case AssignedUser.Custom:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetLegalResponsible(TaskCreationResultDTO agreement, Agreement currentAgreement)
        {
            int processId;

            if (string.IsNullOrEmpty(agreement.ProcessId)
                || agreement.ProcessId == "-1"
                || !int.TryParse(agreement.ProcessId, out processId))
            {
                if (string.IsNullOrWhiteSpace(currentAgreement.AssignedLegalUser))
                {
                    throw new ApplicationException(
                        $"Agreement {agreement.AgreementId} " +
                        "with automatic legal assignment has no assigned legal responsible or he is blocked");
                }

                return currentAgreement.AssignedLegalUser;
            }
            var processReponsible = _agreementService.GetResponsiblePersonByProcess(processId);

            if (string.IsNullOrWhiteSpace(processReponsible) ||
                _hrService.GetUserByLogin(processReponsible).IsBlock == 1)
            {
                throw new ApplicationException(
                    $"Process {agreement.ProcessId} has no responsible or he is blocked");
            }

            return processReponsible;
        }

        /// <summary>
        /// Creates the single task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="sendEmail"></param>
        /// <returns>Task&lt;TaskCreationResultDTO&gt;.</returns>
        private async Task<TaskCreationResultDTO> CreateTaskAndReturnResult(TaskDTO task, bool sendEmail = false)
        {
            await _taskService.CreateTask(task, sendEmail);

            return new TaskCreationResultDTO
            {
                AgreementId = task.AgreementId ?? -1, 
                Result = SuccessCreationResult, 
                ProcessId = task.AgreementProcessId
            };
        }

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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SendOverduedNotifications")]
        public async Task<IHttpActionResult> SendOverduedNotifications(string userName)
        {
            userName = userName?.FormatUserName();

            var odataFilter = $"?$filter=Status ne 'Closed' and PlannedDate lt Date.Now and Responsible eq '" + userName + "'";
            
            await _taskService.SendOverduedNotifications(userName, odataFilter);

            return Ok();
        }
        */
    }
    
}