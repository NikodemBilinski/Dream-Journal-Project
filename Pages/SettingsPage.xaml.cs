using Dream_Journal_Project.Models;

namespace Dream_Journal_Project.Pages;

public partial class SettingsPage : ContentPage
{
	private readonly DataBaseService _databaseService;
    public SettingsPage(DataBaseService databaseservice)
	{
		
		InitializeComponent();

        _databaseService = databaseservice;
    }

	private async void Delete_All_Tags_Clicked(object sender, EventArgs e)
	{
		var answer = await DisplayAlertAsync("Are you sure?", "This action will delete all tags and remove them from all dreams. This cannot be undone.", "Yes", "No");
		if (answer)
		{
			await _databaseService.DeleteAllTags();
			await DisplayAlertAsync("Success", "All tags have been deleted.", "OK");
		}
    }

    private async void Delete_All_Dreams_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlertAsync("Are you sure?", "This action will delete all dreams. This cannot be undone.", "Yes", "No");
        if (answer)
        {
            await _databaseService.DeleteAllDreams();
            await DisplayAlertAsync("Success", "All dreams have been deleted.", "OK");
        }
    }
}