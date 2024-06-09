
namespace TezorwasV2.Model
{
    public class ArticleModel
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CoverUrl { get; set;} = string.Empty;
        public DateTime DatePublished { get; set;}
        //TODO de creat comments model
        //TODO de creat imagesModel    
    }
}
