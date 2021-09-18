using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trading.StateMachine.DataAccess.Models
{
    /// <summary>
    /// Модель заявки
    /// </summary>
    public class Application : IEntity<Guid>
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата создания в UTC
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Дата публикации UTC
        /// </summary>
        public DateTime? Published { get; set; }

        /// <summary>
        /// Идентификатор лота процедуры, на которую была подана заявка
        /// </summary>
        public Guid LotId { get; set; }

        /// <summary>
        /// Лот, на которую была подана заявка
        /// </summary>
        [ForeignKey("LotId")]
        public virtual Lot Lot { get; set; }
    }
}
