using System.Collections.Generic;
using DebtSettlement.Model.DTO;

namespace DebtSettlement.BusinessLayer.Services.Interfaces
{
    public interface IHRService
    {
        /// <summary>
        /// Determines whether [is user login exist] [the specified user login].
        /// </summary>
        /// <param name="userLogin">The user login.</param>
        /// <returns><c>true</c> if [is user login exist] [the specified user login]; otherwise, <c>false</c>.</returns>
        bool IsUserLoginExist(string userLogin);

        /// <summary>
        /// Finds the user.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>IEnumerable&lt;UserDTO&gt;.</returns>
        IEnumerable<UserDTO> FindUsers(string searchTerm);

        /// <summary>
        /// Gets the full name of the login by.
        /// </summary>
        /// <param name="usersFullName">Full name of the users.</param>
        /// <returns>System.String.</returns>
        string GetLoginByFullName(string usersFullName);

        UserDTO GetUserByLogin(string login);

        /// <summary>
        /// Gets the boss by department.
        /// </summary>
        /// <param name="departmentId">The department identifier.</param>
        /// <returns>UserDTO.</returns>
        UserDTO GetBossByDepartment(int departmentId);
    }
}
