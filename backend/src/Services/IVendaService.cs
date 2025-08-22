using MyApp.Models;

namespace MyApp.Services
{
    public interface IVendaService
    {
        CriarOrdemServicoResponse CriarOrdemServico(CriarOrdemServicoRequest request);
    }
}
