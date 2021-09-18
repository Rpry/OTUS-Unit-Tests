using System.Threading.Tasks;
using Trading.StateMachine.DataAccess.Models;

namespace Trading.StateMachine.DataAccess.Repositories.Abstraction
{
    /// <summary>
    /// IProcedureTypeRepository
    /// </summary>
    public interface IProcedureTypeRepository : IRepositoryBase<ProcedureType, string>
    {
        Task<string> GetProcedureNameBy(string procedureCode);
    }
}
