using System;
using MyApp.Models;
using MyApp.Data;
using Microsoft.Extensions.Logging;

namespace MyApp.Repositories
{
    public class OrdemServicoRepository : IOrdemServicoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrdemServicoRepository> _logger;

        public OrdemServicoRepository(ApplicationDbContext context, ILogger<OrdemServicoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public bool RegistrarOrdemServico(OrdemServico ordemServico)
        {
            try
            {
                // Persistir a ordem de serviço no banco de dados
                _context.OrdemServicos.Add(ordemServico);
                _context.SaveChanges();

                // Simulação de integração com o SAP
                _logger.LogInformation("Integrando com o SAP para registrar a OS.");
                // Em um cenário real, aqui haveria uma chamada externa para o SAP
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar ordem de serviço.");
                return false;
            }
        }
    }
}
