using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WlConsultings.BankChallenge.Application.Interfaces;
using WlConsultings.BankChallenge.Application.Services;
using WlConsultings.BankChallenge.Domain.Interfaces.Repository;
using WlConsultings.BankChallenge.Infra.Configuration;
using WlConsultings.BankChallenge.Infra.Repositories;

namespace WlConsultings.BankChallenge.IoC
{
    public static class ServiceExtensions
    {
        public static void AddBankChallengeServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton(configuration);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITransferRepository, TransferRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<IValidatorService, ValidatorService>();

            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseNpgsql(configuration.GetConnectionString("WL_CONN_STRING"))
            );
        }
    }
}
