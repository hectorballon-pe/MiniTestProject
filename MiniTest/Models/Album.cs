using MiniProject.Infrastructure;

namespace MiniProject.Models
{
    public class Album : IModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
    }
}
