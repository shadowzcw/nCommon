using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace nCommon.IOC.Castle
{
    public class IOC
    {
        private static readonly IWindsorContainer _container;
        static IOC()
        {
            _container = new WindsorContainer();
        }

        public static IWindsorContainer Container
        {
            get { return _container; }
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static void Register(Type interfaceType, Type implementationType)
        {
            Container.Register(Component.For(interfaceType).ImplementedBy(implementationType).LifeStyle.Singleton);
        }
    }
}
