using Dream_Journal_Project.Models;
using System.Diagnostics;

namespace Dream_Journal_Project;

public partial class AddDreamPage : ContentPage
{
	private readonly DataBaseService _databaseService;

	private List<Tag> MySelectedTags = new List<Tag>();
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

		var selectedItems = MySelectedTags;

		string TagsIdString = string.Join(",", selectedItems.Select(x => x.Id));



		var newdream = new Dream
		{
			Title = Dream_Title.Text,
			Description = Dream_Description.Text,
			DateCreated = DateTime.Now,
			TagIds = TagsIdString
			
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

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		var border = (Border)sender;
        var border2 = (Border)border.Parent;

        var tag = (Tag)border.BindingContext;

        if (MySelectedTags.Contains(tag))
        {
            MySelectedTags.Remove(tag);
            border2.StrokeThickness = 0;
            border2.Stroke = Colors.Transparent;
        }
        else
        {
            MySelectedTags.Add(tag);
            border2.StrokeThickness = 5;
            border2.Stroke = Colors.GhostWhite;
        }
    }
}