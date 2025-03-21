using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Trading.StateMachine.BusinessLogic.Managers;
using Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using Trading.StateMachine.BusinessLogic.Managers.Resources;
using Trading.StateMachine.DataAccess.Models;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;
using Xunit;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class LotManagerTests_Data_Parametrization: IClassFixture<TestFixture>
    {
        private ILotManager lotManager;
        private ILogger logger;
        private Mock<IProcedureTypeRepository> _procedureTypeRepositoryMock
            = new Mock<IProcedureTypeRepository>();
        
        public LotManagerTests_Data_Parametrization(TestFixture testFixture)
        {
            var serviceProvider = testFixture.ServiceProvider;
            lotManager = new LotManager(serviceProvider.GetService<IMapper>(), 
                _procedureTypeRepositoryMock.Object);
            _procedureTypeRepositoryMock.Setup(m => 
                m.GetProcedureNameBy(It.IsAny<string>())).ReturnsAsync("someName");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Validate_Returns_False_If_Created_User_Id_Is_Not_Set(string userId)
        {
            //Arrange
            var lot = new Lot
            {
                CreatedUserId = userId,
                CreatedOrganizationId = Guid.NewGuid()
            };

            //Act
            var result = await lotManager.ValidateAsync(lot);

            //Assert
            Assert.False(result.IsSuccessful);
            Assert.Single(result.Errors);
            Assert.Equal(Exceptions.Validate_Не_заполнен_идентификатор_пользователя, result.Errors[0]);
        }
    }
}