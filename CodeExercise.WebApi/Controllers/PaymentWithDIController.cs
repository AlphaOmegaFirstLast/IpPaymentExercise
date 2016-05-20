using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeExercise.WebApi.PaymentService;
using CodeExercise.WebApi.Models;

namespace CodeExercise.WebApi.Controllers
{
    [RoutePrefix("api/paymentDI")]
    public class PaymentWithDIController : ApiController
    {
        private readonly IPaymentServiceClientFactory _paymentServiceClientFactory;

        //-----------------------------------------------------------------------
        public PaymentWithDIController(IPaymentServiceClientFactory paymentServiceClientFactory)
        {
            _paymentServiceClientFactory = paymentServiceClientFactory;
        }
        //-----------------------------------------------------------------------

        [HttpGet]
        [Route("WhatsYourId")]
        public string WhatsYourId()
        {
            var paymentServiceClient = _paymentServiceClientFactory.GetService();
            var id = paymentServiceClient.WhatsYourId();
            _paymentServiceClientFactory.Close(paymentServiceClient);

            return id ;
        }

        [HttpGet]
        [Route("IsCardNumberValid/{cardNumber}")]
        public bool IsCardNumberValid(string cardNumber)
        {
            var paymentServiceClient = _paymentServiceClientFactory.GetService();
            var result = paymentServiceClient.IsCardNumberValid(cardNumber);
            _paymentServiceClientFactory.Close(paymentServiceClient);

            return result;
        }


        [HttpGet]
        [Route("IsValidPaymentAmount/{amount}")]
        public bool IsValidPaymentAmount(long amount)
        {
            var paymentServiceClient = _paymentServiceClientFactory.GetService();
            var result = paymentServiceClient.IsValidPaymentAmount(amount);
            _paymentServiceClientFactory.Close(paymentServiceClient);

            return result;
        }


        [HttpGet]
        [Route("CanMakePaymentWithCard/{cardNumber}")]
        public bool CanMakePaymentWithCard(string cardNumber, int expiryMonth, int expiryYear)
        {
            var paymentServiceClient = _paymentServiceClientFactory.GetService();
            var result = paymentServiceClient.CanMakePaymentWithCard(cardNumber, expiryMonth, expiryYear);
            _paymentServiceClientFactory.Close(paymentServiceClient);

            return result;
        }

    }
}
