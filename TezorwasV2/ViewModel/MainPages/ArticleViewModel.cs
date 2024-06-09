
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TezorwasV2.Helpers;
using TezorwasV2.Model;
using TezorwasV2.Services;
using TezorwasV2.View.AppPages;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class ArticleViewModel : ObservableObject
    {
        private readonly IGlobalContext _globalContext;
        //private readonly IPersonService _personService;
        private readonly IArticleService _articleService;

        [ObservableProperty] private string? title;
        [ObservableProperty] private string? content;
        [ObservableProperty] private DateTime datePublished;
        [ObservableProperty] private string coverUri;

        public ObservableCollection<ArticleModel> AllArticles { get; set; } = new ObservableCollection<ArticleModel>();

        public ArticleViewModel(IGlobalContext globalContext, IArticleService articleService /*IPersonService personService*/)
        {
            _globalContext = globalContext;
            _articleService = articleService;
        }

        public async Task LoadAllArticles()
        {
            AllArticles = new ObservableCollection<ArticleModel>();
            var articles = await _articleService.GetAllArticles(_globalContext.UserToken);
            foreach (ArticleModel article in articles)
            {
                AllArticles.Add(article);
            }
        }
        [RelayCommand]
        public async Task GoToArticle(dynamic articleToTransfer)
        {
            var navigationParameters = new Dictionary<string, dynamic>
            {
                { "ArticleToShow",articleToTransfer},
            };
            await Shell.Current.GoToAsync(nameof(ArticlePage), true, navigationParameters);
        }
    }
}
