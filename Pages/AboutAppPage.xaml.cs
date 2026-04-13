namespace Dream_Journal_Project.Pages;

public partial class AboutAppPage : ContentPage
{
	public AboutAppPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();


        var currentTheme = Preferences.Default.Get("AppTheme", AppTheme.Unspecified.ToString());

        if (Enum.TryParse(currentTheme, out AppTheme theme))
        {
            Application.Current.UserAppTheme = theme;
        }


        if (theme == AppTheme.Light)
        {
            GithubImage.Source = "github_light.png";
            GithubBorder.BackgroundColor = Colors.Black;
        }
        else if(theme == AppTheme.Dark)
        {
            GithubImage.Source = "github_betterdark.png";
            GithubImage.CornerRadius = 50;
            GithubBorder.BackgroundColor = Colors.White;
        }
    }
}