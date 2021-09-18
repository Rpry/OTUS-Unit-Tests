using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trading.StateMachine.DataAccess.Context;
using Trading.StateMachine.DataAccess.Models;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;
using Z.EntityFramework.Plus;

namespace Trading.StateMachine.DataAccess.Repositories
{
    /// <summary>
    /// Репозиторий для работы с таблицей документов лота
    /// </summary>
    public class LotDocumentRepository : RepositoryBase<LotDocument, Guid>, ILotDocumentRepository
    {
        public LotDocumentRepository(TradingContext context) : base(context)
        {
        }
        
        /// <summary>
        /// Существует ли документ
        /// </summary>
        /// <param name="lotId">Идентификатор лота</param>
        /// <param name="documentType">Тип документа</param>
        /// <returns>Список документов лота</returns>
        public async Task<bool> AnyAsync(Guid lotId, LotDocumentType documentType)
        {
            return await GetAll(asNoTracking: true).AnyAsync(s => s.LotId == lotId && 
                                                               s.Type == documentType);
        }
    }
}