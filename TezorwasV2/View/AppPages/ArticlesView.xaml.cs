using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Syncfusion.Maui.ListView;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class ArticlesView : ContentPage
{
    private readonly ArticleViewModel _viewModel;
    List<ArticleDto> allArticles { get; set; } = new List<ArticleDto>();
    public ArticlesView(ArticleViewModel articleViewModel)
    {
        InitializeComponent();

        _viewModel = articleViewModel;
        BindingContext = _viewModel;

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

        var popup = new LoadingSpinnerPopup();
        this.ShowPopup(popup);

        await LoadArticles();

        popup.Close();

    }
    protected async Task LoadArticles()
    {

        await _viewModel.LoadAllArticles();

        var articlesToParse = _viewModel.AllArticles;


        foreach (var article in articlesToParse)
        {
            ArticleDto articleToAdd = new ArticleDto();
            articleToAdd.Title = article.Title;
            articleToAdd.Content = article.Content;
            articleToAdd.DatePublished = article.DatePublished;
            articleToAdd.UriCover = new UriImageSource
            {
                Uri = new Uri(article.CoverUrl),
                CachingEnabled = true,
                CacheValidity = TimeSpan.FromDays(1)
            };

            allArticles.Add(articleToAdd);
        }


        var allArticlesOld = allArticles;
        bool areDifferent = AreListsDifferent(allArticlesOld, allArticles);

        if (areDifferent || itemsCollectionView.ItemsSource == null)
        {
            itemsCollectionView.ItemsSource = allArticles;
        }
    }




    private void itemsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentArticle = e.CurrentSelection.FirstOrDefault() as ArticleDto;
        if (currentArticle is not null)
        {
            _viewModel.GoToArticleCommand.Execute(currentArticle);
        }

        //this.ShowBottomSheet(GetMyBottomSheetContent(), true);
    }

    public static bool AreListsDifferent<T>(List<T> list1, List<T> list2)
    {
        var diff1 = list1.Except(list2).ToList();
        var diff2 = list2.Except(list1).ToList();

        return diff1.Any() || diff2.Any();
    }

    public class ArticleDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePublished { get; set; }
        public UriImageSource UriCover { get; set; }
        //public ImageSource UriCover { get; set; }

    }


}