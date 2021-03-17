using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WSUserAccountManager.Abstractions;
using WSUserAccountManager.Builder;
using WSUserAccountManager.Database;
using WSUserAccountManager.Handlers;
using WSUserAccountManager.Middlewares;
using WSUserAccountManager.Services.Cryptography;
using WSUserAccountManager.Services.UserAccount;

namespace WSUserAccountManager.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            // Singletons
            services.AddSingleton<WebSocketMiddleware>();
            services.AddSingleton<RequestDelegate>();
            services.AddSingleton(MapperBuilder.Build());

            // Scoped
            services.AddScoped<UserAccountHandler>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IHashFunction, HashFunction>();
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IVerificationService, VerificationService>();
            services.AddScoped<ISaltService, SaltService>();
            services.AddScoped<ISessionService, SessionService>();
        }
    }
}
