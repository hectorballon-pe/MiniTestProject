using MiniProject.Infrastructure;

namespace MiniProject.BusinessLogic.Models
{
    public class Photo : IModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AlbumId { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
