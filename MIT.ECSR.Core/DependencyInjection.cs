using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Shared.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ECSR.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterCore(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMediatR(Assembly.GetExecutingAssembly());

            var type = typeof(DependencyInjection);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(d => d.FullName.StartsWith(type.Namespace!)).SelectMany(d => d.DefinedTypes);

            ConstantApplication.BaseUrl = configuration.GetSection("ApplicationConfig")["APIURL"];
            ConstantApplication.MediaUrl = ConstantApplication.BaseUrl + "/" + configuration.GetSection("ApplicationConfig")["MediaPath"];
            ConstantApplication.MediaPath = Path.Combine(Directory.GetCurrentDirectory(), configuration.GetSection("ApplicationConfig")["MediaPath"]);

            var interfaces = assemblies.Where(d => d.IsInterface).ToList();
            foreach (var @interface in interfaces)
            {
                var @class = assemblies.Where(x => @interface.IsAssignableFrom(x) && !x.IsInterface)
                                       .OrderByDescending(x => x.Name).FirstOrDefault();
                if (@class != null)
                {
                    services.AddTransient(@interface, @class);
                }
            }
            return services;
        }
    }
}
