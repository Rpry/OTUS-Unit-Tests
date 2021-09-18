using System;
using System.Threading.Tasks;
using Trading.StateMachine.DataAccess.Models;

namespace Trading.StateMachine.DataAccess.Repositories.Abstraction
{
    /// <summary>
    /// Интерфейс репозитория для работы с таблицей документов лота
    /// </summary>
    public interface ILotDocumentRepository : IRepositoryBase<LotDocument, Guid>
    {
        /// <summary>
        /// Существует ли документ
        /// </summary>
        /// <param name="lotId">Идентификатор лота</param>
        /// <param name="documentType">Тип документа</param>
        /// <returns>Список документов лота</returns>
        Task<bool> AnyAsync(Guid lotId, LotDocumentType documentType);

    }
}