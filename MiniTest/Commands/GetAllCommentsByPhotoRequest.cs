using MediatR;
using MiniProject.BusinessLogic.Models;
using System.Collections.Generic;

namespace MiniProject.BusinessLogic.Commands
{
    public class GetAllCommentsByPhotoRequest : IRequest<IEnumerable<Comment>>
    {
        public GetAllCommentsByPhotoRequest(int photoId)
        {
            PhotoId = photoId;
        }

        public int PhotoId { get; }
    }
}
