using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniProject.BusinessLogic.Commands;
using MiniProject.BusinessLogic.Infrastructure.DataSources;
using MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniProject.BusinessLogic.Handlers
{
    class GetAllAlbumsByUserHandler : IRequestHandler<GetAllAlbumsByUserRequest, IEnumerable<Album>>
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public GetAllAlbumsByUserHandler(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public Task<IEnumerable<Album>> Handle(GetAllAlbumsByUserRequest request, CancellationToken cancellationToken)
        {
            var cache = _serviceProvider.GetService<AlbumsCache>();
            return Task.FromResult(cache.TryGetItemsByKey(request.UserId.ToString()));
        }
    }
}
