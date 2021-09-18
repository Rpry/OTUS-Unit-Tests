using System;
using System.Collections.Generic;

namespace Trading.StateMachine.DataAccess.Models
{
    /// <summary>
    /// Доступные типы процедуры
    /// </summary>
    public class ProcedureType : IEntity<string>
    {
        /// <summary>
        /// Код типа аукциона - берётся из констант AuctionTypeConstants
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Шаблон заявки
        /// </summary>
        public byte[] ApplicationTemplate { get; set; }

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
        /// Ресурсы типа процедуры
        /// </summary>
        public virtual List<ProcedureTypeResource> Resources { get; set; }

        /// <summary>
        /// Все лоты с данным аукционом
        /// </summary>
        public virtual List<Lot> Lots { get; set; }

        /// <summary>
        /// Имя валидационного класса для конкретного аукциона
        /// Это класс будет взять из контейнера
        /// Класс должен быть унаследован от IValidator
        /// Этот класс должен лежать в одной общей папке и иметь верный NameSpace или иметь полный NameSpace
        /// </summary>
        public string ValidationClassName { get; set; }

        /// <summary>
        /// Префикс (для генерирования номера процедуры)
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
        /// JSON параметров стратегия изменения ставки по торгам 
        /// </summary>
        public string BiddingBidChangeStrategyParameters { get; set; }

        /// <summary>
        /// Использовать стратегия изменения ставки по торгам из полей
        /// (данные из данной сущности будут игнорироваться) 
        /// </summary>
        public bool UseBiddingBidChangeStrategyFromFields { get; set; }

        /// <summary>
        /// JSON стратегии продления торгов 
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
        /// Признак отображения процедуры
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Признак доступности расширенного тарифа
        /// </summary>
        public bool DoAvailableExtendedTariff { get; set; }

        /// <summary>
        /// Код типа процедуры
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Является ли процедура из ИТ-1
        /// </summary>
        public bool IsFromIt1{ get; set; }
    }
}