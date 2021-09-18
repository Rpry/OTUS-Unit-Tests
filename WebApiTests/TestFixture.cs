using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class TestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Выполняется перед запуском тестов
        /// </summary>
        public TestFixture()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();
            var servicesCollection = new ServiceCollection();
            new Startup(configuration).ConfigureServices(servicesCollection);
            var serviceProvider = servicesCollection.BuildServiceProvider();
            ServiceProvider = serviceProvider;
        }

        public void Dispose()
        {
        }
    }
}
