using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Trading.StateMachine.BusinessLogic.Managers;
using Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using Trading.StateMachine.DataAccess.Models;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class LotServiceTests_Mock
    {
        private ILotService lotService;
     
        private Mock<IProcedureTypeRepository> _procedureTypeRepositoryMock
            = new Mock<IProcedureTypeRepository>();

        public LotServiceTests_Mock()
        {
            lotService = new LotService(_procedureTypeRepositoryMock.Object);
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
            var result = await lotService.ValidateAsync(lot);

            //Assert
            Assert.True(result.IsSuccessful);
        }
    }
}