using CommunitySite.Core.Services.Configuration;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Cfg;

namespace CommunitySite.Core.Data.NHibernate
{
    public class NHibernateConfiguration
    {
        readonly ConfigurationService _configurationService;

        public NHibernateConfiguration(ConfigurationService configurationService) {
            _configurationService = configurationService;
        }

        public ISessionFactory CreateSessionFactory() {
            return Fluently.Configure()
                .Database(MsSqlCeConfiguration.Standard
                    .ConnectionString(_configurationService.ConnectionString))
                .Mappings(m =>
                    m.AutoMappings.Add(
                    AutoMap.AssemblyOf<NHibernateRepository>(new MyAutoConfig())
                    .UseOverridesFromAssemblyOf<NHibernateRepository>()
                    .Conventions.Add(
                        Table.Is(x => Inflector.Net.Inflector.Pluralize(x.EntityType.Name)),
                        PrimaryKey.Name.Is(x => "ID"),
                        ForeignKey.EndsWith("ID"),
                        ConventionBuilder.HasManyToMany.Always(x => x.AsList()),
                        DefaultCascade.All())))
                .ExposeConfiguration(cfg => Configuration = cfg)
                .BuildSessionFactory();
        }

        public Configuration Configuration { get; set; }

        public class MyAutoConfig : DefaultAutomappingConfiguration {
            public override bool IsId(FluentNHibernate.Member member) {
                return member.Name == "ID";
            }

            public override bool ShouldMap(System.Type type) {
                return type.Namespace.Contains("Domain");
            }

            public override bool ShouldMap(FluentNHibernate.Member member) {
                return member.IsProperty;
            }
        }
    }
}
