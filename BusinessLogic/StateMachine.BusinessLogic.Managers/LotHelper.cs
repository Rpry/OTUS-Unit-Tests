using System;
using System.Collections.Generic;
using Trading.StateMachine.Common;
using Trading.StateMachine.DataAccess.Models;

namespace Trading.StateMachine.BusinessLogic.Managers
{
    public static class LotHelper
    {
        public static OperationResult Validate(Lot lot)
        {
            var operationResult = new OperationResult
            {
                Errors = new List<string>()
            };
            if (string.IsNullOrEmpty(lot.CreatedUserId))
            {
                operationResult.Errors.Add(Resources.Exceptions.Validate_Не_заполнен_идентификатор_пользователя);
            }
            if (lot.CreatedOrganizationId == Guid.Empty)
            {
                operationResult.Errors.Add(Resources.Exceptions.Validate_Не_заполнен_идентификатор_организации);
            }
            return operationResult;
        }
    }
}