using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MiniProject.BusinessLogic.Infrastructure.DataSources;
using System;

namespace MiniProject.BusinessLogic
{
    public static class MyServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<AlbumsCache>();
            services.TryAddSingleton<PhotosCache>();
            services.TryAddSingleton<CommentsCache>();

            return services;
        }
    }
}
