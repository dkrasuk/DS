using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AlfaBank.Common.Interfaces;
using HR.Client.Interface;
using DebtSettlement.BusinessLayer.Services.Interfaces;
using DebtSettlement.Web.Attributes;
using DebtSettlement.Web.Models;
using DebtSettlement.Model.Dictionary;

namespace DebtSettlement.Web.Controllers.Api
{
    /// <summary>
    /// Class DictionaryController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/dictionary")]
    public class DictionaryController : ApiController
    {
        private readonly IApiExecuter _apiExecuter;
        private readonly IHRService _hrService;
        private readonly IDepartmentService _departmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryController"/> class.
        /// </summary>
        /// <param name="apiExecuter">The API executer.</param>
        /// <param name="hrService">The hr service.</param>
        /// <param name="departmentService"></param>
        public DictionaryController(
            IApiExecuter apiExecuter, IHRService hrService, IDepartmentService departmentService)
        {
            _apiExecuter = apiExecuter;
            _hrService = hrService;
            _departmentService = departmentService;
        }


        [HttpGet]
        [Route("users")]
        [ODataQueryable]
        private async Task<HttpResponseMessage> GetUsersDictionary(string term = null)
        {
            var result = await Task.Run<IEnumerable<object>>(
                () => _hrService.FindUsers(term).Where(r => r.IsBlock == 0)
                    .Select(t => new
                    {
                        id = t.Login,
                        text = $"{t.FullName} / {t.Login}",
                        DepartmentId = t.DepartmentId
                    }));
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("departments")]
        [ODataQueryable]
        private HttpResponseMessage GetDepartaments(string term = null)
        {
            var list = _departmentService.GetAllDepartments();
            if (!string.IsNullOrEmpty(term))
            {
                list = list.Where(i => i.Id.ToString().Contains(term) || i.Name.Contains(term));
            }
            var result = list.Select(t => new
            {
                id = t.Id,
                text = t.Name
            });
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Get dictionary info by dictionamry name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}")]
        [ODataQueryable]
        public async Task<HttpResponseMessage> Get(string name, string term = null)
        {
            return await Get(name, "1", term);
        }

        /// <summary>
        /// Get dictionary info by dictionamry name and version
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}/{version}")]
        [ODataQueryable]
        public async Task<HttpResponseMessage> Get(string name, string version, string term = null)
        {
            term = term ?? string.Empty;
            if (name.ToLower() == "users")
            {
                return await GetUsersDictionary(term);
            }

            if (name.ToLower() == "departments")
            {
                return GetDepartaments(term);
            }

            var dictionaryUrl = BusinessLayer.AppSettings.DictionaryAPIAddress;
            var dictionary = await _apiExecuter.ExecuteAsync<Dictionary>(dictionaryUrl + name + $"/{version}");
            var result = dictionary?.Items?.Select(i => new { id = i.ValueId, text = i.Value?.Name });

            if (!string.IsNullOrWhiteSpace(term))
            {
                result = result?.Where(i => i.id.IndexOf(term) > -1 || i.text.IndexOf(term) > -1);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }        
        
    }
}
