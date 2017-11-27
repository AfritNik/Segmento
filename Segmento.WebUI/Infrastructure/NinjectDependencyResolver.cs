﻿using Ninject;
using Segmento.Domain.Abstract;
using Segmento.Domain.Database;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Segmento.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
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
            kernel.Bind<IReportRepository>().ToMethod(context=> EFReportRepository.GetInstance());
        }
    }
}