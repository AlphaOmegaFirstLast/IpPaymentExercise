using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using CodeExercise.WebApi;
using CodeExercise.WebApi.PaymentService ;
using CodeExercise.WebApi.Controllers;
using CodeExercise.WebApi.Models;
using Moq;

namespace CodeExercise.WebApi.Tests.Controllers
{
    [TestClass]
    public class PaymentWithDIControllerTest
    {

        private Mock<IPaymentServiceClientFactory> _paymentServiceClientFactory;

        //-------------------------------------------------------------------------------------
        [TestInitialize]
        public void init()
        {
            _paymentServiceClientFactory = new Mock<IPaymentServiceClientFactory>();
        }

        //-------------------------------------------------------------------------------------
        [TestMethod]
        public void WhatsYourId() 
        {
            // instantiate a controller with a mock object
            var controller = new PaymentWithDIController(_paymentServiceClientFactory.Object);

            //set up the mock object method
            //  var _paymentService = (new Mock<IPaymentService>()).Object;

            var _paymentService = new Mock<IPaymentService>();
            _paymentService.Setup(x => x.WhatsYourId()).Returns("ABC");

            // pass mocked service to the factory 
            _paymentServiceClientFactory.Setup(x => x.GetService()).Returns(_paymentService.Object);

            // call controller method
            var result = controller.WhatsYourId();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ABC", result);

        }

    }
}
