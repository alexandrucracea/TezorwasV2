
namespace TezorwasV2.Model
{
    public class TaskModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public int XpEarned { get; set; }
    }
}
