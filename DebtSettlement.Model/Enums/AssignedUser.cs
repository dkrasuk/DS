using System.ComponentModel.DataAnnotations;

namespace DebtSettlement.Model.Enums
{
    /// <summary>
    /// Enum AssignedUser
    /// </summary>
    public enum AssignedUser
    {
        /// <summary>
        /// The custom
        /// </summary>
        [Display(Name = "Custom")]
        Custom = 0,
        /// <summary>
        /// The rs
        /// </summary>
        [Display(Name = "RS")]
        RS = 1,
        /// <summary>
        /// The field
        /// </summary>
        [Display(Name = "Field")]
        Field = 2,
        /// <summary>
        /// The legal
        /// </summary>
        [Display(Name = "Legal")]
        Legal = 3
    }
}
