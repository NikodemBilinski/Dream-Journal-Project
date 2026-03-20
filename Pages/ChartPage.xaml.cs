using Dream_Journal_Project.Models;
using Microcharts;
using SkiaSharp;

namespace Dream_Journal_Project.Pages;

public partial class ChartPage : ContentPage
{

	DataBaseService _dataBaseService;

    public Chart MyChart { get; set; }
	public ChartPage(DataBaseService databaseservice)
	{
		
		InitializeComponent();

        _dataBaseService = databaseservice;

        GenerateChart();
       
    }

    public async Task GenerateChart()
    {
        var entries = new[]
           {
                new ChartEntry(10)
                {
                    Label = "Lucid",
                    ValueLabelColor = SKColors.White,
                    ValueLabel = "10",
                    Color = SKColor.Parse("#FF0000")
                },
                new ChartEntry(20)
                {
                    Label = "Non-Lucid",
                    ValueLabelColor = SKColors.White,
                    ValueLabel = "20",
                    Color = SKColor.Parse("#00FF00")
                }
            };

        MyChart = new DonutChart
        {
            Entries = entries,
            BackgroundColor = SKColors.Empty
        };

        DreamChart.Chart = MyChart;
    }
}