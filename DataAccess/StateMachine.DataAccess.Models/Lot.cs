using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trading.StateMachine.DataAccess.Models
{
    /// <summary>
    /// Лот процедуры
    /// </summary>
    public class Lot : IEntity<Guid>
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Номер лота
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// Причина отмены лота
        /// </summary>
        public string CancelReason { get; set; }

        /// <summary>
        /// Идентификатор создавшего пользователя
        /// </summary>
        public string CreatedUserId { get; set; }

        /// <summary>
        /// Идентификатор создавшего сотрудника
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Идентификатор создавшей организации
        /// </summary>
        public Guid CreatedOrganizationId { get; set; }

        /// <summary>
        /// Дата создания в UTC
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Дата публикации в UTC
        /// </summary>
        public DateTime? Published { get; set; }

        /// <summary>
        /// Список поданных заявок
        /// </summary>
        public virtual List<Application> Applications { get; set; }

        /// <summary>
        /// Документы
        /// </summary>
        public virtual List<LotDocument> Documents { get; set; }

        /// <summary>
        /// Номер процедуры
        /// </summary>
        public string TradeNumber { get; set; }

        /// <summary>
        /// Код типа аукциона
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// Тип аукциона
        /// </summary>
        public virtual ProcedureType ProcedureType { get; set; }

        /// <summary>
        /// Ошибки
        /// </summary>
        public virtual List<LotError> Errors { get; set; }

        /// <summary>
        /// Идентификатор избранной заявки
        /// </summary>
        public Guid? NeoApplicationId { get; set; }

        /// <summary>
        /// Избранная заявка
        /// </summary>
        [ForeignKey("NeoApplicationId")]
        public virtual Application NeoApplication { get; set; }

        /// <summary>
        /// Лучшая ставка избранной заявки
        /// </summary>
        public decimal BestBidAmount { get; set; }

        /// <summary>
        /// Идентификатор комиссии рассмотрения заявок (при ActionInitiator =  Committee)
        /// </summary>
        public Guid? CommitteeId { get; set; }

        /// <summary>
        /// Состояние до блокировки
        /// </summary>
        public string StateBeforeSuspension { get; set; }
        
        /// <summary>
        /// Лот скрыт
        /// </summary>
        public bool IsHidden { get; set; }
    }
}
