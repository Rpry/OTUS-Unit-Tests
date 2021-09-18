using System;

namespace Trading.StateMachine.DataAccess.Models
{
    /// <summary>
    /// Документ лота
    /// </summary>
    public class LotDocument : IEntity<Guid>
    {
        /// <summary>
        /// Идентификатор
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
        /// Идентификатор файла в файловом сервисе
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public LotDocumentType Type { get; set; }

        /// <summary>
        /// Имя документа
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Время добавления документа лота
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Время окончания срока подписания документа
        /// </summary>
        public DateTime? SigningEndDateTime { get; set; }

        /// <summary>
        /// Клонирование класса
        /// </summary>
        /// <returns>Скопированный класс</returns>
        public virtual LotDocument Clone()
        {
            return (LotDocument)MemberwiseClone();
        }
    }
}