using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;

namespace CodeExercise.Library.IntegrationTest
{
    [TestClass]
    public class PaymentServiceTest
    {
        internal static ServiceHost Instance = null;

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            Instance = new ServiceHost(typeof(PaymentService.PaymentServiceClient));
            //no need at the current time as CodeExerCise.Library has the option "Start WCF service host..." on
            // in "debug" mode the service is up by default.
            //    Instance.Open();  
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            if (Instance.State != CommunicationState.Closed)
            {
                Instance.Close();
            }
        }

        //------------------------------------------------------------

        [TestMethod]
        public void WhatsYourId()
        {
            using (var factory = new ChannelFactory<PaymentService.IPaymentService>("BasicHttpBinding_PaymentService"))
            {
                var client = factory.CreateChannel();
                var response = client.WhatsYourId();

                Assert.IsNotNull(response);
            }
        }
        [TestMethod]
        public void IsCardNumberValid()
        {
            using (var factory = new ChannelFactory<PaymentService.IPaymentService>("BasicHttpBinding_PaymentService"))
            {
                var client = factory.CreateChannel();
                var response = client.IsCardNumberValid("3275385073612308");

                Assert.IsNotNull(response);
                Assert.IsTrue(response);
            }
        }

        [TestMethod]
        public void IsValidPaymentAmount()
        {
            using (var factory = new ChannelFactory<PaymentService.IPaymentService>("BasicHttpBinding_PaymentService"))
            {
                var client = factory.CreateChannel();
                var response = client.IsValidPaymentAmount(999);

                Assert.IsNotNull(response);
                Assert.IsTrue(response);
            }
        }

        [TestMethod]
        public void CanMakePaymentWithCard()
        {
            using (var factory = new ChannelFactory<PaymentService.IPaymentService>("BasicHttpBinding_PaymentService"))
            {
                var client = factory.CreateChannel();
                var response = client.CanMakePaymentWithCard("3275385073612308", 12,2016);

                Assert.IsNotNull(response);
                Assert.IsTrue(response);
            }
        }
    }
}
