using System;
using System.Collections.Generic;

namespace Trading.StateMachine.Common
{
    public class ProcedureTypeSeedData
    {
        /// <summary>
        /// Русское наименование
        /// </summary>
        public string NameRu { get; set; }

        /// <summary>
        /// Английское наименование
        /// </summary>
        public string NameEn { get; set; }
        
        /// <summary>
        /// Русское описание
        /// </summary>
        public string DescriptionRu { get; set; }

        /// <summary>
        /// Английское описание
        /// </summary>
        public string DescriptionEn { get; set; }

        /// <summary>
        /// Префикс
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Флаг необходимости лота
        /// </summary>
        public bool RequireLots { get; set; }

        /// <summary>
        /// Время подготовки к торгам
        /// </summary>
        public TimeSpan BiddingPreparationTime { get; set; }

        /// <summary>
        /// Тип торгов
        /// </summary>
        public int BiddingType { get; set; }

        /// <summary>
        /// Стратегия изменения ставки по торгам
        /// </summary>
        public int BiddingBidChangeStrategy { get; set; }

        /// <summary>
        /// JSON параметров стратегия изменения ставки по торгам 
        /// </summary>
        public string BiddingBidChangeStrategyParameters { get; set; }

        /// <summary>
        /// Использовать стратегия изменения ставки по торгам из полей
        /// (данные из данной сущности будут игнорироваться) 
        /// </summary>
        public bool UseBiddingBidChangeStrategyFromFields { get; set; }

        /// <summary>
        /// Стратегия продления торгов
        /// </summary>
        public int BiddingProlongationStrategy { get; set; }

        /// <summary>
        /// JSON cтратегии продления торгов 
        /// </summary>
        public string BiddingProlongationStrategyParameters { get; set; }

        /// <summary>
        /// Использовать стратегию продления торгов
        /// (данные из данной сущности будут игнорироваться) 
        /// </summary>
        public bool UseBiddingProlongationStrategyFromFields { get; set; }

        /// <summary>
        /// Процент понижения цены лота
        /// </summary>
        public double? CutPriceInPercent { get; set; }

        /// <summary>
        /// Максимально допустимое количество понижений цены 
        /// </summary>
        public int? MaxAllowedPriceCuttingCountWasAdded { get; set; }

        /// <summary>
        /// Имя первого протокола
        /// </summary>
        public string FirstProtocolTemplateName { get; set; }

        /// <summary>
        /// Имя второго протокола
        /// </summary>
        public string SecondProtocolTemplateName { get; set; }

        /// <summary>
        /// Имя информационного сообщения о проведении аукциона
        /// </summary>
        public string AuctionInformationMessageTemplateName { get; set; }

        /// <summary>
        /// Порядок вывода
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Использовать бумажный договор
        /// </summary>
        public bool UsePaperDeal { get; set; }

        /// <summary>
        /// Использовать электронный договор
        /// </summary>
        public bool UseElectronicDeal { get; set; }

        /// <summary>
        /// Использовать задатки
        /// </summary>
        public bool UseDeposit { get; set; }

        /// <summary>
        /// Возможность заключать договор со вторым местом торгов
        /// </summary>
        public bool AbilityToSignDealWithSecondPlace { get; set; }

        /// <summary>
        /// Код типа процедуры
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Является ли процедура из ИТ-1
        /// </summary>
        public bool IsFromIt1 { get; set; }

        /// <summary>
        /// Подтипы процедуры
        /// </summary>
        public List<ProcedureSubTypeSeedData> SubTypes { get; set; }
    }
}