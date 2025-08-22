using Microsoft.AspNetCore.Mvc;
using MyApp.Services;
using MyApp.Models;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdemServicoController : ControllerBase
    {
        private readonly IVendaService _vendaService;
        private readonly ILogger<OrdemServicoController> _logger;

        public OrdemServicoController(IVendaService vendaService, ILogger<OrdemServicoController> logger)
        {
            _vendaService = vendaService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CriarOrdemServico([FromBody] CriarOrdemServicoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = _vendaService.CriarOrdemServico(request);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar Ordem de Servico");
                return StatusCode(500, "Erro interno no servidor");
            }
        }
    }
}
