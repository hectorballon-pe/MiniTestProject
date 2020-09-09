using Microsoft.Extensions.Configuration;
using MiniProject.BusinessLogic.Models;
using MiniProject.Infrastructure;
using MiniProject.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;

namespace MiniProject.BusinessLogic.Infrastructure.DataSources
{
    class PhotosCache : ModelsCache<Photo>
    {
        public PhotosCache(IConfiguration configuration) : base(configuration)
        {
        }

        protected override IServiceConsumer<Photo> GetServiceConsumer(string endpointAddress)
        {
            return new ServiceConsumer<Photo>(endpointAddress);
        }

        protected override string GetEndPointName() => "Photos";

        protected override void AddItems(IEnumerable<Photo> items)
        {
            var groups = items.GroupBy(album => album.AlbumId);
            foreach (var group in groups)
                CacheManager.Value.Put(group.Key.ToString(), group.AsEnumerable());
        }
    }
}
