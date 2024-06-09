using TezorwasV2.View.AppPages;
using TezorwasV2.View.Authentication;

namespace TezorwasV2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            #region Routing
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
            Routing.RegisterRoute(nameof(QuestionsView), typeof(QuestionsView));
            Routing.RegisterRoute(nameof(AchievmentssView), typeof(AchievmentssView));
            Routing.RegisterRoute(nameof(HabbitsView),typeof(HabbitsView));
            Routing.RegisterRoute(nameof(ArticlesView),typeof(ArticlesView));
            Routing.RegisterRoute(nameof(ArticlePage),typeof(ArticlePage));
            #endregion
        }
    }
}
