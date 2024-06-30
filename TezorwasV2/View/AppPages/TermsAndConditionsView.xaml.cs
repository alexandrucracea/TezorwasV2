
namespace TezorwasV2.View.AppPages
{
    public partial class TermsAndConditionsView : ContentPage
    {
        public List<FormattedString> ConditionsList { get; set; }

        public TermsAndConditionsView()
        {
            InitializeComponent();

            ConditionsList = new List<FormattedString>
            {
                ConvertToFormattedString("1.Acceptance of Terms: By accessing and using our app, you agree to be bound by these terms. If you do not agree, please do not use our app."),
                ConvertToFormattedString("2.Use of the Application: Our app is designed to promote recycling and reduce harmful habits through AI-based tasks. The content and tasks generated are for educational purposes only."),
                ConvertToFormattedString("3.User Responsibilities: Users are responsible for ensuring that their actions, influenced by our app, comply with local recycling laws and regulations."),
                ConvertToFormattedString("4.Privacy Policy: We collect and use personal data as outlined in our Privacy Policy. By using the app, you consent to such data collection and use."),
                ConvertToFormattedString("5.Intellectual Property: All content, including AI-generated tasks, graphics, and designs, are the property of our app and are protected by intellectual property laws."),
                ConvertToFormattedString("6.Limitation of Liability: We are not liable for any damages or losses resulting from the use of our app. Use the app at your own risk."),
                ConvertToFormattedString("7.Changes to Terms: We reserve the right to modify these terms at any time. Continued use of the app signifies acceptance of the updated terms.")
            };

            BindingContext = this;
        }

        private FormattedString ConvertToFormattedString(string text)
        {
            var formattedString = new FormattedString();
            var parts = text.Split(new[] { ':' }, 2);

            if (parts.Length > 1)
            {
                formattedString.Spans.Add(new Span { Text = parts[0] + ":", FontAttributes = FontAttributes.Bold });
                formattedString.Spans.Add(new Span { Text = parts[1] });
            }
            else
            {
                formattedString.Spans.Add(new Span { Text = text });
            }

            return formattedString;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..", true);
        }
    }
}
