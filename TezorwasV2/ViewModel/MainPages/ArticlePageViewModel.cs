
using CommunityToolkit.Mvvm.ComponentModel;

namespace TezorwasV2.ViewModel.MainPages
{
    public class ArticlePageViewModel : ObservableObject
    {
        public string Test{ get; private set; }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Test = query["YourKey"] as string;
            OnPropertyChanged("Test");
        }
    }
}
