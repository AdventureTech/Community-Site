using System.Linq;
using CommunitySite.Core.Data;
using CommunitySite.Core.Data.NHibernate;
using CommunitySite.Core.Services.Authentication;
using CommunitySite.Core.Services.Configuration;
using NHibernate;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace CommunitySite.Web.UI
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                        scan.LookForRegistries();
                                    });
                            // x.For<IExample>().Use<Example>();
                        });
            return ObjectFactory.Container;
        }
    }

    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            var pluginTypes = typeof(Repository).Assembly.GetTypes().Where(t => !t.Name.StartsWith("NHibernate") && t.Name.EndsWith("Repository"));
            foreach (var pluginType in pluginTypes)
            {
                var concreteType =
                    typeof(NHibernateRepository).Assembly.GetTypes()
                        .FirstOrDefault(t =>
                                        t.Name.StartsWith("NHibernate") &&
                                        t.Name.EndsWith("Repository") &&
                                        pluginType.IsAssignableFrom(t));

                if (concreteType != null)
                    For(pluginType).Use(concreteType);
            }

            For<ISessionFactory>().UseSpecial(
                x => x.ConstructedBy(
                    () => ObjectFactory.GetInstance<NHibernateConfiguration>()
                              .CreateSessionFactory()))
                .Singleton();
        }
    }

    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            For<ConfigurationService>().Use<WebConfigurationService>();
            For<AuthenticationService>().Use<WebAuthenticationService>();
        }
    }
}