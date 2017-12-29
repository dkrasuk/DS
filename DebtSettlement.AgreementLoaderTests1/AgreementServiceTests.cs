using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebtSettlement.AgreementLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtSettlement.AgreementLoader.Tests
{
    [TestClass()]
    public class AgreementServiceTests
    {
        [TestMethod()]
        public void GetAgreementTest()
        {
            AgreementService service = new AgreementService();
            var answer = service.GetAgreement(3151915);
        }
        [TestMethod()]
        public async Task  GetApplicationFormFilling()
        {
            AgreementService service = new AgreementService();
            var answer = await service.GetApplicationFormFilling(1111733);
        }
    }
}