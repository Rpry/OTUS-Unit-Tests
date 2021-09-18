using System;
using System.Threading.Tasks;
using Trading.StateMachine.DataAccess.Models;

namespace Trading.StateMachine.DataAccess.Repositories.Abstraction
{
    /// <summary>
    /// Репозиторий лота
    /// </summary>
    public interface ILotRepository : IRepositoryBase<Lot, Guid>
    {
        /// <summary>
        /// Проверить наличине
        /// </summary>
        /// <param name="id">Идентификатор</param>
        Task<bool> Any(Guid id);
    }
}
