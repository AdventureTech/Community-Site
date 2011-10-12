using CommunitySite.Core.Data.NHibernate;
using CommunitySite.Core.Services.Configuration;
using FakeItEasy;
using Machine.Specifications;
using NHibernate;

namespace CommunitySite.Integration
{
    public class With_a_persistence_context
    {
        protected static ConfigurationService _configService;
        protected static NHibernateConfiguration _configuration;
        protected static ISessionFactory _sessionFactory;

        Establish context = () =>
        {
            _configService = A.Fake<ConfigurationService>();
            A.CallTo(() => _configService.ConnectionString).Returns(@"Data Source=C:\Source\Git\CommunitySite\src\Web.UI\App_Data\CommunitySite.sdf;");
            _configuration = new NHibernateConfiguration(_configService);
        };
    }
}