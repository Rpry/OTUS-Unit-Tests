using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trading.StateMachine.DataAccess.Context;
using Trading.StateMachine.DataAccess.Models;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;

namespace Trading.StateMachine.DataAccess.Repositories
{
    /// <summary>
    /// ProcedureTypeRepository
    /// </summary>
    public class ProcedureTypeRepository : RepositoryBase<ProcedureType, string>, IProcedureTypeRepository
    {
        public ProcedureTypeRepository(TradingContext context) : base(context)
        {
        }
        
        public async Task<string> GetProcedureNameBy(string procedureCode, string language)
        {
            var result = await GetAll(false)
                .Where(x => x.Id == procedureCode && x.Resources.Any(d => d.Language == language))
                .Select(x => new
                {
                    Name = x.Resources.First(f => f.Language == language).Name
                })
                .SingleOrDefaultAsync();

            return result.Name;
        }
        
        public async Task<string> GetProcedureNameBy(string procedureCode)
        {
            return await GetProcedureNameBy(procedureCode, "ru");
        }
    }
}
