﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using Trading.StateMachine.Common;
using Trading.StateMachine.DataAccess.Models;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;

namespace Trading.StateMachine.BusinessLogic.Managers
{
    public class LotManager: ILotManager
    {
        private readonly IProcedureTypeRepository _procedureTypeRepository;
        
        private readonly IMapper _mapper;

        public LotManager(IMapper mapper, IProcedureTypeRepository procedureTypeRepository)
        {
            _procedureTypeRepository = procedureTypeRepository;
            _mapper = mapper;
        }
        
        public async Task<OperationResult> ValidateAsync(Lot lot)
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
            var procedureName = await _procedureTypeRepository.GetProcedureNameBy(lot.ProcedureCode);
            if (string.IsNullOrEmpty(procedureName))
            {
                operationResult.Errors.Add(string.Format(Resources.Exceptions.LotManager_ValidateAsync_Название_процедуры_по_коду__0__не_найдено, lot.ProcedureCode));
            }
            return operationResult;
        }
    }
}