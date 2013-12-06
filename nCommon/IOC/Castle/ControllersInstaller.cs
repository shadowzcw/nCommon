using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace nCommon.IOC.Castle
{
    public class ControllersInstaller : IWindsorInstaller
    {
        private readonly string _namespace;
        public ControllersInstaller(string space)
        {
            _namespace = space;
        }

        /// <summary>
        /// Installs the controllers
        /// </summary>
        /// <param name="container"></param>
        /// <param name="store"></param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(FindControllers().Configure(ConfigureControllers));
        }

        /// <summary>
        /// Find controllers within this assembly
        /// </summary>
        /// <returns></returns>
        private BasedOnDescriptor FindControllers()
        {
            return AllTypes.FromAssemblyNamed(_namespace)
                .BasedOn<IController>()
                .If(t => t.Name.EndsWith("Controller"));

        }

        /// <summary>
        /// Returns the transient lifestyle for the controllers.
        /// </summary>
        /// <returns></returns>
        private void ConfigureControllers(ComponentRegistration componentRegistration)
        {
            componentRegistration.LifeStyle.Transient.Configuration();
        }
    }
}
