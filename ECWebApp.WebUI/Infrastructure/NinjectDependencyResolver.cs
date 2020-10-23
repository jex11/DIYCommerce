using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

using Moq;
using Ninject;

using ECWebApp.Domain.Abstract;
using ECWebApp.Domain.Concrete;
using ECWebApp.Domain;
using ECWebApp.WebUI.Infrastructure.Abstract;
using ECWebApp.WebUI.Infrastructure.Concrete;

namespace ECWebApp.WebUI.Infrastructure
{
    public class NinjectDependencyResolver: IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        { 
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IProductRepository>().To<EFProductRepository>();
            kernel.Bind<ICustomerRepository>().To<EFCustomerRepository>();
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            kernel.Bind<ICartRepository>().To<EFCartRepository>();
            kernel.Bind<ICustomProductRepository>().To<EFCustomProductRepository>();
            kernel.Bind<IOrderRepository>().To<EFOrderRepository>();
        }



    }
}