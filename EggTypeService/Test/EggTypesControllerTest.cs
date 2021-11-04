using AutoMapper;
using Dapr.Client;
using EggTypeService.Controllers;
using EggTypeService.Data;
using EggTypeService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace Test
{
    public class EggTypesControllerTest
    {
        [Fact]
        public void GetEggTypeById_WithUnexistingEggType_ReturnsNotFound()           //naming convention tutorial: UnitOfWork_StateUnderTest_ExpectedBehaviour()
        {
            // Arrange
            var repositoryStub = new Mock<IEggTypeRepo>(); // "Stub" omdat je geen behaviour van de repo gaat testen (zou je bij een Mock wel doen)
            repositoryStub.Setup(repo => repo.GetEggTypeById(It.IsAny<int>()))
                .Returns((EggType)null);
            var mapperStub = new Mock<IMapper>();
            var daprClientStud = new Mock<DaprClient>();

            var controller = new EggTypesController(repositoryStub.Object, mapperStub.Object, daprClientStud.Object);

            // Act
            var random = new Random();
            var result = controller.GetEggTypeById(random.Next());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);  //nb: we testen hier dus alleen maar of er een notFound verschijnt als een eggType niet gevonden wordt (niet het vinden zelf)

        }
    }
}
