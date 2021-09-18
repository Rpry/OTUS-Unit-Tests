using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using AutoMapper;
using Moq;
using Trading.StateMachine.BusinessLogic.Managers;
using Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using Trading.StateMachine.DataAccess.Models;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class LotManagerTests_Data_Behavior: IClassFixture<TestFixture>
    {
        private ILotManager lotManager;
        private Mock<IProcedureTypeRepository> _procedureTypeRepositoryMock
            = new Mock<IProcedureTypeRepository>();
        
        public LotManagerTests_Data_Behavior(TestFixture testFixture)
        {
            var serviceProvider = testFixture.ServiceProvider;
            lotManager = new LotManager(
                serviceProvider.GetService<IMapper>(),
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

            _procedureTypeRepositoryMock.Setup(m => 
                m.GetProcedureNameBy(It.IsAny<string>())).ReturnsAsync("someName");

            //Act
            await lotManager.ValidateAsync(lot);

            //Assert
            _procedureTypeRepositoryMock.Verify(m=> m.GetProcedureNameBy(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}