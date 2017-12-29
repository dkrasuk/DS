using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DebtSettlement.BusinessLayer.Services.Interfaces;
using DebtSettlement.Model.DTO;

namespace DebtSettlement.Web.Controllers.Api
{
    /// <summary>
    /// Class UsersController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IHRService _hrService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="hrService">The hr service.</param>
        public UsersController(IHRService hrService)
        {
            _hrService = hrService;
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        [Route("")]
        [HttpGet]
        public async Task<IEnumerable<object>> GetUsers(string term)
        {
            return await Task.Run<IEnumerable<object>>(
                () =>
                    _hrService.FindUsers(term)
                        .Where(r => r.IsBlock == 0)
                        .Select(t => new {label = $"{t.FullName} / {t.Login}"}));
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        [Route("{userName}")]
        [HttpGet]
        public async Task<UserDTO> GetUser(string userName)
        {
            return await Task.Run(() =>
            {
                var user = _hrService.GetUserByLogin(userName);

                return user != null && user.IsBlock == 0
                    ? user
                    : null;
            });
        }


        /// <summary>
        /// Gets the boss of user.
        /// </summary>
        /// <param name="userLogin">The user login.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        [Route("GetBoss")]
        [HttpGet]
        public async Task<UserDTO> GetBoss(string userLogin)
        {
            var user = await Task.Run(() => _hrService.FindUsers(userLogin.Trim()).FirstOrDefault(t => t.IsBlock == 0));

            if (user == null)
            {
                return null;
            }

            var departmentId = user.DepartmentId;

            return await Task.Run(() =>
            {
                var departmentBoss = _hrService.GetBossByDepartment(departmentId);

                return departmentBoss != null && departmentBoss.IsBlock == 0
                    ? departmentBoss
                    : null;
            });
        }
    }
}
