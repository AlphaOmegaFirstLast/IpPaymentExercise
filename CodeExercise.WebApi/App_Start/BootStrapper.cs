using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;                  // for GlobalConfiguration

using Microsoft.Practices.Unity ;       // for unity Container
using Unity.WebApi;                     // for Web Api Dependency resolver
using CodeExercise.WebApi.Models ;

namespace CodeExercise.WebApi.App_Start
{
    public static class BootStrapper
    {
        //-----------------------------------------------------------------------

        public static void RegisterDependencyInjection()
        {
            var container = BuildUnityContainer();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        //----------------------------------------------------------------------
        public static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer() ;
            container.RegisterType<IPaymentServiceClientFactory , PaymentServiceClientFactory>();
            return container;
        }
    }
}