using MyApp.Models;

namespace MyApp.Repositories
{
    public interface IOrdemServicoRepository
    {
        bool RegistrarOrdemServico(OrdemServico ordemServico);
    }
}
