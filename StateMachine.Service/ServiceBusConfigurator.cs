using Automatonymous;
using GreenPipes;
using GreenPipes.Configurators;
using IT2.Common.SharedData;
using IT2.Trading.StateMachine.BusinessLogic.Exceptions;
using IT2.Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using IT2.Trading.StateMachine.BusinessLogic.StateMachines.Application;
using IT2.Trading.StateMachine.BusinessLogic.StateMachines.Events;
using IT2.Trading.StateMachine.BusinessLogic.StateMachines.Lot;
using IT2.Trading.StateMachine.Common;
using IT2.Trading.StateMachine.Contracts;
using IT2.Trading.StateMachine.Contracts.Application.Events;
using IT2.Trading.StateMachine.Contracts.Calendar.Requests;
using IT2.Trading.StateMachine.Contracts.Calendar.Responses;
using IT2.Trading.StateMachine.Contracts.Lot.Events;
using IT2.Trading.StateMachine.Contracts.Trade.Requests;
using IT2.Trading.StateMachine.Contracts.Trade.Responses;
using IT2.Trading.StateMachine.DataAccess.Context;
using IT2.Trading.StateMachine.DataAccess.Models;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration.Saga;
using MassTransit.RabbitMqTransport;
using MassTransit.Saga;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using StateMachine.BusinessLogic.Consumers.Application;
using StateMachine.BusinessLogic.Consumers.Lot;
using StateMachine.BusinessLogic.Consumers.Trade;
using StateMachine.Compose;
using StateMachine.Compose.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace IT2.Trading.StateMachine.Service
{
    public static class ServiceBusConfigurator
    {
        /// <summary>
        /// Получить полную конфигурацию для шины
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Action<IRabbitMqReceiveEndpointConfigurator>> GetBusConfigurations(
            IServiceProvider serviceProvider, IConfigurationRoot configuration)
        {
            var busConfig = new Dictionary<string, Action<IRabbitMqReceiveEndpointConfigurator>>();

            ConfigureStateMachines(serviceProvider, busConfig, configuration);

            // TODO изучить использование MassTransit.Extensions.DependencyInjection (решить вопрос с созданием Scope)
            busConfig.Add("IT2.Trading.LotPublish.Fault",
                e =>
                {
                    e.Consumer<LotPublishFaultConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add("IT2.Trading.ApplicationPublish.Fault",
                e =>
                {
                    e.Consumer<ApplicationPublishFaultConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotGetCutPriceQueue,
                e =>
                {
                    e.Consumer<LotGetCutPriceConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotProtocol1CreateQueue,
                e =>
                {
                    e.Consumer<LotCreateProtocol1Consumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotGenerateProtocol1DocumentQueue,
                e =>
                {
                    e.Consumer<LotGenerateProtocol1DocumentConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotCreateProtocol2,
                e =>
                {
                    e.Consumer<LotCreateProtocol2Consumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotSendProtocol2ToBuyer,
                e =>
                {
                    e.Consumer<LotSendProtocol2ToBuyerConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotSignProtocol2ByBuyer,
                e =>
                {
                    e.Consumer<LotSignProtocol2ByBuyerConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotProtocol1AttachQueue,
                e =>
                {
                    e.Consumer<LotAttachUserProtocol1Consumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotProtocol2AttachQueue,
                e =>
                {
                    e.Consumer<LotAttachUserProtocol2Consumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotProtocol1AuxDataQueue,
                e =>
                {
                    e.Consumer<SmGetLotProtocol1AuxDataConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotProtocol2AuxDataQueue,
                e =>
                {
                    e.Consumer<SmGetLotProtocol2AuxDataConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotUnblockTariffQueue,
                e =>
                {
                    e.Consumer<ApplicationTariffUnblockConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotUnblockDepositQueue,
                e =>
                {
                    e.Consumer<ApplicationDepositUnBlockConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotBlockTariffQueue,
                e =>
                {
                    e.Consumer<ApplicationTariffBlockConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotBlockDepositQueue,
                e =>
                {
                    e.Consumer<ApplicationDepositBlockConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotTransferDepositQueue,
                e =>
                {
                    e.Consumer<ApplicationDepositTransferConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotWriteOffTariffQueue,
                e =>
                {
                    e.Consumer<ApplicationTariffWriteOffConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.ApplicationDepositWithdrawToThirdPersonEventQueue,
                e =>
                {
                    e.Consumer<ApplicationWithdrawToThirdPersonConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingGetDepositTransitDocumentRequestQueue,
                e =>
                {
                    e.Consumer<LotGetDepositTransferDocumentConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingGetDepositTransitDocumentWithCustomRequisitesRequestQueue,
                e =>
                {
                    e.Consumer<LotGetDepositTransferDocumentWithCustomRequisitesConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.NotifyAboutDailyApplicationsCount,
                e =>
                {
                    e.Consumer<NotifyAboutDailyApplicationsCountConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingSuspend,
                e =>
                {
                    e.Consumer<SmSuspendConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotDelete,
                e =>
                {
                    e.Consumer<LotDeleteConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotRescheduleBiddingQueue,
                e =>
                {
                    e.Consumer<LotRescheduleBiddingConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingSetApplicationStateQueue,
                e =>
                {
                    e.Consumer<ApplicationSetStateConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingSetApplicationStatesQueue,
                e =>
                {
                    e.Consumer<ApplicationSetStatesConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotAddLotDocumentQueue,
                e =>
                {
                    e.Consumer<LotDocumentAddConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingUnSuspend,
                e =>
                {
                    e.Consumer<SmUnSuspendConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotUpdateOrganizationIdQueue,
                e =>
                {
                    e.Consumer<UpdateOrganizationIdConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotUpdateEmployeeIdQueue,
                e =>
                {
                    e.Consumer<UpdateEmployeeIdConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotDocumentSignatureSave,
                e =>
                {
                    e.Consumer<LotDocumentSignatureSaveConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotStateHasBeenChanged,
                e =>
                {
                    e.Handler<LotStateHasBeenChangedEvent>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider.GetService<ILotManager>()
                                    .SendInfoAboutLotToBus(ctx.Message.Id, ctx.Message.State);
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<LotStateHasBeenChangedEvent>>()
                                    .LogError(exception, "Error during LotStateHasBeenChangedEvent");
                                throw;
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotUpdateInElastic,
                e =>
                {
                    e.Handler<UpdateLotsInElasticEvent>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider.GetService<ILotManager>()
                                    .UpdateLotsInElastic();
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<UpdateLotsInElasticEvent>>()
                                    .LogError(exception, "Error during UpdateLotsInElasticEvent");
                                throw;
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingApplicationUpdateInElastic,
                e =>
                {
                    e.Handler<UpdateApplicationsInElasticEvent>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider.GetService<IApplicationManager>()
                                    .UpdateApplicationsInElastic();
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<UpdateApplicationsInElasticEvent>>()
                                    .LogError(exception, "Error during UpdateApplicationsInElasticEvent");
                                throw;
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotHideQueue,
                e =>
                {
                    e.Handler<HideLotEvent>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider.GetService<ILotManager>()
                                    .HideLot(ctx.Message.Id);
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<HideLotEvent>>()
                                    .LogError(exception, "Error during HideLotEvent");
                                throw;
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingSetStateQueue,
                e =>
                {
                    e.Handler<SetLotStateEvent>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider.GetService<ILotManager>()
                                    .SetLotState(ctx.Message.Id, ctx.Message.State);
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<SetLotStateEvent>>()
                                    .LogError(exception, "Error during SetLotStateEvent");
                                throw;
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add("IT2.Trading.StateMachineService.ErrorDuringPublishLot",
                e =>
                {
                    e.Handler<ErrorDuringPublishLotEvent>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider.GetService<ILotManager>()
                                    .SendInfoAboutLotToBus(ctx.Message.Id, "Draft");
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<ErrorDuringPublishLotEvent>>()
                                    .LogError(exception, "Error during ErrorDuringPublishLotEvent");
                                throw;
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingApplicationHasBeenPublishedQueueName,
                e =>
                {
                    e.Handler<ApplicationHasBeenPublishedEvent>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider.GetService<IApplicationManager>()
                                    .SendInfoAboutLotToBus(ctx.Message.Id, ApplicationStateCommonConstants.Created);
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<ApplicationHasBeenPublishedEvent>>()
                                    .LogError(exception, "Error during ApplicationHasBeenPublishedEvent");
                                throw;
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingApplicationDraftHasBeenSavedQueueName,
                e =>
                {
                    e.Handler<ApplicationDraftHasBeenSavedEvent>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider.GetService<IApplicationManager>()
                                    .SendInfoAboutLotToBus(ctx.Message.Id, "Draft");
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<ApplicationDraftHasBeenSavedEvent>>()
                                    .LogError(exception, "Error during ApplicationDraftHasBeenSavedEvent");
                                throw;
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingApplicationStateHasBeenChangedQueueName,
                e =>
                {
                    e.Handler<ApplicationStateHasBeenChangedEvent>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider.GetService<IApplicationManager>()
                                    .SendInfoAboutLotToBus(ctx.Message.Id, ctx.Message.State);
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<ApplicationStateHasBeenChangedEvent>>()
                                    .LogError(exception, "Error during ApplicationStateHasBeenChangedEvent");
                                throw;
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotDeleteLotDocumentQueue,
                e =>
                {
                    e.Handler<SmLotDocumentDeleteEvent>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider
                                           .GetService<ILotDocumentManager>()
                                           .DeleteLotDocument(ctx.Message.LotId,
                                                              ctx.Message.FileId,
                                                              ctx.Message.EmployeeId,
                                                              ctx.Message.CurrentOrganizationId);
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<SmLotDocumentDeleteEvent>>()
                                    .LogError(exception, "Error during SmLotDocumentDeleteEvent");
                                throw;
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.ApplicationFillEmptyCreatedOrganizationIdsQueue,
                e =>
                {
                    e.Handler<SmFillEmptyCreatedOrganizationIdsEvent>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider.GetService<IApplicationManager>().FillEmptyCreatedOrganizationIdsAsync();
                                await scope.ServiceProvider.GetService<ISuspensionManager>().FillEmptyCreatedOrganizationIdsAsync();
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<SmFillEmptyCreatedOrganizationIdsEvent>>()
                                    .LogError(exception, "Error during SmFillEmptyCreatedOrganizationIdsEvent ");
                                throw;
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingLotSagaSaveAuditInfoQueue,
                e =>
                {
                    e.Consumer<LotSagaSaveAuditInfoConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingApplicationSagaSaveAuditInfoQueue,
                e =>
                {
                    e.Consumer<ApplicationSagaSaveAuditInfoConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradeGenerateInformationMessageRequestQueue,
                e =>
                {
                    e.Consumer<CreateInformationMessageBeforePublishingConsumer>(serviceProvider);
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.ValidateTradeRequestQueue,
               e =>
               {
                   e.Consumer<ValidateTradeConsumer>(serviceProvider);
                   e.SetThreadMode();
               });
            busConfig.Add(Queues.ValidateLotRequestQueue,
              e =>
              {
                  e.Consumer<ValidateLotConsumer>(serviceProvider);
                  e.SetThreadMode();
              });
            busConfig.Add(Queues.TradingGetTradeNumberQueue,
                e =>
                {
                    e.Handler<GetTradeNumberRequest>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                var result = await scope.ServiceProvider
                                    .GetService<ILotManager>()
                                    .GetTradeNumber(ctx.Message.ProcedureCode, ctx.Message.CreatedOrganizationId,
                                        ctx.Message.Id);

                                await ctx.RespondAsync(new
                                    GetTradeNumberResponse
                                {
                                    IsSuccess = true,
                                    Result = result
                                });
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<GetTradeNumberRequest>>()
                                    .LogError(exception, "Error during GetTradeNumberRequest");
                                await ctx.RespondAsync(new
                                    GetTradeNumberResponse
                                {
                                    IsSuccess = false,
                                    ErrorMessages = new List<string>
                                        {
                                            exception.Message
                                        }
                                });
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.SaveUserDataRequestQueue,
                e =>
                {
                    e.Handler<SaveTradeUserDataRequest>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider
                                    .GetService<ITradeManager>()
                                    .SaveTradeUserData(ctx.Message);

                                await ctx.RespondAsync(new
                                    SaveTradeUserDataResponse
                                {
                                    IsSuccess = true,
                                    Result = true
                                });
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<SaveTradeUserDataRequest>>()
                                    .LogError(exception, "Error during SaveTradeUserDataRequest");
                                await ctx.RespondAsync(new
                                    SaveTradeUserDataResponse
                                {
                                    IsSuccess = false,
                                    ErrorMessages = new List<string>
                                        {
                                            exception.Message
                                        }
                                });
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.SavePublishDraftUserDataRequestQueue,
                e =>
                {
                    e.Handler<SaveTradePublishDraftUserDataRequest>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider.GetService<ITradeManager>().SaveTradePublishDraftUserData(ctx.Message);
                                await ctx.RespondAsync(new SaveTradePublishDraftUserDataResponse
                                {
                                    IsSuccess = true,
                                    Result = true
                                });
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<SaveTradePublishDraftUserDataRequest>>()
                                    .LogError(exception, "Error during SaveTradePublishDraftUserDataRequest");
                                await ctx.RespondAsync(new SaveTradePublishDraftUserDataResponse
                                {
                                    IsSuccess = false,
                                    ErrorMessages = new List<string>
                                    {
                                        exception.Message
                                    }
                                });
                            }
                        }
                    });
                    e.SetThreadMode();
                });
            busConfig.Add(Queues.TradingSaveLotToPostgresQueue,
                e =>
                {
                    e.Consumer<SaveLotToPostgresConsumer>(serviceProvider);
                    e.SetThreadMode();
                    e.UseMessageRetry(r =>
                    {
                        r.Ignore<LotCreatedBusinessLogicException>();
                        r.Ignore<BusinessLogicException>();
                        r.Ignore<ValidationExceptions>();
                        r.Incremental(retryLimit: 3, initialInterval: TimeSpan.FromSeconds(1),
                            intervalIncrement: TimeSpan.FromSeconds(1));
                    });
                });
            busConfig.Add(Queues.TradingSaveLotsToPostgresQueue,
                e =>
                {
                    e.Consumer<SaveLotsToPostgresConsumer>(serviceProvider);
                    e.SetThreadMode();
                    e.UseMessageRetry(r =>
                    {
                        r.Ignore<LotCreatedBusinessLogicException>();
                        r.Ignore<BusinessLogicException>();
                        r.Ignore<ValidationExceptions>();
                        r.Incremental(retryLimit: 3, initialInterval: TimeSpan.FromSeconds(1),
                            intervalIncrement: TimeSpan.FromSeconds(1));
                    });
                });
            busConfig.Add(Queues.TradingSaveLotToPostgresRequestQueue,
                e =>
                {
                    e.Consumer<SaveLotToPostgresRequestConsumer>(serviceProvider);
                    e.SetThreadMode();
                    e.UseMessageRetry(r =>
                    {
                        r.Ignore<LotCreatedBusinessLogicException>();
                        r.Ignore<BusinessLogicException>();
                        r.Ignore<ValidationExceptions>();
                        r.Incremental(retryLimit: 3, initialInterval: TimeSpan.FromSeconds(1),
                            intervalIncrement: TimeSpan.FromSeconds(1));
                    });
                });

            busConfig.Add($"{Queues.TradingSaveLotToPostgresQueue}.Fault",
                e =>
                {
                    e.Consumer<SaveLotToPostgresFaultConsumer>(serviceProvider);
                    e.SetThreadMode();
                });

            busConfig.Add(Queues.CalendarGetNextWorkingDay,
                e =>
                {
                    e.Handler<CalendarGetNextWorkingDayRequest>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                var result = await scope.ServiceProvider
                                    .GetService<ICalendarService>()
                                    .GetNextWorkingDate(ctx.Message.StateFrom, ctx.Message.DaysCount);

                                await ctx.RespondAsync(new
                                    CalendarGetNextWorkingDayResponse
                                {
                                    IsSuccess = true,
                                    Result = result
                                });
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<CalendarGetNextWorkingDayRequest>>()
                                    .LogError(exception, "Error during CalendarGetNextWorkingDayRequest");
                                await ctx.RespondAsync(new
                                    CalendarGetNextWorkingDayResponse
                                {
                                    IsSuccess = false,
                                    ErrorMessages = new List<string>
                                        {
                                            exception.Message
                                        }
                                });
                            }
                        }
                    });
                    e.SetThreadMode();
                });

            busConfig.Add(Queues.NotifyAboutLotHasBeenPushedEvent,
                e =>
                {
                    e.Handler<SmNotifyAboutLotHasBeenPushedEvent>(async ctx =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            try
                            {
                                await scope.ServiceProvider
                                    .GetService<INotificationManager>()
                                    .NotifyAboutLotPublishAsync(ctx.Message.LotGuids);
                            }
                            catch (Exception exception)
                            {
                                serviceProvider.GetService<ILogger<SmNotifyAboutLotHasBeenPushedEvent>>()
                                    .LogError(exception, "Error during SmNotifyAboutLotHasBeenPushedEvent");
                            }
                        }
                    });
                });

            busConfig.Add(Queues.UpdateTradeFieldsRequestQueue,
              e =>
              {
                  e.Consumer<UpdateTradeFieldsConsumer>(serviceProvider);
                  e.SetThreadMode();
              });
            busConfig.Add(Queues.TradingApplicationReviewAndCreateProtocol1QueueName,
              e =>
              {
                  e.Consumer<ApplicationsReviewAndCreateProtocol1Consumer>(serviceProvider);
                  e.SetThreadMode();
              });
            busConfig.Add(Queues.TradingApplicationReviewAndAttachUserProtocol1QueueName,
             e =>
             {
                 e.Consumer<ApplicationsReviewAndAttachUserProtocol1Consumer>(serviceProvider);
                 e.SetThreadMode();
             });
            var stateMachineEventsBusConfigurator = serviceProvider.GetService<StateMachineEventsBusConfigurator>();
            foreach (var tuple in stateMachineEventsBusConfigurator.GetHandlersForStateMachine())
            {
                busConfig.Add(tuple.Item1, tuple.Item2);
            }

            return busConfig;
        }

        private static void ConfigureStateMachines(IServiceProvider serviceProvider,
            Dictionary<string, Action<IRabbitMqReceiveEndpointConfigurator>> busConfig,
            IConfigurationRoot configuration)
        {
            foreach (var procedureType in ProcedureTypeHelper.GetAllAvailableProcedureTypes(configuration))
            {
                switch (procedureType)
                {
                    case ProcedureTypeConstants.ConfiscatedProperty:
                        {
                            ConfigureStateMachine<SmLotConfiscatedProperty, SmApplicationConfiscatedProperty>(
                                serviceProvider,
                                busConfig,
                                procedureType);
                        }
                        break;
                    case ProcedureTypeConstants.ArrestedProperty:
                        {
                            ConfigureStateMachine<SmLotArrestedProperty, SmApplicationArrestedProperty>(
                                serviceProvider,
                                busConfig,
                                procedureType);
                        }
                        break;
                    default:
                        {
                            ConfigureStateMachine<LotStateMachine, ApplicationStateMachine>(
                                serviceProvider,
                                busConfig,
                                procedureType);
                            break;
                        }
                }
            }
        }

        private static void ConfigureStateMachine<TLotStateMachine, TApplicationStateMachine>(
            IServiceProvider serviceProvider,
            Dictionary<string, Action<IRabbitMqReceiveEndpointConfigurator>> busConfig,
            string auctionType)
            where TLotStateMachine : SagaStateMachine<LotSaga>
            where TApplicationStateMachine : SagaStateMachine<ApplicationSaga>
        {
            DbContext ContextFactory()
            {
                return
                    serviceProvider
                        .GetRequiredService<TradingContext
                        >(); //Временное решение (см. комментарий при регистрации TradingContext)
            }

            var limitConsumerSetting = serviceProvider.GetService<ConsumerLimitSetting>();
            busConfig.Add(
                $"{StateMachineQueueNameConstants.PrefixForLotStateMachineInnerEvents}.{auctionType}",
                e =>
                {
                    e.UseInMemoryOutbox();
                    e.StateMachineSaga(serviceProvider.GetRequiredService<TLotStateMachine>(),
                        GetLotSagaRepository(ContextFactory));
                    e.UseRetry(RetryConfigurator);
                    e.SetThreadMode(limitConsumerSetting.ConcurrencyLimit, limitConsumerSetting.PrefetchCount);
                });

            busConfig.Add(
                $"{StateMachineQueuePrefixes.TradingApplicationStateMachineQueuePrefix}.{auctionType}",
                e =>
                {
                    e.UseInMemoryOutbox();
                    e.StateMachineSaga(serviceProvider.GetRequiredService<TApplicationStateMachine>(),
                        GetApplicationSagaRepository(ContextFactory));
                    e.UseRetry(RetryConfigurator);
                    e.SetThreadMode(limitConsumerSetting.ConcurrencyLimit, limitConsumerSetting.PrefetchCount);
                });
        }

        public static readonly Action<IRetryConfigurator> RetryConfigurator = configurator =>
        {
            configurator.Handle<DbUpdateException>();
            configurator.Handle<NpgsqlException>();
            configurator.Handle<PostgresException>(); //возникает при попытке одновременно сохранить несколько сущностей
            configurator.Handle<InvalidOperationException>();
            configurator.Intervals(new[] { 0.5, 2, 10, 30, 60, 120 }.Select(t => TimeSpan.FromMinutes(t)).ToArray());
        };

        public static ISagaRepository<LotSaga> GetLotSagaRepository(Func<DbContext> contextFactory)
        {
            return EntityFrameworkSagaRepository<LotSaga>.CreateOptimistic(contextFactory);
        }

        private static ISagaRepository<ApplicationSaga> GetApplicationSagaRepository(
            Func<DbContext> contextFactory)
        {
            return EntityFrameworkSagaRepository<ApplicationSaga>.CreateOptimistic(contextFactory);
        }

        private static void SetThreadMode(this IRabbitMqReceiveEndpointConfigurator endpointConfigurator, ushort concurrencyLimit = 100, ushort prefetchCount = 30)
        {
            endpointConfigurator.PrefetchCount = prefetchCount;
            endpointConfigurator.UseConcurrencyLimit(concurrencyLimit);
        }
    }
}