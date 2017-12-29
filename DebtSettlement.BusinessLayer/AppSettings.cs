using System;
using System.Configuration;

namespace DebtSettlement.BusinessLayer
{
    public static class AppSettings
    {
        public static string WorkflowWebAPIAddress => ConfigurationManager.AppSettings["WorkflowEngineWebAPIAddress"];
        public static string DictionaryAPIAddress => ConfigurationManager.AppSettings["DictionaryAPIAddress"];

        public static string HRWebAPIAddress => ConfigurationManager.AppSettings["HRWebAPIAddress"];

        public static string HRWebAPILogin => ConfigurationManager.AppSettings["HRWebAPILogin"];

        public static string HRWebAPIPassword => ConfigurationManager.AppSettings["HRWebAPIPassword"];

        public static string DefaultWorkflowSchemeCode => ConfigurationManager.AppSettings["DefaultWorkflowSchemeCode"];

        public static int DataUpdatePeriod => Convert.ToInt32(ConfigurationManager.AppSettings["DataUpdatePeriod"]);

        public static int UserBaseRoleId => Convert.ToInt32(ConfigurationManager.AppSettings["UserBaseRoleId"]);

        public static int TechnicalRoleId => Convert.ToInt32(ConfigurationManager.AppSettings["TechnicalRoleId"]);

        public static string NotificationsServiceUrl => ConfigurationManager.AppSettings["NotificationsServiceUrl"];        

        public static string DebtSettlementCreateTaskPermission
        {
            get
            {
                var value = ConfigurationManager.AppSettings["DebtSettlementCreateTaskPermission"];
                return value ?? "DebtSettlementCreateTaskPermission";
            }
        }
    }
}