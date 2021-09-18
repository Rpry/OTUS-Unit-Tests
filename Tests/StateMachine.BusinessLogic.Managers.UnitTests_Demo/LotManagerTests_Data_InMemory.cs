using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using Trading.StateMachine.BusinessLogic.Managers.Resources;
using Trading.StateMachine.DataAccess.Models;
using Xunit;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class LotManagerTests_Data_InMemory: IClassFixture<TestFixture_InMemory>
    {
        private ILotManager lotManager;

        public LotManagerTests_Data_InMemory(TestFixture_InMemory testFixture)
        {
            var serviceProvider = testFixture.ServiceProvider;
            lotManager = serviceProvider.GetService<ILotManager>();
        }

        [Fact]
        public async Task Validate_Returns_True_For_Valid_Data()
        {
            //Arrange
            var lot = new Lot 
            {
                CreatedUserId = Guid.NewGuid().ToString(),
                CreatedOrganizationId = Guid.NewGuid(),
                ProcedureCode = "MyProperty1"
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
                CreatedUserId = Guid.NewGuid().ToString(),
                ProcedureCode = "MyProperty1"
            };

            //Act
            var result = await lotManager.ValidateAsync(lot);

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Single(result.Errors);
            Assert.Equal(Exceptions.Validate_Не_заполнен_идентификатор_организации, result.Errors[0]);
        }
        
        [Fact]
        public async Task  Validate_Returns_False_If_CreatedUserId_Not_Set()
        {
            //Arrange
            var lot = new Lot
            {
                CreatedOrganizationId = Guid.NewGuid(),
                ProcedureCode = "MyProperty1"
            };
            
            //Act
            var result = await lotManager.ValidateAsync(lot);

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Single(result.Errors);
            Assert.Equal(Exceptions.Validate_Не_заполнен_идентификатор_пользователя, result.Errors[0]);
        }
    }
}