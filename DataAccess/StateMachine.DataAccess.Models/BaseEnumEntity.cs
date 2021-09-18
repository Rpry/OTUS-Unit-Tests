using System;

namespace Trading.StateMachine.DataAccess.Models
{
    public class BaseEnumEntity<TEnum> where TEnum : struct, IConvertible
    {
        public TEnum Code { get; set; }
        public string Description { get; set; }
    }
}