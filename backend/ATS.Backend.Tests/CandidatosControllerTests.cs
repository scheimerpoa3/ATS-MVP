using ATS.Backend.Controllers;
using ATS.Backend.Models;
using ATS.Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ATS.Backend.Tests
{
    public class CandidatosControllerTests
    {
        private readonly Mock<ICandidatoRepository> _mockRepo;
        private readonly Mock<ILogger<CandidatosController>> _mockLogger;
        private readonly CandidatosController _controller;

        public CandidatosControllerTests()
        {
            _mockRepo = new Mock<ICandidatoRepository>();
            _mockLogger = new Mock<ILogger<CandidatosController>>();
            _controller = new CandidatosController(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Get_ReturnsAllCandidatos()
        {
            // Arrange
            var lista = new List<Candidato>
            {
                new Candidato { Id = "1", Nome = "Teste1", Email = "teste1@mail.com" },
                new Candidato { Id = "2", Nome = "Teste2", Email = "teste2@mail.com" }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(lista);

            // Act
            var result = await _controller.Get() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(lista, result.Value);
        }

        [Fact]
        public async Task GetById_ReturnsCandidato_WhenFound()
        {
            // Arrange
            var candidato = new Candidato { Id = "1", Nome = "Teste1", Email = "teste1@mail.com" };
            _mockRepo.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(candidato);

            // Act
            var result = await _controller.GetById("1") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(candidato, result.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync("1")).ReturnsAsync((Candidato)null);

            // Act
            var result = await _controller.GetById("1");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedCandidato()
        {
            // Arrange
            var candidato = new Candidato { Nome = "Novo", Email = "novo@mail.com" };
            _mockRepo.Setup(r => r.CreateAsync(candidato)).ReturnsAsync(candidato);

            // Act
            var result = await _controller.Create(candidato) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(candidato, result.Value);
        }

        [Fact]
        public async Task Update_ReturnsNoContent()
        {
            // Arrange
            var candidato = new Candidato { Nome = "Atualizado", Email = "atualizado@mail.com" };

            // Act
            var result = await _controller.Update("1", candidato);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockRepo.Verify(r => r.UpdateAsync("1", candidato), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Act
            var result = await _controller.Delete("1");

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockRepo.Verify(r => r.DeleteAsync("1"), Times.Once);
        }
    }
}
