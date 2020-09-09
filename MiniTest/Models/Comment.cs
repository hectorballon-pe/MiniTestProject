using MiniProject.Infrastructure;

namespace MiniProject.BusinessLogic.Models
{
    public class Comment : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PostId { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }
}
