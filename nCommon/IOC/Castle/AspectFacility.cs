using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;

namespace nCommon.IOC.Castle
{
    public class AspectFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.Register(Component.For<IInterceptor>()
                                     .ImplementedBy<MethodBoundaryInterceptor>()
                                     .Named(typeof(MethodBoundaryInterceptor).Name)
                                     .LifeStyle.Is(LifestyleType.Singleton));
            Kernel.ComponentRegistered += AddMethodBoundaryInterceptorIfNeeded;
        }

        private void AddMethodBoundaryInterceptorIfNeeded(string key, IHandler handler)
        {
            List<Attribute> attributes = GetMethodBoundaryAttributes(handler);

            if (attributes.Count > 0)
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(MethodBoundaryInterceptor).Name));
            }
        }

        private List<Attribute> GetMethodBoundaryAttributes(IHandler handler)
        {
            var attributes = new List<Attribute>();

            //mthod level attributes
            foreach (MethodInfo methodInfo in handler.ComponentModel.Implementation.GetMethods())
            {
                attributes.AddRange((Attribute[])methodInfo.GetCustomAttributes(typeof(MethodBoundaryAttribute), false));
            }

            //class level attributes
            attributes.AddRange((Attribute[])handler.ComponentModel.Implementation.GetCustomAttributes(typeof(MethodBoundaryAttribute), false));

            return attributes;
        }
    }
}
