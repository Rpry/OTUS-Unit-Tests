using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Trading.StateMachine.BusinessLogic.Managers;
using Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using Trading.StateMachine.BusinessLogic.Managers.MapProfiles;
using Trading.StateMachine.DataAccess.Context;
using Trading.StateMachine.DataAccess.Repositories;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;

namespace StateMachine.Compose
{
    public static class Configuration
    {
        public static IServiceCollection GetServiceCollection(IConfigurationRoot configuration, string serviceName, IServiceCollection serviceCollection = null)
        {
            if (serviceCollection == null)
            {
                serviceCollection = new ServiceCollection();
            }
            serviceCollection
                .AddSingleton(configuration)
                .AddSingleton((IConfiguration)configuration)
                .ConfigureAutomapper()
                .ConfigureAllRepositories()
                .ConfigureAllBusinessServices()
                .AddLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddConfiguration(configuration.GetSection("Logging"));
                    builder
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning);
                })
                .AddMemoryCache();
            return serviceCollection;
        }
        
        public static IServiceCollection ConfigureInMemoryContext(this IServiceCollection services)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            services.AddDbContext<TradingContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDb", builder => { });
                options.UseInternalServiceProvider(serviceProvider);
            });
            services.AddTransient<DbContext, TradingContext>();
            return services;
        }


        private static IServiceCollection ConfigureAutomapper(this IServiceCollection services) => services
            .AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));

        private static IServiceCollection ConfigureAllRepositories(this IServiceCollection services) => services
            .AddTransient<ILotRepository, LotRepository>()
            .AddTransient<ILotDocumentRepository, LotDocumentRepository>()
            .AddTransient<IProcedureTypeRepository, ProcedureTypeRepository>();

        private static IServiceCollection ConfigureAllBusinessServices(this IServiceCollection services) => services
            .AddTransient<ILotService, LotService>()
            .AddTransient<ILotManager, LotManager>()
            .AddTransient<ILotDocumentService, LotDocumentService>();

        private static MapperConfiguration GetMapperConfiguration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LotManagerMapProfiles>();
            });
            configuration.AssertConfigurationIsValid();
            return configuration;
        }
    }
}
