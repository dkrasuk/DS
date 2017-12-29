using System;
namespace DebtSettlement.Web.Models
{
    /// <summary>
    /// Class ProlongateTaskViewModel.
    /// </summary>
    public class ProlongateTaskViewModel
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date {get; set;}
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }
    }
}
