using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Moq;
using MyApp.Controllers;
using MyApp.Services;
using MyApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MyApp.Tests.Controllers
{
    [TestFixture]
    public class OrdemServicoControllerTests
    {
        private Mock<IVendaService> _vendaServiceMock;
        private Mock<ILogger<OrdemServicoController>> _loggerMock;
        private OrdemServicoController _controller;

        [SetUp]
        public void SetUp()
        {
            _vendaServiceMock = new Mock<IVendaService>();
            _loggerMock = new Mock<ILogger<OrdemServicoController>>();
            _controller = new OrdemServicoController(_vendaServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public void CriarOrdemServico_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new CriarOrdemServicoRequest
            {
                IdCliente = 1,
                DetalhesVenda = "Venda de teste",
                ValorTotal = 100.0m
            };

            var expectedResponse = new CriarOrdemServicoResponse
            {
                Sucesso = true,
                Mensagem = "Ordem de serviço criada com sucesso.",
                CodigoOrdem = Guid.NewGuid().ToString()
            };

            _vendaServiceMock.Setup(s => s.CriarOrdemServico(request)).Returns(expectedResponse);

            // Act
            var result = _controller.CriarOrdemServico(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult.Value);
        }

        [Test]
        public void CriarOrdemServico_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("DetalhesVenda", "Campo obrigatório");
            var request = new CriarOrdemServicoRequest();

            // Act
            var result = _controller.CriarOrdemServico(request);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CriarOrdemServico_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CriarOrdemServicoRequest
            {
                IdCliente = 1,
                DetalhesVenda = "Teste Exception",
                ValorTotal = 150.0m
            };

            _vendaServiceMock.Setup(s => s.CriarOrdemServico(request)).Throws(new Exception("Erro de teste"));

            // Act
            var result = _controller.CriarOrdemServico(request);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Erro interno no servidor", objectResult.Value);
        }
    }
}
