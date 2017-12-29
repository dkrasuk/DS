using AlfaBank.Common.Utils;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

namespace DebtSettlement.Web.Attributes
{
    /// <summary>
    /// Class ODataQueryableAttribute. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="System.Web.Http.OData.EnableQueryAttribute" />
    public sealed class ODataQueryableAttribute : EnableQueryAttribute
    {
        private const string userNameVariable = "::username";

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataQueryableAttribute"/> class.
        /// </summary>
        public ODataQueryableAttribute()
        {
            this.AllowedArithmeticOperators = AllowedArithmeticOperators.All;
            this.AllowedFunctions = 
                AllowedFunctions.All | 
                AllowedFunctions.IndexOf | 
                AllowedFunctions.ToLower |
                AllowedFunctions.ToUpper;
            this.AllowedLogicalOperators = AllowedLogicalOperators.All;
            this.AllowedQueryOptions =
                AllowedQueryOptions.All |
                AllowedQueryOptions.Skip |
                AllowedQueryOptions.Top |
                AllowedQueryOptions.Filter |
                AllowedQueryOptions.OrderBy |
                AllowedQueryOptions.InlineCount |
                AllowedQueryOptions.Select |
                AllowedQueryOptions.Expand;
            this.MaxTop = 100;
        }

        /// <summary>
        /// Applies the query to the given IQueryable based on incoming query from uri and query settings. By default, the implementation supports $top, $skip, $orderby and $filter. Override this method to perform additional query composition of the query.
        /// </summary>
        /// <param name="queryable">The original queryable instance from the response message.</param>
        /// <param name="queryOptions">The <see cref="T:System.Web.Http.OData.Query.ODataQueryOptions" /> instance constructed based on the incoming request.</param>
        /// <returns>IQueryable.</returns>
        public override IQueryable ApplyQuery(IQueryable queryable, ODataQueryOptions queryOptions)
        {
            
            if (queryOptions.Filter != null)
            {
                var url = queryOptions.Request.RequestUri.AbsoluteUri;
                var req = new HttpRequestMessage(HttpMethod.Get, url);

                queryOptions = new ODataQueryOptions(queryOptions.Context, req);
            }
            var result = queryOptions.ApplyTo(queryable);
            return result;
        }

        /// <summary>
        /// Validates the OData query in the incoming request. By default, the implementation throws an exception if the query contains unsupported query parameters. Override this method to perform additional validation of the query.
        /// </summary>
        /// <param name="request">The incoming request.</param>
        /// <param name="queryOptions">The <see cref="T:System.Web.Http.OData.Query.ODataQueryOptions" /> instance constructed based on the incoming request.</param>
        public override void ValidateQuery(HttpRequestMessage request, ODataQueryOptions queryOptions)
        {
            if (queryOptions.Filter != null)
            {
                var url = queryOptions.Request.RequestUri.AbsoluteUri;
                request.RequestUri = new Uri(ProcessUrl(url));
                var queryContext = new ODataQueryContext(queryOptions.Context.Model, queryOptions.Context.ElementType);
                queryOptions = new ODataQueryOptions(queryContext, request);
                base.ValidateQuery(request, queryOptions);
            }
        }

        /// <summary>
        /// Processes the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>System.String.</returns>
        private string ProcessUrl(string url)
        {
            url = url.Replace("DateTime.Now", "DateTime%27" + DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ss") + "%27")
                    .Replace("Date.Now", "DateTime%27" + DateTime.UtcNow.ToString("yyyy-MM-dd") + "%27");

            if (Thread.CurrentPrincipal != null
                && Thread.CurrentPrincipal.Identity != null
                && !string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name))
            {
                var userName = Thread.CurrentPrincipal.Identity.Name.FormatUserName();
                url = url.Replace("'" + userNameVariable + "'", "'" + userName + "'")
                        .Replace("%27" + userNameVariable + "%27", "'" + userName + "'")
                        .Replace(userNameVariable, "'" + userName + "'"); ;
            }

            return url;
        }
    }
}