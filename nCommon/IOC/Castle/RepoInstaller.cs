using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace nCommon.IOC.Castle
{
    public class RepoInstaller : IWindsorInstaller
    {
        private readonly string _namespace;
        private readonly string _endsWith;
        //注册筛选条件
        private readonly Predicate<Type> _filter;
        public RepoInstaller(string space, string endsWith = "Repo", Predicate<Type> filter = null)
        {
            _namespace = space;
            _endsWith = endsWith;
            _filter = filter;
        }
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var reg = AllTypes.FromAssemblyNamed(_namespace).Where(t => t.Name.EndsWith(_endsWith)).WithService.DefaultInterfaces();
            if (_filter != null)
                reg = reg.Where(_filter);
            container.Register(reg);
        }
    }
}
