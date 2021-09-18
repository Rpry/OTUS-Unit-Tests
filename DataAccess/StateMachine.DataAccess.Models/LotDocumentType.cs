namespace Trading.StateMachine.DataAccess.Models
{
   public enum LotDocumentType
    {
        /// <summary>
        /// Неизвестный
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Протокол №1
        /// </summary>
        Protocol1 = 5,

        /// <summary>
        /// Протокол №1 (форма со штампом печати)
        /// </summary>
        Protocol1Preview = 10
    }
}