using IT2.Trading.StateMachine.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StateMachine.Compose;
using System;
using System.IO;
using System.Reflection;
using IT2.Trading.StateMachine.Common.Settings;
using IT2.Common.BusManager.Abstraction;
using IT2.Common.Infrastructure.Proxy.Abstraction;
using IT2.Common.Infrastructure.Proxy.Implementation;
using StateMachine.Compose.Settings;
using StateMachine.Infrastructure.Proxy.Implementation;

namespace IT2.Trading.StateMachine.Service
{
    class Program
    {
        static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("secrets/appsettings.secrets.json", optional: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            var proxySettings = new ProxySettings();
            configuration.Bind(nameof(ProxySettings), proxySettings);
            var serviceProvider = Configuration.GetServiceCollection(configuration, "IT2.LotService")
                .ConfigureContext(configuration)
                .AddEmployeeProxy<IStateMachineEmployeeProxy, StateMachineEmployeeProxy>(proxySettings)
                .AddSingleton<ConsumerLimitSetting>(builder =>
                    configuration.GetSection(ConsumerLimitSetting.ConsumerSettingsSection).Get<ConsumerLimitSetting>())
                .AddSingleton(builder =>
                    configuration.GetSection(nameof(ProcedureAggregationPushSettings)).Get<ProcedureAggregationPushSettings>())
                .BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Starting StateMachineService..");
            try
            {
                logger.LogInformation("Starting run migrations");
                var context = serviceProvider.GetRequiredService<TradingContext>();
                context.Database.Migrate();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error during run migrations for EF core");
                return;
            }
            logger.LogInformation("Migrations applied");
            SeedProcedureTypes.SeedData(serviceProvider);
            SeedAllEnums.SeedData(serviceProvider);
            SeedPropertyTypes.SeedData(serviceProvider);
            IBusManager busManager;
            try
            {
                logger.LogInformation("Starting connect to bus");
                busManager = serviceProvider.GetRequiredService<IBusManager>();
                busManager.StartBus(ServiceBusConfigurator.GetBusConfigurations(serviceProvider, configuration));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during start bus: " + ex.Message);
                return;
            }

            logger.LogInformation("Bus connected");
            logger.LogInformation(@"Press any key to exit");
            Console.ReadKey();
            busManager.StopBus();
        }
    }
}
