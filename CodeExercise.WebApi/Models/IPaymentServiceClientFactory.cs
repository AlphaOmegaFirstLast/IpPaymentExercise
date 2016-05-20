using CodeExercise.WebApi.PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeExercise.WebApi.Models
{
    public interface IPaymentServiceClientFactory
    {
        IPaymentService GetService () ;
        void Close(IPaymentService service);
    }

 }
