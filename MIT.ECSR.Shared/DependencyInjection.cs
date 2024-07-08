using Microsoft.Extensions.DependencyInjection;
using MIT.ECSR.Shared.Interface;
using MIT.ECSR.Shared.Helper;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MIT.ECSR.Shared.Attributes;

namespace MIT.ECSR.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterShared(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApplicationConfig>(options => configuration.Bind(nameof(ApplicationConfig), options));
            services.AddSingleton<IWrapperHelper, WrapperHelper>();
            services.AddSingleton<IGeneralHelper, GeneralHelper>();

            return services;
        }
    }
}
