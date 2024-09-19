using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using AutoFixture;
using AutoMapper;
using Moq;
using Trading.StateMachine.BusinessLogic.Managers;
using Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using Trading.StateMachine.BusinessLogic.Managers.Resources;
using Trading.StateMachine.DataAccess.Models;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class LotManagerTests_Data_Initialization: IClassFixture<TestFixture>
    {
        private ILotManager lotManager;
        private Mock<IProcedureTypeRepository> _procedureTypeRepositoryMock
            = new Mock<IProcedureTypeRepository>();
        
        public LotManagerTests_Data_Initialization(TestFixture testFixture)
        {
            var serviceProvider = testFixture.ServiceProvider;
            lotManager = new LotManager(serviceProvider.GetService<IMapper>(), 
                _procedureTypeRepositoryMock.Object);
            _procedureTypeRepositoryMock.Setup(m => 
                m.GetProcedureNameBy(It.IsAny<string>())).ReturnsAsync("someName");
        }

        [Fact]
        public async Task Validate_Returns_False_If_Organization_Not_Set()
        {
            //Arrange
            var lot = new Lot //инициализация непосредственно в тесте
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
        
        [Fact]
        public async Task Validate_Returns_Success_For_Valid_Data()
        {
            //Arrange
            var lot = GetLot(); //инициализация в отдельном методе

            //Act
            var result = await lotManager.ValidateAsync(lot);

            //Assert
            Assert.True(result.IsSuccessfull);
        }

        [Fact]
        public async Task Validate_Returns_False_If_CreatedOrganizationId_Not_Set()
        {
            //Arrange
            var lot = new LotBuilder() //инициализация строителем
                .WithCreatedUserId(Guid.NewGuid().ToString())
                .Build();

            //Act
            var result = await lotManager.ValidateAsync(lot);

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Single(result.Errors);
            Assert.Equal(Exceptions.Validate_Не_заполнен_идентификатор_организации, result.Errors[0]);
        }
        
        [Fact]
        public async Task Validate_Returns_Success_For_Valid_Data2()
        {
            //Arrange
            Fixture autoFixture = new Fixture();
            
            #region FSetup

            autoFixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => autoFixture.Behaviors.Remove(b));
            autoFixture.Behaviors.Add(new OmitOnRecursionBehavior());


            #endregion
            
            var lot = autoFixture.Create<Lot>();

            //Act
            var result = await lotManager.ValidateAsync(lot);

            //Assert
            Assert.True(result.IsSuccessfull);
        }

        [Fact]
        public async Task Validate_Returns_False_If_CreatedUserId_Not_Set2()
        {
            //Arrange
            var autoFixture = new Fixture();

            #region FSetup

            autoFixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => autoFixture.Behaviors.Remove(b));
            autoFixture.Behaviors.Add(new OmitOnRecursionBehavior());


            #endregion
            
            var lot = autoFixture.Build<Lot>()
                .Without(t => t.CreatedUserId)
                .Create();

            //Act
            var result = await lotManager.ValidateAsync(lot);

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Single(result.Errors);
            Assert.Equal(Exceptions.Validate_Не_заполнен_идентификатор_пользователя, result.Errors[0]);
        }
        
        private Lot GetLot()
        {
            return new Lot
            {
                CreatedOrganizationId = Guid.NewGuid(),
                CreatedUserId = Guid.NewGuid().ToString()
            };
        }
    }
}