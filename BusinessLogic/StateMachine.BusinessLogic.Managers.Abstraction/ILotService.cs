using System.Threading.Tasks;
using Trading.StateMachine.Common;
using Trading.StateMachine.DataAccess.Models;

namespace Trading.StateMachine.BusinessLogic.Managers.Abstraction
{
    public interface ILotService
    {
        Task<OperationResult> ValidateAsync(Lot lot);
    }
}