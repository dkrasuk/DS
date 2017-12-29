using System;
using System.Collections.Generic;

namespace DebtSettlement.Web.Models
{
    /// <summary>
    /// Class ExecuteCommandModel.
    /// </summary>
    public class ExecuteCommandModel
    {
        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>The task identifier.</value>
        public Guid? TaskId {get; set;}
        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        /// <value>The name of the command.</value>
        public string CommandName { get; set; }
        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
}