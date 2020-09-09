using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniProject.BusinessLogic.Commands;
using MiniProject.BusinessLogic.Models;
using MiniProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MiniProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        public DataController(IMediator mediator)
        {
            Mediator = mediator;
        }

        private IMediator Mediator { get; }

        [HttpGet("albums/{user}")]
        [ProducesResponseType(typeof(IEnumerable<Album>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAlbumsByUserAsync([FromRoute] string user)
        {
            if (string.IsNullOrEmpty(user)) return BadRequest("User Id value is not valid");
            if (!int.TryParse(user, out var userId)) return BadRequest("User Id value is not integer");

            var albums = await Mediator.Send(new GetAllAlbumsByUserRequest(userId));
            if (albums != null && albums.Any()) return Ok(albums);
            return NotFound();
        }

        [HttpGet("photos/{album}")]
        [ProducesResponseType(typeof(IEnumerable<Photo>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPhotosByAlbumAsync([FromRoute] string album)
        {
            if (string.IsNullOrEmpty(album)) return BadRequest("Album Id value is not valid");
            if (!int.TryParse(album, out var albumId)) return BadRequest("User Id value is not integer");

            var photos = await Mediator.Send(new GetAllPhotosByAlbumRequest(albumId));
            if (photos != null && photos.Any()) return Ok(photos);
            return NotFound();
        }

        [HttpGet("comments/{photo}")]
        [ProducesResponseType(typeof(IEnumerable<Comment>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCommentsByPhotoAsync([FromRoute] string photo)
        {
            if (string.IsNullOrEmpty(photo)) return BadRequest("Photo Id value is not valid");
            if (!int.TryParse(photo, out var photoId)) return BadRequest("Photo Id value is not integer");

            var comments = await Mediator.Send(new GetAllCommentsByPhotoRequest(photoId));
            if (comments != null && comments.Any()) return Ok(comments);
            return NotFound();
        }
    }
}
