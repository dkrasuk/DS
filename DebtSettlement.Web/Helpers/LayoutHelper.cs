namespace DebtSettlement.Web.Helpers
{
    /// <summary>
    /// Class LayoutHelper.
    /// </summary>
    public static class LayoutHelper
    {
        /// <summary>
        /// Gets the default layout.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetDefaultLayout()
        {
            string layout;

            var isPartial = System.Web.HttpContext.Current.Request.QueryString.Get("isPartial");

            var includeBundles = System.Web.HttpContext.Current.Request.QueryString.Get("includeBundles");

            if (!string.IsNullOrEmpty(isPartial) && isPartial.ToLower() == "true" &&
                !string.IsNullOrEmpty(includeBundles) && includeBundles.ToLower() == "true")
            {
                layout = "~/Views/Shared/_SimpleLayout.cshtml";
            }
            else if (!string.IsNullOrEmpty(isPartial) && isPartial.ToLower() == "true" &&
                (string.IsNullOrEmpty(includeBundles) || includeBundles.ToLower() != "true"))
            {
                layout = null;
            }
            else
            {
                layout = "~/Views/Shared/_Layout.cshtml";
            }
            return layout;
        }
    }
}