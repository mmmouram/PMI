using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using MyApp.Services;
using MyApp.Repositories;
using MyApp.Models;
using System;

namespace MyApp.Tests.Services
{
    [TestFixture]
    public class VendaServiceTests
    {
        private Mock<IOrdemServicoRepository> _ordemServicoRepositoryMock;
        private Mock<ILogger<VendaService>> _loggerMock;
        private VendaService _vendaService;

        [SetUp]
        public void SetUp()
        {
            _ordemServicoRepositoryMock = new Mock<IOrdemServicoRepository>();
            _loggerMock = new Mock<ILogger<VendaService>>();
            _vendaService = new VendaService(_ordemServicoRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public void CriarOrdemServico_InvalidValorTotal_ReturnsFailureResponse()
        {
            // Arrange
            var request = new CriarOrdemServicoRequest
            {
                IdCliente = 1,
                DetalhesVenda = "Venda Teste",
                ValorTotal = 0 // Valor inválido
            };

            // Act
            var response = _vendaService.CriarOrdemServico(request);

            // Assert
            Assert.IsFalse(response.Sucesso);
            Assert.AreEqual("Valor total deve ser maior que zero.", response.Mensagem);
        }

        [Test]
        public void CriarOrdemServico_SuccessfulIntegration_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new CriarOrdemServicoRequest
            {
                IdCliente = 1,
                DetalhesVenda = "Venda Teste",
                ValorTotal = 200.0m
            };

            _ordemServicoRepositoryMock.Setup(r => r.RegistrarOrdemServico(It.IsAny<OrdemServico>())).Returns(true);

            // Act
            var response = _vendaService.CriarOrdemServico(request);

            // Assert
            Assert.IsTrue(response.Sucesso);
            Assert.AreEqual("Ordem de serviço criada e registrada no SAP com sucesso.", response.Mensagem);
            Assert.IsNotNull(response.CodigoOrdem);
        }

        [Test]
        public void CriarOrdemServico_FailedIntegration_ReturnsFailureResponse()
        {
            // Arrange
            var request = new CriarOrdemServicoRequest
            {
                IdCliente = 1,
                DetalhesVenda = "Falha SAP",
                ValorTotal = 300.0m
            };

            _ordemServicoRepositoryMock.Setup(r => r.RegistrarOrdemServico(It.IsAny<OrdemServico>())).Returns(false);

            // Act
            var response = _vendaService.CriarOrdemServico(request);

            // Assert
            Assert.IsFalse(response.Sucesso);
            Assert.AreEqual("Falha na integração com o SAP.", response.Mensagem);
        }
    }
}
