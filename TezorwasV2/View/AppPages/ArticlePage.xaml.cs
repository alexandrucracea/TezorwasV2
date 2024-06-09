using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class ArticlePage : ContentPage
{
	ArticlePageViewModel viewModel;
    public ArticlePage(ArticlePageViewModel articlePageViewModel)
	{
		InitializeComponent();
		viewModel = articlePageViewModel;
		this.BindingContext = this;
	}

}