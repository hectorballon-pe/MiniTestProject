using MediatR;
using MiniProject.BusinessLogic.Models;
using System.Collections.Generic;

namespace MiniProject.BusinessLogic.Commands
{
    public class GetAllPhotosByAlbumRequest : IRequest<IEnumerable<Photo>>
    {
        public GetAllPhotosByAlbumRequest(int albumId)
        {
            AlbumId = albumId;
        }

        public int AlbumId { get; }
    }
}
