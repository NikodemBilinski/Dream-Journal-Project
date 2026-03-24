using Dream_Journal_Project.Models;

namespace Dream_Journal_Project.Pages;

public partial class TagPage : ContentPage
{
	private readonly DataBaseService _databaseservice;
	public TagPage(DataBaseService databaseservice)
	{
		InitializeComponent();

		_databaseservice = databaseservice;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await Load_Tags();
    }

    public async Task Load_Tags()
	{
		var tags = await _databaseservice.GetTags();

		TagTemplate.ItemsSource = tags;
	}

	public async void Trash_Bin_Clicked(object sender, EventArgs e)
	{
		var element = (VisualElement)sender;

		var tagToDelete = (Tag)element.BindingContext;

		_databaseservice.DeleteTag(tagToDelete);

		Refresh_Tags();
    }

	public async Task Refresh_Tags()
	{
		await Load_Tags();
	}

    private async void Add_Tag(object sender, EventArgs e)
    {
		string name = await DisplayPromptAsync("New Tag", "Enter the name of the new tag:", "OK", "Cancel");

		if (string.IsNullOrEmpty(name))
		{
			return;
		}

		string colorName = await DisplayActionSheetAsync("Pick a color", "Cancel", null, "Purple", "Blue", "Lime","Green", "Yellow", 
			"Pink", "Orange", "Gold", "Maroon", "Royal", "Violet");

        string hex = colorName switch
        {
            "Purple" => "#800080",
            "Blue" => "#0000FF",
            "Lime" => "#00FF00",
            "Green" => "#008000",
            "Yellow" => "#FFFF00",
            "Pink" => "#FFC0CB",
            "Orange" => "#FFA500",
            "Gold" => "#FFD700",
            "Maroon" => "#800000",
            "Royal" => "#4169E1",
            "Violet" => "#EE82EE",
            _ => "#FFFFFF" // Default white color if anything happens
        };

		Tag newtag = new Tag
		{
			Name = name,
			ColorHex = hex,
			IsActive = false

		};

		await _databaseservice.AddTag(newtag);

		await Refresh_Tags();
    }
}