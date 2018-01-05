using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using DebtSettlement.AgreementLoader.DataAccess;
using DebtSettlement.AgreementLoader.Interface;
using DebtSettlement.AgreementLoader.Models;
using HR.Client.Interface;
using CommonServiceLocator;
using AlfaBank.Common.Utils;
using DebtSettlement.Model.DTO.ApplicationForm;

namespace DebtSettlement.AgreementLoader
{
    public class AgreementService : IAgreementService
    {
        private static Dictionary<int, List<string>> _responsiblePersonsCache;

        private static Dictionary<int, List<string>> _responsiblePersonsForAgreementProcessesCache;

        private static readonly IUserService UserService;

        private static DateTime _lastResponsiblePersonsCacheUpdate = default(DateTime);

        private static TimeSpan UsersUpdatePeriod => TimeSpan.FromMilliseconds(AppSettings.DataUpdatePeriod);

        static AgreementService()
        {
            _responsiblePersonsCache = new Dictionary<int, List<string>>();

            _responsiblePersonsForAgreementProcessesCache = new Dictionary<int, List<string>>();

            //  UserService = ServiceLocator.Current.GetInstance<IUserService>();
        }

        /// <summary>
        /// GetApplicationFormFilling
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        public async Task<ApplicationForm> GetApplicationFormFilling(int agreementId)
        {
            using (var context = new CollectSmContext())
            {

                var form = context.TransitAgreems
                    .Include("TransitPersons")
                    .Include("TransitContactsAdress")
                    .Include("DictionaryRegions")
                    .Include("TransitDelinquency")
                    .Where(t => t.AgreementId == agreementId)
                    .Select(t => new ApplicationForm
                    {
                        AgreementId = agreementId,
                        Client = new Client() { INN = t.Person.Inn, FIO = t.Person.Fio, Region = t.Person.ContactsAdress.FirstOrDefault().Region.Region, City = t.Person.ContactsAdress.FirstOrDefault().Settlement, PersonId = t.Person.ContactsAdress.FirstOrDefault().PersonId },
                        Credit = new Credit() {Outstanding = t.Outstanding, DPD = t.Delinquency.DPD, Interest = t.CurrentInterest, PurchasePrice = t.CurrentInterest}
                        
                    }).FirstOrDefault();

                var listAgreems = context.TransitAgreems.Where(t => t.PersonId == form.Client.PersonId).Select(n => n.AgreementId).ToList();
                form.Credit.AgreemNumber.AddRange(listAgreems);

                var guarantors = context.TransitLinkedPersons
                   .Include("TransitPersons")
                   .Where(t => t.AgreemId == agreementId)
                   .Select(t => t.Persons.Fio).ToList();
                form.Guarantors = (guarantors.Count() > 0) ? guarantors.Aggregate((a, b) => a + ", " + b) : "Нет";

                await GetWorkAddress(form, context);
                await GetClientAddresses(form, context);

                return form;
            }
        }

        /// <summary>
        /// GetWorkAddress
        /// </summary>
        /// <param name="form"></param>
        /// <param name="context"></param>
        private async Task GetWorkAddress(ApplicationForm form, CollectSmContext context)
        {
            await Task.Run(() =>
            {
                const string query = "SELECT * FROM TABLE(pcg_alfacollection.getclientbyagreementid(:AgreementId)) ";
                var parameter = new OracleParameter(":AgreementId", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = form.AgreementId
                };

                var address = context.Database.SqlQuery<JobAddress>(query, parameter).FirstOrDefault();

                if (address != null)
                {
                    string[] workAddr = address.Work.Split(',');
                    if (workAddr.Length > 1)
                    {
                        form.Job = new Job()
                        {
                            WorkPlace = workAddr[0].Trim(),
                            Position = workAddr[1].Trim()
                        };
                    }
                    else
                    {
                        form.Job = new Job()
                        {
                            WorkPlace = workAddr[0].Trim()
                        };
                    }
                }
            });
        }
        /// <summary>
        /// GetClientAddresses
        /// </summary>
        /// <param name="form"></param>
        /// <param name="context"></param>
        private async Task GetClientAddresses(ApplicationForm form, CollectSmContext context)
        {
            await Task.Run(() =>
            {
                const string query = "SELECT * FROM TABLE((pcg_alfacollection.getAddressesByAgreementId(:AgreementId)))";
                var parameter = new OracleParameter(":AgreementId", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = form.AgreementId
                };
                var answer = context.Database.SqlQuery<ClientAddresses>(query, parameter).ToList()
                    .Where(n => n.Owner == "Client");
                var regAddr = answer?.Where(n => n.Type == "Registration" && n.Correct == "100").OrderByDescending(n => n.Id)?.FirstOrDefault();
                var resAddr = answer?.Where(n => n.Type == "Home" && n.Correct == "100").OrderByDescending(n => n.Id)?.FirstOrDefault();
                form.Address = new Address()
                {
                    RegistrationAddress = (regAddr != null) ? $"{regAddr.Region}, {regAddr.Settlement}, {regAddr.Street}, {regAddr.House}, {regAddr.Flat}" : string.Empty,
                    ResidentialAddress = (resAddr != null) ? $"{resAddr.Region}, {resAddr.Settlement}, {resAddr.Street}, {resAddr.House}, {resAddr.Flat}" : string.Empty
                };
            });
        }

