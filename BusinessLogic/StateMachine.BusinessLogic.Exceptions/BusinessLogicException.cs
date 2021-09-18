using System;

namespace Trading.StateMachine.BusinessLogic.Exceptions
{
    /// <summary>
    /// Ошибка бизнес-логики
    /// </summary>
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(string message): base(message)
        {
        }
    }
}
