using Dream_Journal_Project.Models;
using System.Diagnostics;

namespace Dream_Journal_Project.Pages;

[QueryProperty(nameof(DreamId), "dreamId")]

[QueryProperty(nameof(Dream), "Dream")]

public partial class EditDreamPage : ContentPage
{
	public int DreamId { get; set; }

	public Dream Dream { get; set; }
    public EditDreamPage()
	{
		InitializeComponent();

		
		if(Dream != null)
		{
            Debug.WriteLine(DreamId + ": dream id");
            Debug.WriteLine(Dream.Title + ": dream title from dream");
        }
		
	}
}