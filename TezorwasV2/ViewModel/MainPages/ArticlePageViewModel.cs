
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using static TezorwasV2.View.AppPages.ArticlesView;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class ArticlePageViewModel : ObservableObject
    {
        public ArticleDto ArticleToShow { get; set; }

        public ObservableCollection<ParagraphDto> paragraphs = new ObservableCollection<ParagraphDto>();
        [ObservableProperty] public string articleTitle;
        [ObservableProperty] public UriImageSource coverUri;
        //[ObservableProperty] public ImageSource coverUri;
        [ObservableProperty] public DateTime publishDate;


        private void PopulateParagraphs()
        {
            paragraphs.Clear();
            var phrases = ArticleToShow.Content.Split('.');
            foreach (var phrase in phrases)
            {
                paragraphs.Add(new ParagraphDto { Content = phrase });
            }
        }
        public void PopulateArticle()
        {
            if (ArticleToShow is not null)
            {
                ArticleTitle = ArticleToShow.Title;
                CoverUri = ArticleToShow.UriCover;
                PublishDate = ArticleToShow.DatePublished;
                PopulateParagraphs();
            }
        }
    }
    public class ParagraphDto
    {
        public string Content { get; set; }
    }
}
