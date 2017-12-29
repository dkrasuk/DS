using HR.Client.Interface;
using AlfaBank.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using DebtSettlement.BusinessLayer.Services.Interfaces;
using DebtSettlement.Model.DTO;

namespace DebtSettlement.BusinessLayer.Services
{
    /// <summary>
    /// Class HRService.
    /// </summary>
    /// <seealso cref="DebtSettlement.BusinessLayer.Interface.IHRService" />
    public class HRService : IHRService
    {
        #region Private members
        private static DateTime _lastUsersUpdate = default(DateTime);

        private static TimeSpan UsersUpdatePeriod => TimeSpan.FromMilliseconds(AppSettings.DataUpdatePeriod);

        private static List<UserDTO> _usersList;

        private static ILogger _logger;

        private readonly IUserService _userService;

        private readonly object _syncObj = string.Empty;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HRService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="userService">The user service.</param>
        public HRService(ILogger logger, IUserService userService)
        {
            _logger = logger;

            _userService = userService;
        }

        /// <summary>
        /// Determines whether [is user login exist] [the specified user login].
        /// </summary>
        /// <param name="userLogin">The user login.</param>
        /// <returns><c>true</c> if [is user login exist] [the specified user login]; otherwise, <c>false</c>.</returns>
        public bool IsUserLoginExist(string userLogin)
        {
            TryToUpdateUsersList();

            return _usersList.Any(t => t.Login.ToLower().Trim() == userLogin.Trim().ToLower());
        }

        /// <summary>
        /// Finds the user.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>IEnumerable&lt;UserDTO&gt;.</returns>
        public IEnumerable<UserDTO> FindUsers(string searchTerm)
        {
            TryToUpdateUsersList();

            return _usersList.Where(t => t.FullName.ToLower().Contains(searchTerm.ToLower()) ||
                                         t.Login.ToLower().Contains(searchTerm.ToLower()));
        }
        public UserDTO GetUserByLogin(string login)
        {
            TryToUpdateUsersList();

            return _usersList.FirstOrDefault(t => t.Login.ToLower() == login.ToLower());
        }
        /// <summary>
        /// Gets the full name of the login by.
        /// </summary>
        /// <param name="usersFullName">Full name of the users.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="DebtSettlementException">
        /// Users FullName property is null or empty
        /// or
        /// </exception>
        public string GetLoginByFullName(string usersFullName)
        {
            if (string.IsNullOrWhiteSpace(usersFullName))
            {
                throw new DebtSettlementException("Users FullName property is null or empty", _logger);
            }

            TryToUpdateUsersList();

            var login = _usersList.FirstOrDefault(t => t.FullName == usersFullName)?.Login;

            if (login == null)
            {
                throw new DebtSettlementException($"Unable to find user login by '{usersFullName}'", _logger);
            }

            return login;
        }

        /// <summary>
        /// Gets the boss by department.
        /// </summary>
        /// <param name="departmentId">The department identifier.</param>
        /// <returns>DebtSettlement.BusinessLayer.Model.DTO.UserDTO.</returns>
        public UserDTO GetBossByDepartment(int departmentId)
        {
            try
            {
                var boss = _userService.GetBossByDepartment(departmentId);

                return new UserDTO
                {
                    Login = boss.Login,
                    DepartmentId = departmentId,
                    FullName = boss.FullName,
                    IsBlock = boss.IsBlock
                };
            }
            catch (Exception ex)
            {
                throw new DebtSettlementException(
                    $"Error while retrieve boss by department. {GetInnerException(ex)}", _logger);
            }
        }

        #region Private members
        private void TryToUpdateUsersList()
        {
            if (DateTime.Now - _lastUsersUpdate <= UsersUpdatePeriod && _usersList != null)
            {
                return;
            }

            lock (_syncObj)
            {
                var baseRoleUsers =
                    GetUsersList(AppSettings.UserBaseRoleId)
                        .Where(t => !string.IsNullOrWhiteSpace(t.FullName));
                var technicalRoleUsers = GetUsersList(AppSettings.TechnicalRoleId);

                _usersList = baseRoleUsers.Where(t => !technicalRoleUsers.Contains(t)).ToList();

                _lastUsersUpdate = DateTime.Now;
            }
        }

        private List<UserDTO> GetUsersList(int roleId)
        {
            try
            {
                return _userService.GetUsersByRole(roleId).ToList().Select(t =>
                    new UserDTO
                    {
                        Login = t.Login,
                        IsBlock = t.IsBlock,
                        DepartmentId = t.DepartmentId ?? -1,
                        FullName = t.FullName,
                        Email = t.UserParameters?.FirstOrDefault(u => u.Name == "Email")?.UserParameters.ToString()
                    }).ToList();
            }
            catch (Exception ex)
            {
                throw new DebtSettlementException(
                    $"Error while retrieve user list. {GetInnerException(ex)}", _logger);
            }
        }

        private static string GetInnerException(Exception exception)
        {
            var message = string.Empty;

            if (exception.InnerException != null)
            {
                message += $" {exception.InnerException.Message} {GetInnerException(exception.InnerException)}";
            }

            return message;
        }
        #endregion
    }
}
