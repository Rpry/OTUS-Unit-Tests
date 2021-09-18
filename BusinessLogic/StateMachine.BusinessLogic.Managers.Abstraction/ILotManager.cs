using System.Threading.Tasks;
using Trading.StateMachine.Common;
using Trading.StateMachine.DataAccess.Models;

namespace Trading.StateMachine.BusinessLogic.Managers.Abstraction
{
    /// <summary>
    /// Интерфейс менеджера работы с лотами 
    /// </summary>
    public interface ILotManager
    {
        Task<OperationResult> ValidateAsync(Lot lot);
    }
}