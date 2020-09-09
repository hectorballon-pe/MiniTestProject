using Microsoft.Extensions.Configuration;
using MiniProject.BusinessLogic.Models;
using MiniProject.Infrastructure;
using MiniProject.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;

namespace MiniProject.BusinessLogic.Infrastructure.DataSources
{
    class CommentsCache : ModelsCache<Comment>
    {
        public CommentsCache(IConfiguration configuration) : base(configuration)
        {
        }

        protected override IServiceConsumer<Comment> GetServiceConsumer(string endpointAddress)
        {
            return new ServiceConsumer<Comment>(endpointAddress);
        }

        protected override string GetEndPointName() => "Comments";

        protected override void AddItems(IEnumerable<Comment> items)
        {
            var groups = items.GroupBy(album => album.PostId);
            foreach (var group in groups)
                CacheManager.Value.Put(group.Key.ToString(), group.AsEnumerable());
        }
    }
}
