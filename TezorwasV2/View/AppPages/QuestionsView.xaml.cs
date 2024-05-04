using TezorwasV2.Helpers;
using TezorwasV2.ViewModel.MainPages;

namespace TezorwasV2.View.AppPages;

public partial class QuestionsView : ContentPage
{
    public string Name { get; set; } = string.Empty;

    public IGlobalContext? GlobalContext { get; private set; }
    //public string PersonName { get; set; } = string.Empty;

    private readonly QuestionsViewModel _questionsViewModel;


    public QuestionsView(QuestionsViewModel questionsViewModel)
    {
        InitializeComponent();
        //PopulateAgeValues();
        SetVisibilityToSectionOne(true);

        _questionsViewModel = questionsViewModel;
        BindingContext = _questionsViewModel;

    }

    private void ContinueQuestionsSectionOne_OnClicked(object? sender, EventArgs e)
    {
        SetVisibilityToSectionOne(false);
        SetVisibilityToSectionTwo(true);
        //todo de vazut ce mesaj punem in acest caz si daca tratam asta aici sau in vm


    }

    private void SetVisibilityToSectionOne(bool visibility)
    {
        AgeEntry.IsVisible = visibility;
        StreetEntry.IsVisible = visibility;
        CityEntry.IsVisible = visibility;
        CountyEntry.IsVisible = visibility;
        ContinueQuestionsSectionOne.IsVisible = visibility;

        SetVisibilityToSectionTwo(!visibility);

    }

    private void SetVisibilityToSectionTwo(bool visibility)
    {
        LblHabbitTitle.IsVisible = visibility;
        EdtHabbitOneDescription.IsVisible = visibility;
        EdtHabbitTwoDescription.IsVisible = visibility;
        EdtHabbitThreeDescription.IsVisible = visibility;
        wasteRating.IsVisible = visibility;
        ContinueQuestionsSectionTwo.IsVisible = visibility;
    }

    private void ContinueQuestionsSectionTwo_OnClicked(object? sender, EventArgs e)
    {
        //todo trebuie valiare facuta pe view model ca cel putin un habbit sa fie completat
        //Dictionary<string, dynamic> habbits =
        //{

        //}
        //todo trebuie preluat ratingul si adaugat la fiecare habbit
        //todo trebuie facuta redirectionare catre ecranul principal
        //todo de vazut daca au sens dto urile in loc de servicii in aplicatie (momentan sunt la fel si nu pare sa aiba sens distinctia)
    }
}