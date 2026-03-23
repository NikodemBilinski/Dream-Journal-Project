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
}