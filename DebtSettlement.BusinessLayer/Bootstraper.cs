using Microsoft.Practices.Unity;
using DebtSettlement.BusinessLayer.Services;
using DebtSettlement.BusinessLayer.Services.Interfaces;
using DebtSettlement.BusinessLayer.Services.Notifications;
using Unity;

namespace DebtSettlement.BusinessLayer
{
    public class Bootstraper
    {
        public static void Register(IUnityContainer container)
        {

            CollateralServiceApiClient.Bootstraper.Register(container);

            DictionariesClient.Bootsrap.Register(container);

            HR.Client.Bootstraper.Register(container);

            WorkflowEngine.Client.Bootstraper.Register(container);

            AlfaBank.Common.Bootstrapper.Register(container);

            DebtSettlement.AgreementLoader.Bootstraper.Register(container);

            container.RegisterType<IHRService, HRService>();

            container.RegisterType<IDebtSettlementService, DebtSettlementService>();

            container.RegisterType<INotificationsService, NotificationsHttpService>();

            container.RegisterType<INotificationTemplateService, NotificationTemplateService>();
            //container.RegisterType<INotificationsService, NotificationsServiceBusService>();

            container.RegisterType<ISmtpService, SmtpService>();

            container.RegisterType<IDictionaryService, DictionaryService>();
            container.RegisterType<ICollateralServices, CollateralServices>();

        }
    }
}
