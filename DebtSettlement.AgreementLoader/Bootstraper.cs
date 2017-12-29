using HR.Client;
using HR.Client.Interface;
using DebtSettlement.AgreementLoader.Interface;
using Microsoft.Practices.Unity;
using Unity;

namespace DebtSettlement.AgreementLoader
{
    public class Bootstraper
    {
        public static void Register(IUnityContainer container)
        {
            HR.Client.Bootstraper.Register(container);

            container.RegisterType<IAgreementService, AgreementService>();
            container.RegisterType<IUserService, UserService>();
        }
    }
}