        /// <summary>
        /// Gets the agreement.
        /// </summary>
        /// <param name="agreementId">The agreement identifier.</param>
        /// <returns>Agreement.</returns>
        public Agreement GetAgreement(int agreementId)
        {
            using (var context = new CollectSmContext())
            {
                var agreement =
                    context.TransitAgreems.Where(t => t.AgreementId == agreementId).Select(t => new Agreement
                    {
                        Id = t.AgreementId,
                        Number = t.AgreementNumber,
                        PersonFullName = t.Person.Fio,
                        ProductCode = t.ProductCode.ProductCode,
                        OpenDate = t.OpenDate,
                        CloseDate = t.PlanedCloseDate,
                        AssignedCollector = t.OperWorkflow.AssignedCollector,
                        AssignedFieldUser = t.OperWorkflow.AssignedFieldUser,
                        AssignedLegalUser = t.OperWorkflow.AssignedLegalUser
                    }).FirstOrDefault();

                if (agreement == null)
                {
                    return null;
                }

                var assignedCollector = agreement.AssignedCollector?.FormatUserName();
                var assignedFieldUser = agreement.AssignedFieldUser?.FormatUserName();
                var assignedLegalUser = agreement.AssignedLegalUser?.FormatUserName();

                agreement.AssignedCollectorFullName = GetFullNameOfNotBlockedUser(assignedCollector);
                agreement.AssignedCollector = string.IsNullOrWhiteSpace(agreement.AssignedCollectorFullName)
                    ? null
                    : assignedCollector;

                agreement.AssignedFieldUserFullName = GetFullNameOfNotBlockedUser(assignedFieldUser);
                agreement.AssignedFieldUser = string.IsNullOrWhiteSpace(agreement.AssignedFieldUserFullName)
                    ? null
                    : assignedFieldUser;

                agreement.AssignedLegalUserFullName = GetFullNameOfNotBlockedUser(assignedLegalUser);
                agreement.AssignedLegalUser = string.IsNullOrWhiteSpace(agreement.AssignedLegalUserFullName)
                    ? null
                    : assignedLegalUser;

                FillProcesses(agreement, context);

                return agreement;
            }
        }

        private static void FillProcesses(Agreement agreement, CollectSmContext context)
        {
            var processes = new List<ProcessDTO>();

            foreach (var process in context.Process.Where(t => t.AgreementId == agreement.Id))
            {
                var responsible = UserService.SearchUsers(process.Responsible?.FormatUserName()).FirstOrDefault();

                processes.Add(new ProcessDTO
                {
                    Id = process.Id,
                    Responsible = responsible != null && responsible.IsBlock == 0 ? responsible.Login : null
                });
            }

            agreement.Processes = processes;
        }


        private string GetFullNameOfNotBlockedUser(string userLogin)
        {
            if (string.IsNullOrWhiteSpace(userLogin))
            {
                return null;
            }

            var searchResult = UserService.SearchUsers(userLogin?.FormatUserName()).FirstOrDefault();

            if (searchResult != null && searchResult.IsBlock == 0)
            {
                return $"{searchResult.FullName} / {userLogin}";
            }

            return null;
        }

        /// <summary>
        /// Gets the general actions.
        /// </summary>
        /// <returns>IEnumerable&lt;ActionType&gt;.</returns>
        public IEnumerable<ActionType> GetGeneralActions()
        {
            using (var context = new CollectSmContext())
            {
                return context.DictionaryHistoryAction.Select(t => new ActionType
                {
                    Id = t.Id,
                    Name = t.ActionType
                }).ToList();
            }
        }

