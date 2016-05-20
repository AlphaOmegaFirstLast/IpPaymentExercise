using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeExercise.WebApi.Models;
using CodeExercise.WebApi.PaymentService;

namespace CodeExercise.WebApi.Models
{
    public class PaymentServiceClientFactory : IPaymentServiceClientFactory
    {
        public void Close(IPaymentService service)
        {
            if ( service is PaymentServiceClient )
            {
                (service as PaymentServiceClient).Close();
            }
        }


        public IPaymentService GetService()
        {
           return new PaymentServiceClient();   //in test this class will be mocked
        }
    }
}