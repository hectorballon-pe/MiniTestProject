using Microsoft.Extensions.Configuration;
using MiniProject.Infrastructure;
using MiniProject.Infrastructure.Services;
using MiniProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace MiniProject.BusinessLogic.Infrastructure.DataSources
{
    class AlbumsCache : ModelsCache<Album>
    {
        public AlbumsCache(IConfiguration configuration) : base(configuration)
        {
        }

        protected override IServiceConsumer<Album> GetServiceConsumer(string endpointAddress)
        {
            return new ServiceConsumer<Album>(endpointAddress);
        }

        protected override string GetEndPointName() => "Albums";

        protected override void AddItems(IEnumerable<Album> items)
        {
            var groups = items.GroupBy(album => album.UserId);
            foreach (var group in groups)
                CacheManager.Value.Put(group.Key.ToString(), group.AsEnumerable());
        }
    }
}
