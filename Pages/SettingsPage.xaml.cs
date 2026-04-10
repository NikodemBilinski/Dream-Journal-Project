using CommunityToolkit.Maui.Storage;
using Dream_Journal_Project.Models;
using System.Diagnostics;

namespace Dream_Journal_Project.Pages;

public partial class SettingsPage : ContentPage
{
	private readonly DataBaseService _databaseService;

    public SettingsPage(DataBaseService databaseservice)
	{
		
		InitializeComponent();

        _databaseService = databaseservice;

        if(Preferences.Default.Get("FilterAnimation", true) == true)
        {
            FilterAnimationSwitch.IsToggled = true;
        }
        else
        {
            FilterAnimationSwitch.IsToggled = false;
        }
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

    private async void Change_Theme(object sender, EventArgs e)
    {

    }

    private async void Import_Database(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select Database File to import"
            });

            if (result == null)
            {
                return;
            }

            if (!result.FileName.EndsWith(".db3"))
            {
                await DisplayAlertAsync("Error", "Please select a valid .db3 file.", "OK");
                return;
            }

            await _databaseService.CloseConnection();

            MainPage.IsFirstLoad = true;

            var stream = await result.OpenReadAsync();

            var dbpath = File.Create(Constants.DatabasePath);

            await stream.CopyToAsync(dbpath);

            await DisplayAlertAsync("Success", "Database imported successfully.", "OK");

            await _databaseService.Init();

            Application.Current.MainPage = new AppShell();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", "Failed to import database: " + ex.Message, "OK");
        }
    }

    private async void Export_Database(object sender, EventArgs e)
    {

        try
        {
            await _databaseService.CloseConnection();

            string dbpath = Path.Combine(FileSystem.AppDataDirectory, "DreamJournal.db3");

            using var stream = File.OpenRead(dbpath);

            var fileSaveResult = await FileSaver.Default.SaveAsync("DreamJournal_Backup.db3", stream);

            if (fileSaveResult.IsSuccessful)
            {
                await DisplayAlertAsync("Success", "Database exported successfully.", "OK");

            }
            else
            {
                await DisplayAlertAsync("Error", "Failed to export database: " + fileSaveResult.Exception.Message, "OK");
            }

            await _databaseService.Init();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        
    }
    
    private async void Filter_Animations(object sender, EventArgs e)
    {
        Debug.WriteLine("Filter Animations Toggled: " + FilterAnimationSwitch.IsToggled);
        Preferences.Default.Set("FilterAnimation", FilterAnimationSwitch.IsToggled);
    }

    
}