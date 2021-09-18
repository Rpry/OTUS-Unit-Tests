using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Trading.StateMachine.Common;
using Trading.StateMachine.DataAccess.Models;

namespace Trading.StateMachine.DataAccess.Context
{
    public static class SeedProcedureTypes
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<TradingContext>();

                var addResults = new List<bool>();
                foreach (var procedureType in ProcedureTypeConstants.AllProcedureTypes)
                {
                    addResults.Add(AddProcedureType(context, procedureType.Key, procedureType.Value));
                }

                if (addResults.Any(s => s == true))
                {
                    context.SaveChanges();
                }
            }
        }

        private static bool AddProcedureType(TradingContext context, string procedureCode, ProcedureTypeSeedData procedureSeedData)
        {
            if (string.IsNullOrEmpty(procedureCode))
            {
                return false;
            }

            if (procedureSeedData == null)
            {
                return false;
            }

            var procedureTypeFromDb = context.ProcedureTypes.Include(s => s.Resources)
                                                            .FirstOrDefault(x => x.Id == procedureCode);
            if (procedureTypeFromDb != null)
            {
                procedureTypeFromDb.Resources = new List<ProcedureTypeResource>
                {
                    new ProcedureTypeResource
                    {
                        Name = procedureSeedData.NameRu,
                        Description = procedureSeedData.DescriptionRu,
                        Language = "ru"
                    },
                    new ProcedureTypeResource
                    {
                        Name = procedureSeedData.NameEn,
                        Description = procedureSeedData.DescriptionEn,
                        Language = "en"
                    }
                };

                procedureTypeFromDb.Prefix = procedureSeedData.Prefix;
                procedureTypeFromDb.RequireLots = procedureSeedData.RequireLots;
                procedureTypeFromDb.BiddingPreparationTime = procedureSeedData.BiddingPreparationTime;
                procedureTypeFromDb.BiddingBidChangeStrategyParameters = procedureSeedData.BiddingBidChangeStrategyParameters;
                procedureTypeFromDb.BiddingProlongationStrategyParameters = procedureSeedData.BiddingProlongationStrategyParameters;
                procedureTypeFromDb.UseBiddingBidChangeStrategyFromFields = procedureSeedData.UseBiddingBidChangeStrategyFromFields;
                procedureTypeFromDb.UseBiddingProlongationStrategyFromFields = procedureSeedData.UseBiddingProlongationStrategyFromFields;
                procedureTypeFromDb.FirstProtocolTemplateName = procedureSeedData.FirstProtocolTemplateName;
                procedureTypeFromDb.SecondProtocolTemplateName = procedureSeedData.SecondProtocolTemplateName;
                procedureTypeFromDb.AuctionInformationMessageTemplateName = procedureSeedData.AuctionInformationMessageTemplateName;
                procedureTypeFromDb.CutPriceInPercent = procedureSeedData.CutPriceInPercent;
                procedureTypeFromDb.MaxAllowedPriceCuttingCountWasAdded = procedureSeedData.MaxAllowedPriceCuttingCountWasAdded;
                procedureTypeFromDb.Order = procedureSeedData.Order;
                procedureTypeFromDb.UseDeposit = procedureSeedData.UseDeposit;
                procedureTypeFromDb.UseElectronicDeal = procedureSeedData.UseElectronicDeal;
                procedureTypeFromDb.UsePaperDeal = procedureSeedData.UsePaperDeal;
                procedureTypeFromDb.AbilityToSignDealWithSecondPlace = procedureSeedData.AbilityToSignDealWithSecondPlace;
                procedureTypeFromDb.Code = procedureSeedData.Code;
                procedureTypeFromDb.IsFromIt1 = procedureSeedData.IsFromIt1;
                return true;
            }

            var procedureType = new ProcedureType
            {
                Id = procedureCode,
                Resources = new List<ProcedureTypeResource>
                {
                    new ProcedureTypeResource
                    {
                        Name = procedureSeedData.NameRu,
                        Description = procedureSeedData.DescriptionRu,
                        Language = "ru"
                    },
                    new ProcedureTypeResource
                    {
                        Name = procedureSeedData.NameEn,
                        Description = procedureSeedData.DescriptionEn,
                        Language = "en"
                    }
                },
                Prefix = procedureSeedData.Prefix,
                RequireLots = procedureSeedData.RequireLots,
                BiddingPreparationTime = procedureSeedData.BiddingPreparationTime,
                BiddingBidChangeStrategyParameters = procedureSeedData.BiddingBidChangeStrategyParameters,
                BiddingProlongationStrategyParameters = procedureSeedData.BiddingProlongationStrategyParameters,
                UseBiddingBidChangeStrategyFromFields = procedureSeedData.UseBiddingBidChangeStrategyFromFields,
                UseBiddingProlongationStrategyFromFields = procedureSeedData.UseBiddingProlongationStrategyFromFields,
                FirstProtocolTemplateName = procedureSeedData.FirstProtocolTemplateName,
                SecondProtocolTemplateName = procedureSeedData.SecondProtocolTemplateName,
                AuctionInformationMessageTemplateName = procedureSeedData.AuctionInformationMessageTemplateName,
                CutPriceInPercent = procedureSeedData.CutPriceInPercent,
                MaxAllowedPriceCuttingCountWasAdded = procedureSeedData.MaxAllowedPriceCuttingCountWasAdded,
                Order = procedureSeedData.Order,
                UseDeposit = procedureSeedData.UseDeposit,
                UseElectronicDeal = procedureSeedData.UseElectronicDeal,
                UsePaperDeal = procedureSeedData.UsePaperDeal,
                AbilityToSignDealWithSecondPlace = procedureSeedData.AbilityToSignDealWithSecondPlace,
                Code = procedureSeedData.Code,
                IsFromIt1 = procedureSeedData.IsFromIt1,
            };
            context.ProcedureTypes.Add(procedureType);
            return true;
        }
    }
}