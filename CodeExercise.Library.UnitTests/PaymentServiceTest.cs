using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;

namespace CodeExercise.Library.UnitTests
{
    [TestClass]
    public class PaymentServiceTest
    {
        //-------------------------   WhatsYourId    ------------------------------
        [TestMethod]
        public void WhatsYourId_Success()
        {
            var paymentService = new PaymentService();
            Assert.AreEqual(paymentService.WhatsYourId(), "2a8dec5a-5f70-45a2-9e0b-b14064850de0");
        }

        //-------------------------- IsCardNumberValid ---------------------------------

        [TestMethod]
        public void IsCardNumberValid_Success()
        {
            var paymentService = new PaymentService();
            Assert.IsTrue(paymentService.IsCardNumberValid("3275385073612308"));
        }

        [TestMethod]
        public void IsCardNumberValid_InValidStringLength()
        {
            var paymentService = new PaymentService();
            try
            {
                try
                {
                    paymentService.IsCardNumberValid("");                       // empty string
                    Assert.Fail("An exception should have been thrown");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Invalid CardNumber", faultEx.Code.Name);
                }
                try
                {
                    paymentService.IsCardNumberValid("32753850736123");         // less than 16 digit
                    Assert.Fail("An exception should have been thrown");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Invalid CardNumber", faultEx.Code.Name);
                }
                try
                {
                    paymentService.IsCardNumberValid("3275385073612308777777"); // greater than 16 digit
                    Assert.Fail("An exception should have been thrown");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Invalid CardNumber", faultEx.Code.Name);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }


        [TestMethod]
        public void IsCardNumberValid_InValidDataType()
        {
            var paymentService = new PaymentService();
            try
            {
                try
                {
                    paymentService.IsCardNumberValid("327538507361AAAA");       // length=16 but letters included
                    Assert.Fail("An exception should have been thrown");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Invalid CardNumber", faultEx.Code.Name);
                }
                try
                {
                    paymentService.IsCardNumberValid("32753850736_=+%#");       // length=16 but special characters included
                    Assert.Fail("An exception should have been thrown");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Invalid CardNumber", faultEx.Code.Name);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void IsCardNumberValid_InValidSequence()
        {
            var paymentService = new PaymentService();
            try
            {
                paymentService.IsCardNumberValid("3275385073612398");       // apply algorithm - it fails
                Assert.Fail("An exception should have been thrown");
            }
            catch (FaultException faultEx)
            {
                Assert.AreEqual("Invalid CardNumber", faultEx.Code.Name);
            }
        
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        //--------------------------------- IsValidPaymentAmount ----------------------------------
        [TestMethod]
        public void IsValidPaymentAmount_Success()
        {
            var paymentService = new PaymentService();

            Assert.IsTrue(paymentService.IsValidPaymentAmount(100));                //Within range 99 - 99999999
            Assert.IsTrue(paymentService.IsValidPaymentAmount(99));                 //Lower boundry check - boundries are inclusive
            Assert.IsTrue(paymentService.IsValidPaymentAmount(99999999));           //Upper boundry check - boundries are inclusive
        }

        [TestMethod]
        public void IsValidPaymentAmount_InValidRange()
        {
            var paymentService = new PaymentService();
            try
            {
                try
                {
                    paymentService.IsValidPaymentAmount(98);                //Less than lower boundry
                    Assert.Fail("An exception should have been thrown");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Argument Out of Range", faultEx.Code.Name);
                }
                try
                {
                    paymentService.IsValidPaymentAmount(999999991);         //Greater than upper boundry
                    Assert.Fail("An exception should have been thrown");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Argument Out of Range", faultEx.Code.Name);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }
        //------------------------------ CanMakePaymentWithCard --------------------------------------

        [TestMethod]
        public void CanMakePaymentWithCard_Success()
        {
            var paymentService = new PaymentService();

            Assert.IsTrue(paymentService.CanMakePaymentWithCard("3275385073612308", 12, 2016));             // valid expiry month and expiry year
        }

        [TestMethod]
        public void CanMakePaymentWithCard_InvalidCardnumber()
        {
            var paymentService = new PaymentService();
            try
            {
                paymentService.CanMakePaymentWithCard("3275385073612308777777", 12, 2016);
                Assert.Fail("An exception should have been thrown");
            }
            catch (FaultException faultEx)
            {
                Assert.AreEqual("Invalid CardNumber", faultEx.Code.Name);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message) );
            }
        }

        [TestMethod]
        public void CanMakePaymentWithCard_InvalidExpiry()
        {
            var paymentService = new PaymentService();
            try
            {
                try
                {
                    paymentService.CanMakePaymentWithCard("3275385073612308", 0, 2016);             //invalid expiry month
                    Assert.Fail("An exception should have been thrown - invalid expiry month 1");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Argument Out of Range", faultEx.Code.Name);
                }
                try
                {
                    paymentService.CanMakePaymentWithCard("3275385073612308", 13, 2016);            //invalid expiry month
                    Assert.Fail("An exception should have been thrown - invalid expiry month 2");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Argument Out of Range", faultEx.Code.Name);
                }
                try
                {
                    paymentService.CanMakePaymentWithCard("3275385073612308", 12, 15);              //invalid expiry year 
                    Assert.Fail("An exception should have been thrown - invalid expiry year 1");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Invalid Expiry Date", faultEx.Code.Name);
                }
                try
                {
                    paymentService.CanMakePaymentWithCard("3275385073612308", 12, 2116);            //invalid expiry year
                    Assert.Fail("An exception should have been thrown - invalid expiry year 2");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Invalid Expiry Date", faultEx.Code.Name);
                }
                try
                {
                    paymentService.CanMakePaymentWithCard("3275385073612308", 12, 2015);            //invalid expiry date (in past)
                    Assert.Fail("An exception should have been thrown - invalid expiry date (in past)");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Invalid Expiry Date", faultEx.Code.Name);
                }
                try
                {
                    paymentService.CanMakePaymentWithCard("3275385073612308", 4, 2016);             //invalid expiry date (check the boundry - current month - business rule "assumed" card expires 1st of month)
                    Assert.Fail("An exception should have been thrown- invalid expiry date (check the boundry)");
                }
                catch (FaultException faultEx)
                {
                    Assert.AreEqual("Invalid Expiry Date", faultEx.Code.Name);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }

        }

    }
}
