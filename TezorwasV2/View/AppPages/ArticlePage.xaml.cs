using TezorwasV2.ViewModel.MainPages;
using static TezorwasV2.View.AppPages.ArticlesView;

namespace TezorwasV2.View.AppPages
{
    public partial class ArticlePage : ContentPage, IQueryAttributable
    {
        ArticlePageViewModel viewModel;

        public ArticlePage(ArticlePageViewModel articlePageViewModel)
        {
            InitializeComponent();

            viewModel = articlePageViewModel;
            BindingContext = viewModel;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("ArticleToShow"))
            {
                var article = query["ArticleToShow"] as ArticleDto;
                viewModel.ArticleToShow = article;
                if (viewModel.ArticleTitle == null)
                    viewModel.PopulateArticle();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            itemsCollectionView.ItemsSource = viewModel.paragraphs;

        }
    }
}
