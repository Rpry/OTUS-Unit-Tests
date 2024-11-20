using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trading.StateMachine.DataAccess.Models;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;

namespace Trading.StateMachine.DataAccess.Repositories
{
    /// <summary>
    /// Репозиторий процедур
    /// </summary>
    public class LotRepository : RepositoryBase<Lot, Guid>, ILotRepository
    {
        public LotRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// Проверить наличине
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public async Task<bool> AnyAsync(Guid id)
        {
            return await EntitySet.Where(t => t.Id.Equals(id)).AnyAsync();
        }
    }
}
