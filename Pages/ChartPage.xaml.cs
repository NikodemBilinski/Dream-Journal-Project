using Dream_Journal_Project.Models;
using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Dream_Journal_Project.Pages;

public partial class ChartPage : ContentPage
{

    private ObservableCollection<Dream> Dreams { get; set; } = new();

    DataBaseService _dataBaseService;

    public Chart MyChart { get; set; }
	public ChartPage(DataBaseService databaseservice)
	{
		
		InitializeComponent();

        _dataBaseService = databaseservice;

        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await GenerateChart2();

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error generating chart: "+ ex.Message);
        }
    }

    public async Task GenerateChart2()
    {
        // get dreams from database to collection
        var dreamsFromDb = await _dataBaseService.GetDreams();
        Debug.WriteLine($"sny:  {dreamsFromDb.Count}");
        Dreams.Clear();
        foreach (var dream in dreamsFromDb)
        {
            Dreams.Add(dream);
        }

        // get number of specific dreams to ints
        int lucidCount = Dreams.Count(x => x.LucidDream);
        int nonlucidCount = Dreams.Count(x => !x.LucidDream);
        int dreamsTotal = Dreams.Count();

        // create entries for chart (Lucid and non lucid dreams)
        var entries = new[]
        {
            new ChartEntry(lucidCount)
            {
                Label = "Lucid",
                ValueLabelColor = SKColors.White,
                ValueLabel = lucidCount.ToString(),
                Color = SKColor.Parse("#FF0000")
            },

            new ChartEntry(nonlucidCount)
            {
                Label = "Non-Lucid",
                ValueLabelColor = SKColors.White,
                ValueLabel = nonlucidCount.ToString(),
                Color = SKColor.Parse("#00FF00")
            }

        };

        MyChart = new DonutChart
        {
            Entries = entries,
            BackgroundColor = SKColor.Empty,
            LabelTextSize = 15,
            Typeface = SKTypeface.FromFamilyName("Arial"),
            LabelColor = SKColors.White
        };

        DreamChart.Chart = MyChart;
    }



    //public async Task GenerateChart()
    //{
    //    var entries = new[]
    //       {
    //            new ChartEntry(10)
    //            {
    //                Label = "Lucid",
    //                ValueLabelColor = SKColors.White,
    //                ValueLabel = "10",
    //                Color = SKColor.Parse("#FF0000")
    //            },
    //            new ChartEntry(20)
    //            {
    //                Label = "Non-Lucid",
    //                ValueLabelColor = SKColors.White,
    //                ValueLabel = "20",
    //                Color = SKColor.Parse("#00FF00")
    //            }
    //        };

    //    MyChart = new DonutChart
    //    {
    //        Entries = entries,
    //        BackgroundColor = SKColors.Empty
    //    };

    //    DreamChart.Chart = MyChart;
    //}
}