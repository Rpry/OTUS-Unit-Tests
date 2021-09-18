using System;

namespace Trading.StateMachine.DataAccess.Models
{
    /// <summary>
    /// Ошибка лота
    /// </summary>
    public class LotError : IEntity<Guid>
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор лота
        /// </summary>
        public Guid LotId { get; set; }

        /// <summary>
        /// Лот
        /// </summary>
        public virtual Lot Lot { get; set; }

        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Язык ошибки
        /// </summary>
        public string Language { get; set; }
    }
}
