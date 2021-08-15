using Application;
using Infrastructure;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddMyCrwaler(this IServiceCollection services)
        {
            services.AddScoped<ICrwaler, CrwalerFactory>();
            return services;
        }

        public static IServiceCollection AddMyRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepository, StoreToCsv>();
            return services;
        }

        public static IServiceCollection AddMyAppService(this IServiceCollection services)
        {
            services.AddScoped<IAppService, AppService>();
            return services;
        }

        public static IAppService GetCrawlerDotNet5(this IServiceProvider provider)
        {
            return provider.GetRequiredService<IAppService>();
        }
    }
}
