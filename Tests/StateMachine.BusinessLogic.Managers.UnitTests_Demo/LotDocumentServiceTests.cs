using System;
using System.Threading.Tasks;
using AutoFixture;
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
    public class LotDocumentServiceTests: IClassFixture<TestFixture_InMemory>
    {
        private ILotDocumentService _lotDocumentService;
        private ILotRepository _lotRepository;
        private ILotDocumentRepository _lotDocumentRepository;

        Fixture autoFixture;
        
        public LotDocumentServiceTests(TestFixture_InMemory testFixture)
        {
            var serviceProvider = testFixture.ServiceProvider;
            _lotDocumentService = serviceProvider.GetService<ILotDocumentService>();
            _lotRepository = serviceProvider.GetService<ILotRepository>();
            _lotDocumentRepository = serviceProvider.GetService<ILotDocumentRepository>();
            autoFixture = new Fixture();
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Create_Should_Throw_Exception_If_Name_Is_Not_Set(string name)
        {
            //Arrange
            var lotId = autoFixture.Create<Guid>();
            var fileId = autoFixture.Create<Guid>();
            var type = autoFixture.Create<LotDocumentType>();
            
            //Act
            var exception = await _lotDocumentService.CreateAsync(lotId, fileId, type, name)
                .ShouldThrowAsync<ArgumentException>();

            //Assert
            exception.Message.ShouldBe(Exceptions.LotDocumentService_CreateAsync_Имя_лота_не_передано);
        }

        [Fact]
        public async Task Create_Should_Throw_Exception_If_Lot_Is_Not_Found()
        {
            //Arrange
            var name = autoFixture.Create<string>();
            var lotId = autoFixture.Create<Guid>();
            var fileId = autoFixture.Create<Guid>();
            var type = autoFixture.Create<LotDocumentType>();
            
            //Act
            var exception = await _lotDocumentService.CreateAsync(lotId, fileId, type, name)
                .ShouldThrowAsync<LotNotFoundException>();

            //Assert
            exception.Message.ShouldBe($"Лот с идентификатором {lotId} не найден");
        }
        
        [Fact]
        public async Task Create_Should_Throw_Exception_If_LotDocument_Already_Exists()
        {
            //Arrange
            var name = autoFixture.Create<string>();
            var fileId = autoFixture.Create<Guid>();
            var type = autoFixture.Create<LotDocumentType>();

            var lot = new Lot();

            var createdLot = await _lotRepository.AddAsync(lot);
            await _lotRepository.SaveChangesAsync();

            var lotDocument = new LotDocument
            {
                Type = type,
                LotId = createdLot.Id
            }; 
            await _lotDocumentRepository.AddAsync(lotDocument);
            await _lotDocumentRepository.SaveChangesAsync();

            //Act
            var exception = await _lotDocumentService.CreateAsync(createdLot.Id, fileId, type, name)
                .ShouldThrowAsync<BusinessLogicException>();

            //Assert
            exception.Message.ShouldBe(string.Format(Exceptions.LotDocumentService_Create_Документ_переданного_типа_для_лота__0__уже_существует, createdLot.Id));
        }
    }
}