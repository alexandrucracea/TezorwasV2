using CommunityToolkit.Mvvm.ComponentModel;
using Google.Cloud.Vision.V1;
using TezorwasV2.View.AppPages;


namespace TezorwasV2.ViewModel
{
    public partial class AddHabbitPopupViewModel : ObservableObject
    {
        [ObservableProperty]
        private HabbitsView.ReceiptDataDto dataObject;

        [ObservableProperty] public string description;
        [ObservableProperty] public double levelOfWaste;

        public void SetData(HabbitsView.ReceiptDataDto data)
        {
            DataObject = data;
            Description = data.HabbitDescription;
            LevelOfWaste = data.HabbitLevelOfWaste;
        }
    }
}
