using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StateMachine.BusinessLogic.Managers.UnitTests_Demo;
using WebApi;
using WebApi.Controllers;
using Xunit;

namespace WebApiTests
{
    public class WeatherForecastTests: IClassFixture<TestFixture>
    {
        private WeatherForecastController _weatherForecastController;
        private Mock<ILogger<WeatherForecastController>> loggerMock = new Mock<ILogger<WeatherForecastController>>(); 
        public WeatherForecastTests(TestFixture testFixture)
        {
            var serviceProvider = testFixture.ServiceProvider;
            _weatherForecastController = new WeatherForecastController(loggerMock.Object);
        }

        [Fact]
        public void Validate_Returns_Success_For_Valid_Data()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            
            //Act
            var result = _weatherForecastController.Get(id);

            //Assert
            var okRequestResult = Assert.IsType<OkObjectResult>(result);
            var okObjectResult = Assert.IsType<WeatherForecast>(okRequestResult.Value);
            Assert.Equal(id, okObjectResult.Id);
        }
        
        [Fact]
        public void Validate_Returns_Error_If_Id_Is_Empty()
        {
            //Arrange
            Guid id = Guid.Empty;
            
            //Act
            var result = _weatherForecastController.Get(id);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(Resources.WeatherForecastController_Get_Не_передан_идентификатор, badRequestResult.Value);
        }
    }
}