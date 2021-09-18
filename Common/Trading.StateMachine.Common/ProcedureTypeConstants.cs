using System;
using System.Collections.Generic;

namespace Trading.StateMachine.Common
{
    /// <summary>
    /// Все поддерживаемые типы процедур.
    /// Строка должна быть без пробелов, и уникальная
    /// Это имя строки будет использовано в имени очереди для аукциона
    /// </summary>
    public class ProcedureTypeConstants
    {
        public const string MyProperty1 = "MyProperty1";
        public const string MyProperty2 = "MyProperty2";

        /// <summary>
        /// Все доступные типы процедур(в том числе для сидинга)
        /// </summary>
        public static Dictionary<string, ProcedureTypeSeedData> AllProcedureTypes =
            new Dictionary<string, ProcedureTypeSeedData>
            {
                {
                    MyProperty1,
                    new ProcedureTypeSeedData
                    {
                        NameRu = "Реализация имущества",
                        Prefix = "01",
                        RequireLots = false,
                        BiddingPreparationTime = TimeSpan.FromMinutes(5),
                        BiddingBidChangeStrategy = 5,
                        BiddingBidChangeStrategyParameters = "{\"Value\":5}",
                        BiddingProlongationStrategy = 5,
                        BiddingProlongationStrategyParameters = "{\"Value\":\"00:15:00\"}",
                        BiddingType = 5,
                        UseBiddingBidChangeStrategyFromFields = true,
                        UseBiddingProlongationStrategyFromFields = true,
                        Order = 3,
                        UseDeposit = true,
                        UsePaperDeal = false
                    }
                },
                {
                    MyProperty2,
                    new ProcedureTypeSeedData
                    {
                        NameRu = "Реализация имущества",
                        Prefix = "03",
                        RequireLots = false,
                        BiddingPreparationTime = TimeSpan.FromMinutes(5),
                        BiddingBidChangeStrategy = 5,
                        BiddingBidChangeStrategyParameters = "{\"Value\":5}",
                        BiddingProlongationStrategy = 5,
                        BiddingProlongationStrategyParameters = "{\"Value\":\"00:15:00\"}",
                        BiddingType = 5,
                        UseBiddingBidChangeStrategyFromFields = true,
                        UseBiddingProlongationStrategyFromFields = true,
                        MaxAllowedPriceCuttingCountWasAdded = 3,
                        Order = 2,
                        UseDeposit = true,
                        UsePaperDeal = true,
                        UseElectronicDeal = true,
                        AbilityToSignDealWithSecondPlace = false,
                        Code = "200"
                    }
                }
            };
    }
}