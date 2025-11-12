using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.Backend.Services;
using ContaVida.MVC.Security.Core;
using ContaVida.MVC.Security.Infraestructure;

namespace ContaVida.MVC.Server
{
    public static class ServiceInjector
    {

        public static void InjectServices(this IServiceCollection services)
        {
            services.AddTransient<IAccountUserService, AccountUserService>();
            services.AddTransient<IEncryptCore, EncryptCore>();
            services.AddTransient<IDecryptCore, DecryptCore>();
        }

    }
}
