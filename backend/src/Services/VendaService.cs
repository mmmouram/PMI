using System;
using MyApp.Models;
using MyApp.Repositories;

namespace MyApp.Services
{
    public class VendaService : IVendaService
    {
        private readonly IOrdemServicoRepository _ordemServicoRepository;
        private readonly ILogger<VendaService> _logger;

        public VendaService(IOrdemServicoRepository ordemServicoRepository, ILogger<VendaService> logger)
        {
            _ordemServicoRepository = ordemServicoRepository;
            _logger = logger;
        }

        public CriarOrdemServicoResponse CriarOrdemServico(CriarOrdemServicoRequest request)
        {
            // Validação de consistência dos dados da venda
            if (request.ValorTotal <= 0)
            {
                return new CriarOrdemServicoResponse
                {
                    Sucesso = false,
                    Mensagem = "Valor total deve ser maior que zero."
                };
            }

            // Gerar código único para a Ordem de Serviço
            var codigoOrdem = Guid.NewGuid().ToString();

            // Criar objeto de OrdemServico para persistência e integração com SAP
            var ordemServico = new Models.OrdemServico
            {
                CodigoOrdem = codigoOrdem,
                IdCliente = request.IdCliente,
                DetalhesVenda = request.DetalhesVenda,
                ValorTotal = request.ValorTotal,
                DataCriacao = DateTime.UtcNow
            };

            // Registrar a ordem de serviço internamente e realizar a integração com SAP
            var sucessoIntegracao = _ordemServicoRepository.RegistrarOrdemServico(ordemServico);

            if (sucessoIntegracao)
            {
                _logger.LogInformation($"Ordem de Serviço criada com sucesso. Código: {codigoOrdem}");
                return new CriarOrdemServicoResponse
                {
                    Sucesso = true,
                    Mensagem = "Ordem de serviço criada e registrada no SAP com sucesso.",
                    CodigoOrdem = codigoOrdem
                };
            }
            else
            {
                _logger.LogWarning($"Falha ao integrar com SAP. Código da ordem: {codigoOrdem}");
                return new CriarOrdemServicoResponse
                {
                    Sucesso = false,
                    Mensagem = "Falha na integração com o SAP."
                };
            }
        }
    }
}