        /// <summary>
        /// Gets the legal actions.
        /// </summary>
        /// <returns>IEnumerable&lt;ActionType&gt;.</returns>
        public IEnumerable<ActionType> GetLegalActions()
        {
            using (var context = new ProcessManagerContext())
            {
                return context.DictionaryActionType.Select(t => new ActionType
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList();
            }
        }

        /// <summary>
        /// Gets the responsible persons by agreement.
        /// </summary>
        /// <param name="agreementId">The agreement identifier.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public IEnumerable<string> GetResponsiblePersonsByAgreement(int agreementId)
        {
            TryToUpdateCache();

            if (_responsiblePersonsCache.ContainsKey(agreementId))
            {
                return _responsiblePersonsCache[agreementId];
            }

            FindAbsentRecord(agreementId);

            return _responsiblePersonsCache[agreementId];
        }

        /// <summary>
        /// Gets the responsible persons for agreement processes.
        /// </summary>
        /// <param name="agreementId">The agreement identifier.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public IEnumerable<string> GetResponsiblePersonsByAgreementProcesses(int agreementId)
        {
            TryToUpdateCache();

            if (_responsiblePersonsForAgreementProcessesCache.ContainsKey(agreementId))
            {
                return _responsiblePersonsForAgreementProcessesCache[agreementId];
            }

            FindAbsentRecord(agreementId);

            return _responsiblePersonsForAgreementProcessesCache[agreementId];
        }

        /// <summary>
        /// Gets the responsible person by process.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        /// <returns>System.String.</returns>
        public string GetResponsiblePersonByProcess(int processId)
        {
            using (var context = new ProcessManagerContext())
            {
                return
                    context.Processes.FirstOrDefault(t => t.Id == processId)?
                        .Responsible;
            }
        }

        /// <summary>
        /// Searches the agreements.
        /// </summary>
        /// <param name="searcherLogin">The searcher login.</param>
        /// <param name="searhTerm">The searh term.</param>
        /// <returns>IEnumerable&lt;SearchResult&gt;.</returns>
        public async Task<IEnumerable<SearchResult>> SearchAgreements(string searcherLogin, string searhTerm)
        {
            using (var context = new CollectSmContext())
            {
                const string query = "select * from table(search.getSearchResEng(:SearchWord))";

                var parameter = new OracleParameter(":SearchWord", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = searhTerm + searcherLogin
                };

                return await context.Database.SqlQuery<SearchResult>(query, parameter).ToListAsync();
            }
        }

        private static void FindAbsentRecord(int agreementId)
        {
            using (var context = new CollectSmContext())
            {
                var agreement = context.OperWorkflow.FirstOrDefault(t => t.AgreementId == agreementId);

                if (agreement == null)
                {
                    _responsiblePersonsCache[agreementId] = new List<string>();
                    _responsiblePersonsForAgreementProcessesCache[agreementId] = new List<string>();

                    return;
                }

                _responsiblePersonsCache[agreementId] = new List<string>
                {
                    agreement.AssignedCollector?.FormatUserName(),
                    agreement.AssignedFieldUser?.FormatUserName(),
                    agreement.AssignedLegalUser?.FormatUserName()
                };


                List<string> _responsiblePersonsForAgreementProcesses = new List<string>();

                var processes = context.Process.Where(t => t.AgreementId == agreementId).ToList();

                foreach (Process process in processes)
                {
                    string responsiblePerson = process.Responsible?.FormatUserName();

                    if (!String.IsNullOrWhiteSpace(responsiblePerson))
                    {
                        _responsiblePersonsForAgreementProcesses.Add(responsiblePerson);
                    }
                }

                _responsiblePersonsForAgreementProcessesCache[agreementId] = _responsiblePersonsForAgreementProcesses;
            }
        }

        private static void TryToUpdateCache()
        {
            if (DateTime.Now - _lastResponsiblePersonsCacheUpdate <= UsersUpdatePeriod)
            {
                return;
            }

            _lastResponsiblePersonsCacheUpdate = DateTime.Now;

            _responsiblePersonsCache = new Dictionary<int, List<string>>();

            _responsiblePersonsForAgreementProcessesCache = new Dictionary<int, List<string>>();
        }
    }
}
