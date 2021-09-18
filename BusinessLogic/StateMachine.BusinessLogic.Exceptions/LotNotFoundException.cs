using System;
using System.Runtime.Serialization;

namespace Trading.StateMachine.BusinessLogic.Exceptions
{
    /// <summary>
    /// Исключение типа Лот не найден
    /// </summary>
    [Serializable]
    public class LotNotFoundException : Exception
    {
        public LotNotFoundException(Guid lotId) 
            : base($"Лот с идентификатором {lotId} не найден")
        {
        }

        public LotNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) 
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
