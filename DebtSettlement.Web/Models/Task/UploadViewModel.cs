using System.Collections.Generic;

namespace DebtSettlement.Web.Models.Task
{
    /// <summary>
    /// Class TaskStateModel.
    /// </summary>
    public class UploadViewModel
    {
        /// <summary>
        /// Gets or sets wheather upload was successful.
        /// </summary>
        /// <value>The name of the state.</value>
        public bool Success { get; set; }
        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        /// <value>The comment.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the content of the agreement.
        /// </summary>
        /// <value>The content of the agreement.</value>
        public string AgreementContent { get; set; }
    }
}