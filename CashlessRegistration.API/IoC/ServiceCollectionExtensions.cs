using CashlessRegistration.API.Domain.Contracts.Repositories;
using CashlessRegistration.API.Domain.Contracts.Services;
using CashlessRegistration.API.Domain.Services;
using CashlessRegistration.API.Infra.Data.Connections;
using CashlessRegistration.API.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CashlessRegistration.API.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddSingleton<IWriterService, ConsoleWriterService>();

        public static IServiceCollection AddRepositories(this IServiceCollection services) => services
            .AddSingleton<IConnection, InMemoryConnection>()
            .AddSingleton<ICardRepository, CardRepository>();
    }
}