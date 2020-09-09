using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniProject.BusinessLogic.Commands;
using MiniProject.BusinessLogic.Infrastructure.DataSources;
using MiniProject.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniProject.BusinessLogic.Handlers
{
    class GetAllCommentsByPhotoHandler : IRequestHandler<GetAllCommentsByPhotoRequest, IEnumerable<Comment>>
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public GetAllCommentsByPhotoHandler(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public Task<IEnumerable<Comment>> Handle(GetAllCommentsByPhotoRequest request, CancellationToken cancellationToken)
        {
            var cache = _serviceProvider.GetService<CommentsCache>();
            return Task.FromResult(cache.TryGetItemsByKey(request.PhotoId.ToString()));
        }
    }
}
