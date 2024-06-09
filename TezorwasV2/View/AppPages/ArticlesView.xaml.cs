using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class ArticlesView : ContentPage
{
    ArticleViewModel viewModel;
    List<ArticleDto> allArticles {get;set;} = new List<ArticleDto>();
    public ArticlesView(ArticleViewModel articleViewModel)
    {
        InitializeComponent();

        viewModel = articleViewModel;
        this.BindingContext = viewModel;

        #region test
        //var art1Url = "https://firebasestorage.googleapis.com/v0/b/tezorwas.appspot.com/o/art1.jpg?alt=media&token=7f4c1d15-fd48-4538-a219-29779d1f447e";
        //var art2Url = "https://firebasestorage.googleapis.com/v0/b/tezorwas.appspot.com/o/art2.jpg?alt=media&token=494c4525-f289-4767-b486-73a74ed2beb9";

        //var uriImageSource = new UriImageSource
        //{
        //    Uri = new Uri(art1Url),
        //    CachingEnabled = false, // Setează la true dacă dorești caching
        //    CacheValidity = TimeSpan.FromDays(1) // Valabilitate cache, dacă este activat
        //};

        //art1Imgg.Source = uriImageSource;


        //uriImageSource = new UriImageSource
        //{
        //    Uri = new Uri(art2Url),
        //    CachingEnabled = false, // Setează la true dacă dorești caching
        //    CacheValidity = TimeSpan.FromDays(1) // Valabilitate cache, dacă este activat
        //};

        //art2Imgg.Source = uriImageSource;
        #endregion

    }
    protected override async void OnAppearing()
    {
#pragma warning disable CA1416 // Validate platform compatibility
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Color.FromArgb("#eff1f3"),
            StatusBarStyle = StatusBarStyle.DarkContent
        });
#pragma warning restore CA1416 // Validate platform compatibility
        allArticles = new List<ArticleDto>();
        await LoadArticles();
        var x = 3;
    }

    protected async Task LoadArticles()
    {
        await viewModel.LoadAllArticles();
        var articlesToParse = viewModel.AllArticles;
        foreach (var article in articlesToParse)
        {
            ArticleDto articleToAdd = new ArticleDto();
            articleToAdd.Title = article.Title;
            articleToAdd.Content = article.Content;
            articleToAdd.DatePublished = article.DatePublished;
            articleToAdd.UriCover = new UriImageSource
            {
                Uri = new Uri(article.CoverUrl),
                CachingEnabled = false,
                CacheValidity = TimeSpan.FromDays(1)
            };

            allArticles.Add(articleToAdd);
        }
        itemsCollectionView.ItemsSource = allArticles;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ArticlePage),true);
    }

    public class ArticleDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePublished { get; set; }
        public UriImageSource UriCover { get; set; }
    }
}