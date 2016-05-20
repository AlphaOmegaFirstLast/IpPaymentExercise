using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel;
using CodeExercise.WebApi.Models;

namespace CodeExercise.WebApi.Controllers
{
    [RoutePrefix("api/payment")]
    public class PaymentController : ApiController
    {

        [HttpGet]
        [Route("WhatsYourId")]
        public string WhatsYourId()
        {
            var id = "";
            using (var paymentServiceClient = new PaymentService.PaymentServiceClient())
            {
                id = paymentServiceClient.WhatsYourId();
            }
            return id ;
        }

        [HttpGet]
        [Route("IsCardNumberValid/{cardNumber}")]
        public ApiResponse<bool> IsCardNumberValid(string cardNumber)
        {
            var response = new ApiResponse<bool>();
            var paymentServiceClient = new PaymentService.PaymentServiceClient();
            try
            {
                response.Data = paymentServiceClient.IsCardNumberValid(cardNumber);
            }
            catch (FaultException e)
            {
                response.status.IsSuccess = false;
                response.status.Code = e.Code.Name;
                response.status.Reason = e.Reason.ToString();
            }
            finally
            {
                paymentServiceClient.Close();
            }

            return response;
        }


        [HttpGet]
        [Route("IsValidPaymentAmount/{amount}")]
        public ApiResponse<bool> IsValidPaymentAmount(long amount)
        {
            var response = new ApiResponse<bool>();
            var paymentServiceClient = new PaymentService.PaymentServiceClient();
            try
            {
                response.Data = paymentServiceClient.IsValidPaymentAmount(amount);
            }
            catch (FaultException e)
            {
                response.status.IsSuccess = false;
                response.status.Code = e.Code.Name;
                response.status.Reason = e.Reason.ToString();
            }
            finally
            {
                paymentServiceClient.Close();
            }

            return response;
        }


        [HttpGet]
        [Route("CanMakePaymentWithCard/{cardNumber}")]
        public ApiResponse<bool> CanMakePaymentWithCard(string cardNumber, int expiryMonth, int expiryYear)
        {
            var response = new ApiResponse<bool>();
            var paymentServiceClient = new PaymentService.PaymentServiceClient();
            try
            {
                response.Data = paymentServiceClient.CanMakePaymentWithCard(cardNumber, expiryMonth, expiryYear);
            }
            catch (FaultException e)
            {
                response.status.IsSuccess = false;
                response.status.Code = e.Code.Name;
                response.status.Reason = e.Reason.ToString();
            }
            finally
            {
                paymentServiceClient.Close();
            }

            return response;
        }

    }
}
