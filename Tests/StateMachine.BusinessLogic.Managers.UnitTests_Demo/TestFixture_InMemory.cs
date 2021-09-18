using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StateMachine.Compose;
using Trading.StateMachine.DataAccess.Context;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class TestFixture_InMemory : IDisposable
    {
        public IServiceProvider ServiceProvider { get; set; }

        public IServiceCollection ServiceCollection { get; set; }
        
        /// <summary>
        /// Выполняется перед запуском тестов
        /// </summary>
        public TestFixture_InMemory()
        {
            var builder = new ConfigurationBuilder();
            var configuration = builder.Build();
            ServiceCollection = Configuration.GetServiceCollection(configuration, "Tests");
            var serviceProvider = GetServiceProvider();
            ServiceProvider = serviceProvider;
        }

        private IServiceProvider GetServiceProvider()
        {
            var serviceProvider = ServiceCollection
                .ConfigureInMemoryContext()
                .BuildServiceProvider();
            SeedProcedureTypes.SeedData(serviceProvider);
            return serviceProvider;
        }

        public void Dispose()
        {
        }
    }
}
