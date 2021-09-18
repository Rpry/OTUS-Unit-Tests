using System;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.Extensions.DependencyInjection;
using Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using Trading.StateMachine.BusinessLogic.Managers.Resources;
using Trading.StateMachine.DataAccess.Models;
using Xunit;
using Trading.StateMachine.BusinessLogic.Exceptions;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class LotDocumentServiceTests_InMemory: IClassFixture<TestFixture_InMemory>
    {
        private ILotDocumentService _lotDocumentService;
        private ILotRepository _lotRepository;
        private ILotDocumentRepository _lotDocumentRepository;

        public LotDocumentServiceTests_InMemory(TestFixture_InMemory testFixture)
        {
            var serviceProvider = testFixture.ServiceProvider;
            _lotDocumentService = serviceProvider.GetService<ILotDocumentService>();
            _lotDocumentRepository = serviceProvider.GetService<ILotDocumentRepository>();
            _lotRepository = serviceProvider.GetService<ILotRepository>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Create_Should_Throw_Exception_If_Name_Not_Set(string name)
        {
            //Arrange
            var lotId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var type = LotDocumentType.Protocol1;

            //Act
            var result = await Assert.ThrowsAsync<ArgumentException>(async ()=> await _lotDocumentService.CreateAsync(lotId, fileId, type, name));

            //Assert
            Assert.Equal(Exceptions.LotDocumentService_CreateAsync_Имя_лота_не_передано, result.Message);
        }
        
        [Fact]
        public async Task Create_Should_Throw_Exception_If_Lot_Is_Not_Found()
        {
            //Arrange
            var lotId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var type = LotDocumentType.Protocol1;
            var name = "1";

            //Act
            //Assert
            await Assert.ThrowsAsync<LotNotFoundException>(async ()=> await _lotDocumentService.CreateAsync(lotId, fileId, type, name));
        }
        
        [Fact]
        public async Task Create_Should_Throw_Exception_If_LotDocument_Exists()
        {
            //Arrange
            var lotId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var type = LotDocumentType.Protocol1;
            var name = "1";

            _lotRepository.Add(new Lot
            {
                Id = lotId
            });
            await _lotRepository.SaveChangesAsync();

            _lotDocumentRepository.Add(new LotDocument
            {
                LotId = lotId,
                Type = type
            });
            await _lotDocumentRepository.SaveChangesAsync();

            //Act
            //Assert
            await Assert.ThrowsAsync<BusinessLogicException>(async ()=> await _lotDocumentService.CreateAsync(lotId, fileId, type, name));
        }
        
        [Fact]
        public async Task Create_Should_Add_LotDocument_Successfully()
        {
            //Arrange
            var lotId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var type = LotDocumentType.Protocol1;
            var name = "1";

            _lotRepository.Add(new Lot
            {
                Id = lotId
            });
            await _lotRepository.SaveChangesAsync();

            //Act
            var result = await _lotDocumentService.CreateAsync(lotId, fileId, type, name);
            
            //Assert
            Assert.NotEqual(Guid.Empty, result);
        }
    }
}