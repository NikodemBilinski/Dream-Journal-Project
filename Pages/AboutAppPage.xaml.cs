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
            GithubImage.Source = "github_betterdark.png";
            GithubImage.CornerRadius = 15;
            GithubImage.BackgroundColor = Colors.White;
            GithubBorder.BackgroundColor = Colors.White;
        }
        else if(theme == AppTheme.Dark)
        {
            GithubImage.Source = "github_betterdark.png";
            GithubBorder.BackgroundColor = Colors.White;
        }
    }

    private async void Github_Clicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://github.com/NikodemBilinski");
    }

    private async void Pointer_Entered_Testing(object sender, EventArgs e)
    {
        GithubBorder.Scale = 1;

        await GithubBorder.ScaleToAsync(1.2, 200, Easing.CubicOut);
    }
    private async void Pointer_Exited_Testing(object sender, EventArgs e)
    {
        await GithubBorder.ScaleToAsync(1, 200, Easing.CubicIn);
    }
}