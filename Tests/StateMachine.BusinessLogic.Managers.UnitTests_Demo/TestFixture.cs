using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StateMachine.Compose;

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
            var builder = new ConfigurationBuilder();
            var configuration = builder.Build();
            var serviceCollection = Configuration.GetServiceCollection(configuration, "Tests");
            var serviceProvider = serviceCollection
                .BuildServiceProvider();
            ServiceProvider = serviceProvider;
        }

        public void Dispose()
        {
        }
    }
}
