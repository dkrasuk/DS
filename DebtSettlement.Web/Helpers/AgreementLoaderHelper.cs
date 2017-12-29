using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.Web.Helpers
{
    /// <summary>
    /// Class AgreementLoaderHelper.
    /// </summary>
    public static class AgreementLoaderHelper
    {
        /// <summary>
        /// To the select list items.
        /// </summary>
        /// <param name="actionTypes">The action types.</param>
        /// <param name="selectedId">The selected identifier.</param>
        /// <returns>IEnumerable&lt;SelectListItem&gt;.</returns>
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<ActionType> actionTypes, int selectedId)
        {
            return
                actionTypes.OrderBy(actionType => actionType.Name)
                    .Select(actionType =>
                        new SelectListItem
                        {
                            Selected = (actionType.Id == selectedId),
                            Text = actionType.Name,
                            Value = actionType.Id.ToString()
                        });
        }
    }
}