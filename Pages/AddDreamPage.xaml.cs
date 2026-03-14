using Dream_Journal_Project.Models;

namespace Dream_Journal_Project;

public partial class AddDreamPage : ContentPage
{
	public AddDreamPage(DataBaseService databaseservice)
	{
		InitializeComponent();
	}
	private async void Save_Dream_Clicked(object sender, EventArgs e)
	{
		await Save_Dream();
	}
    private async Task Save_Dream()
    {
		// error handling
		if (string.IsNullOrWhiteSpace(Dream_Title.Text))
		{
			await DisplayAlertAsync("Error", "Please enter a title for your dream.", "OK");
			return;
        }
		if (string.IsNullOrWhiteSpace(Dream_Description.Text))
		{
			await DisplayAlertAsync("Error", "Please enter a description for your dream.", "OK");
			return;
        }


        var newdream = new Dream
		{
			Title = Dream_Title.Text,
			Description = Dream_Description.Text,
			DateCreated = DateTime.Now
		};

        var parameters = new Dictionary<string, object>
		{
			{"NewDream", newdream }
		};

		// go back to mainpage
		await Shell.Current.GoToAsync("..", parameters);
    }
}