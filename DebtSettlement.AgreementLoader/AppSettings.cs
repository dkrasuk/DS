using System;
using System.Configuration;

namespace DebtSettlement.AgreementLoader
{
    public static class AppSettings
    {
        public static int DataUpdatePeriod => Convert.ToInt32(ConfigurationManager.AppSettings["DataUpdatePeriod"]);
    }
}