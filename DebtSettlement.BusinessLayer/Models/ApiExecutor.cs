using System.Collections.Generic;
using RestSharp;

namespace DebtSettlement.BusinessLayer.Models
{
    public class ApiExecutor
    {
        public string ApiUrl { get; set; }

        public string ApiKey { get; set; }

        public string Resource { get; set; }

        public Method Method { get; set; }

        public object Body { get; set; }

        public List<Parameter> Parameters { get; set; }

        public DataFormat DataFormat { get; set; }
    }
}
