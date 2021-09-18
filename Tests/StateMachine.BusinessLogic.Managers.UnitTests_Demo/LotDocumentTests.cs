using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Trading.StateMachine.BusinessLogic.Exceptions;
using Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using Trading.StateMachine.BusinessLogic.Managers.Resources;
using Trading.StateMachine.DataAccess.Models;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;
using Xunit;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class LotDocumentTests: IClassFixture<TestFixture_InMemory>
    {
        private ILotDocumentService lotDocumentService;
        private ILotRepository lotRepository;
        private ILotDocumentRepository _lotDocumentRepository;

        public LotDocumentTests(TestFixture_InMemory testFixture)
        {
            var serviceProvider = testFixture.ServiceProvider;
            lotDocumentService = serviceProvider.GetService<ILotDocumentService>();
            lotRepository = serviceProvider.GetService<ILotRepository>();
            _lotDocumentRepository = serviceProvider.GetService<ILotDocumentRepository>();
        }

        [Fact]
        public async Task Create_Should_Throw_Exception_If_Lot_Not_Found()
        {
            //Arrange
            var fixture = new Fixture();
            var documentName = fixture.Create<string>();
            var lotId = fixture.Create<Guid>();
            var fileId = fixture.Create<Guid>();
            var type = fixture.Create<LotDocumentType>();

            //Act
            //Assert
            var ex = await Should.ThrowAsync<LotNotFoundException>(async ()=> await lotDocumentService.CreateAsync(lotId, fileId, type, documentName));
            
            ex.Message.ShouldBe($"Лот с идентификатором {lotId} не найден");
        }
        
        [Fact]
        public async Task Create_Should_Create_Document_In_Lot()
        {
            //Arrange
            var fixture = new Fixture();
            var documentName = "";
            //var lotId = fixture.Create<Guid>();
            var fileId = fixture.Create<Guid>();
            var type = fixture.Create<LotDocumentType>();

            var lot = new Lot();
            var createdLot = await lotRepository.AddAsync(lot);
            await lotRepository.SaveChangesAsync();

            //Act
            var result = await Should.ThrowAsync<ArgumentException>(async () => await lotDocumentService.CreateAsync(createdLot.Id, fileId, type, documentName));

            //Assert
            result.Should().NotBe(Guid.Empty);
        }
        
        [Fact]
        public async Task Create_Should_Throw_Exception_If_Document_Exists()
        {
            //Arrange
            var fixture = new Fixture();
            var documentName = fixture.Create<string>();
            var fileId = fixture.Create<Guid>();
            var type = fixture.Create<LotDocumentType>();

            var lot = new Lot();
            var createdLot = await lotRepository.AddAsync(lot);
            await lotRepository.SaveChangesAsync();

            var lotDocument = new LotDocument
            {
                LotId = lot.Id,
                Type = type
            };
            await _lotDocumentRepository.AddAsync(lotDocument);
            await _lotDocumentRepository.SaveChangesAsync();
            
            //Act
            var ex = Should.Throw<BusinessLogicException>(async ()=> await lotDocumentService.CreateAsync(createdLot.Id, fileId, type, documentName));

            //Assert
            ex.Message.ShouldBe(string.Format(Exceptions.LotDocumentService_Create_Документ_переданного_типа_для_лота__0__уже_существует, lot.Id));
        }
    }
}