using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StateMachine.Compose;
using Trading.StateMachine.BusinessLogic.Managers;
using Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using Trading.StateMachine.BusinessLogic.Managers.Resources;
using Trading.StateMachine.DataAccess.Models;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;
using Xunit;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class LotManagerTests_IOC: IDisposable
    {
        private ILotManager lotManager;
        private Mock<IProcedureTypeRepository> _procedureTypeRepositoryMock
            = new Mock<IProcedureTypeRepository>();
        public LotManagerTests_IOC()
        {
            var builder = new ConfigurationBuilder();
            var configuration = builder.Build();
            var serviceCollection = Configuration.GetServiceCollection(configuration, "Tests");
            var serviceProvider = serviceCollection
                .BuildServiceProvider();
            
            _procedureTypeRepositoryMock.Setup(m => 
                m.GetProcedureNameBy(It.IsAny<string>())).ReturnsAsync("someName");
            
            lotManager = new LotManager(serviceProvider.GetService<IMapper>(), 
                _procedureTypeRepositoryMock.Object);
        }

        [Fact]
        public async Task Validate_Returns_Success_For_Valid_Data()
        {
            //Arrange
            var lot = new Lot
            {
                CreatedUserId = Guid.NewGuid().ToString(),
                CreatedOrganizationId = Guid.NewGuid()
            };

            //Act
            var result = await lotManager.ValidateAsync(lot);

            //Assert
            Assert.True(result.IsSuccessfull);
        }
        
        [Fact]
        public async Task Validate_Returns_False_If_Organization_Not_Set()
        {
            //Arrange
            var lot = new Lot
            {
                CreatedUserId = Guid.NewGuid().ToString()
            };

            //Act
            var result = await lotManager.ValidateAsync(lot);

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Single(result.Errors);
            Assert.Equal(Exceptions.Validate_Не_заполнен_идентификатор_организации, result.Errors[0]);
        }
        
        public void Dispose()
        {
        }
    }
}