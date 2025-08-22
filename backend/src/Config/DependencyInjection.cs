using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyApp.Services;
using MyApp.Repositories;
using MyApp.Data;

namespace MyApp.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurar o Entity Framework com SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Injeção de dependência dos serviços e repositórios
            services.AddScoped<IVendaService, VendaService>();
            services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();

            return services;
        }
    }
}
