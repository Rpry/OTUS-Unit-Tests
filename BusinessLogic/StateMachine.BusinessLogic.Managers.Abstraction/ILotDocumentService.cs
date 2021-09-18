using System;
using System.Threading.Tasks;
using Trading.StateMachine.DataAccess.Models;

namespace Trading.StateMachine.BusinessLogic.Managers.Abstraction
{
    public interface ILotDocumentService
    {
        /// <summary>
        /// Создание документа лота
        /// </summary>
        Task<Guid> CreateAsync(Guid lotId, Guid fileId, LotDocumentType type, string name);
    }
}