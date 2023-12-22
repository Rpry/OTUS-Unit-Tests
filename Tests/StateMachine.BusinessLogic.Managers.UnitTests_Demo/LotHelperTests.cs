using System;
using System.Collections.Generic;
using Trading.StateMachine.BusinessLogic.Managers;
using Trading.StateMachine.BusinessLogic.Managers.Resources;
using Trading.StateMachine.DataAccess.Models;
using Xunit;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class LotHelperTests: IDisposable
    {
        public LotHelperTests()
        {
            
        }

        [Fact]
        public void Validate_Returns_True_For_Valid_Data()
        {
            //Arrange
            var lot = new Lot
            {
                CreatedOrganizationId = Guid.NewGuid(),
                CreatedUserId = Guid.NewGuid().ToString()
            };

            //Act
            var result = LotHelper.Validate(lot);

            //Assert
            Assert.True(result.IsSuccessfull);
        }
        
        [Fact]
        public void Validate_Returns_False_If_Organization_Not_Set()
        {
            //Arrange
            var lot = new Lot
            {
                CreatedUserId = Guid.NewGuid().ToString()
            };

            //Act
            var result = LotHelper.Validate(lot);

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Single(result.Errors);
            Assert.Equal(Exceptions.Validate_Не_заполнен_идентификатор_организации, result.Errors[0]);
        }
        
        [Fact]
        public void Validate_Returns_False_If_CreatedUserId_Not_Set()
        {
            //Arrange
            var lot = new Lot
            {
                CreatedOrganizationId = Guid.NewGuid()
            };

            //Act
            var result = LotHelper.Validate(lot);

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Single(result.Errors);
            Assert.Equal(Exceptions.Validate_Не_заполнен_идентификатор_пользователя, result.Errors[0]);
        }

        public void Dispose()
        {
            
        }
    }
}