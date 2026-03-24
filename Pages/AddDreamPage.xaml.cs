using Dream_Journal_Project.Models;
using System.Diagnostics;

namespace Dream_Journal_Project;

public partial class AddDreamPage : ContentPage
{
	private readonly DataBaseService _databaseService;
    public AddDreamPage(DataBaseService databaseservice)
	{
		InitializeComponent();
		_databaseService = databaseservice;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var tags = await _databaseService.GetTags();

		TagsSelection.ItemsSource = tags;
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

		var selectedItems = TagsSelection.SelectedItems.Cast<Tag>().ToList();

		string tagsString = string.Join(", ", selectedItems.Select(x => x.Name));


		var newdream = new Dream
		{
			Title = Dream_Title.Text,
			Description = Dream_Description.Text,
			DateCreated = DateTime.Now,
			LucidDream = LucidDreamBox.IsChecked,
			TagIds = tagsString
			
		};

		//debug hehe
		Debug.WriteLine(newdream);

        var parameters = new Dictionary<string, object>
		{
			{"NewDream", newdream }
		};

		// go back to mainpage
		await Shell.Current.GoToAsync("..", parameters);
    }
}