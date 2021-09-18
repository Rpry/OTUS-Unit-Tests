using System;

namespace Trading.StateMachine.DataAccess.Models
{
    /// <summary>
    /// Описание типа процедуры
    /// </summary>
    public class ProcedureTypeResource : IEntity<Guid>
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор типа процедуры
        /// </summary>
        public string ProcedureTypeId { get; set; }

        /// <summary>
        /// Тип процедуры
        /// </summary>
        public virtual ProcedureType ProcedureType { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание типа
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Язык
        /// </summary>
        public string Language { get; set; }
    }
}
