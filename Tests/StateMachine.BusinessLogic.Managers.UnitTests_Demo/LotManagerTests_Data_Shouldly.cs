using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using Shouldly;
using Trading.StateMachine.BusinessLogic.Managers;
using Trading.StateMachine.BusinessLogic.Managers.Abstraction;
using Trading.StateMachine.BusinessLogic.Managers.Resources;
using Trading.StateMachine.DataAccess.Repositories.Abstraction;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class LotManagerTests_Data_Shouldly: IClassFixture<TestFixture>
    {
        private ILotManager lotManager;
        private Mock<IProcedureTypeRepository> _procedureTypeRepositoryMock
            = new Mock<IProcedureTypeRepository>();

        public LotManagerTests_Data_Shouldly(TestFixture testFixture)
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
            var lot = new LotBuilder() 
                .WithCreatedUserId(Guid.NewGuid().ToString())
                .Build();

            //Act
            var result = await lotManager.ValidateAsync(lot);

            //Assert
            result.IsSuccessfull.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ShouldBe(Exceptions.Validate_Не_заполнен_идентификатор_организации);
        }
        
        [Fact]
        public async Task Validate_Returns_False_If_CreatedUserId_Not_Set()
        {
            //Arrange
            var lot = new LotBuilder() //инициализация строителем
                .WithCreatedOrganizationId(Guid.NewGuid())
                .Build();

            //Act
            var result = await lotManager.ValidateAsync(lot);

            //Assert
            result.IsSuccessfull.Should().BeFalse();
            result.Errors.Should()
                .HaveCount(1)
                .And
                .Subject.ShouldContain(Exceptions.Validate_Не_заполнен_идентификатор_пользователя);
        }
    }
}