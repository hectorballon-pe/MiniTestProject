using MediatR;
using MiniProject.Models;
using System.Collections.Generic;

namespace MiniProject.BusinessLogic.Commands
{
    public class GetAllAlbumsByUserRequest : IRequest<IEnumerable<Album>>
    {
        public GetAllAlbumsByUserRequest(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; }
    }
}
