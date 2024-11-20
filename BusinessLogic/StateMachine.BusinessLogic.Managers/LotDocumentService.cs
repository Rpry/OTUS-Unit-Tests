using System;
using System.Threading.Tasks;
using Trading.StateMachine.BusinessLogic.Exceptions;
using Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using Trading.StateMachine.DataAccess.Models;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;

namespace Trading.StateMachine.BusinessLogic.Managers
{
    public class LotDocumentService : ILotDocumentService
    {
        private readonly ILotRepository _lotRepository;
        private readonly ILotDocumentRepository _lotDocumentRepository;

        public LotDocumentService(ILotRepository lotRepository, ILotDocumentRepository lotDocumentRepository)
        {
            _lotRepository = lotRepository;
            _lotDocumentRepository = lotDocumentRepository;
        }

        /// <summary>
        /// Создание документа лота
        /// </summary>
        public async Task<Guid> CreateAsync(Guid lotId, Guid fileId, LotDocumentType type, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.Exceptions.LotDocumentService_CreateAsync_Имя_лота_не_передано);
            }
            if (!await _lotRepository.Any(lotId))
            {
                throw new LotNotFoundException(lotId);
            }
            if (await _lotDocumentRepository.AnyAsync(lotId, type))
            {
                throw new BusinessLogicException(string.Format(Resources.Exceptions.LotDocumentService_Create_Документ_переданного_типа_для_лота__0__уже_существует, lotId));
            }
            var lotDocument = new LotDocument
            {
                Type = type,
                Name = name,
                FileId = fileId,
                Created = DateTime.UtcNow
            };
            var createdDocument = _lotDocumentRepository.Add(lotDocument);
            await _lotDocumentRepository.SaveChangesAsync();
            return createdDocument.Id;
        }
    }
}