using CommunityToolkit.Maui.Core;
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

        var taglist = await _dataBaseService.GetTags();

        ExpanderList.ItemsSource = taglist;


    }


    // for now it will stay commented until i make it work with tags
    public async Task GenerateChart2()
    {
        //// get dreams from database to collection
        //var dreamsFromDb = await _dataBaseService.GetDreams();
        //Debug.WriteLine($"sny:  {dreamsFromDb.Count}");
        //Dreams.Clear();
        //foreach (var dream in dreamsFromDb)
        //{
        //    Dreams.Add(dream);
        //}

        //// get number of specific dreams to ints
        ////int lucidCount = Dreams.Count(x => x.LucidDream);
        ////int nonlucidCount = Dreams.Count(x => !x.LucidDream);
        ////int dreamsTotal = Dreams.Count();

        //// create entries for chart (Lucid and non lucid dreams)
        //var entries = new[]
        //{
        //    new ChartEntry(lucidCount)
        //    {
        //        Color = SKColors.Red
        //    },

        //    new ChartEntry(nonlucidCount)
        //    {
        //        Color = SKColors.Green
                
        //    }

        //};

        //MyChart = new DonutChart
        //{
        //    Entries = entries,
        //    BackgroundColor = SKColor.Empty,
        //    LabelTextSize = 15,
        //    Typeface = SKTypeface.FromFamilyName("Arial"),
        //    LabelColor = SKColors.White
        //};

        //DreamChart.Chart = MyChart;

        //ChartLucidLabel.Text = "Lucid Count: " + lucidCount.ToString();
        //ChartNonLucidLabel.Text = "Non-Lucid Count:  " + nonlucidCount.ToString();
    }

    
    private async void ExpanderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var tag = e.CurrentSelection.FirstOrDefault() as Tag;

        if (tag != null)
        {
            await RemoveChart();
            await GenerateChart(tag);
            
        }
        
    }


    private async Task RemoveChart()
    {
        DreamChart.Chart = null;
    }
    private async Task GenerateChart(Tag tag)
    {
        List<Dream> AllDreams = await _dataBaseService.GetDreams();

        var dreamswithTag = AllDreams.Where(d => d.TagIds != null && d.TagIds.Split(',').Contains(tag.Id.ToString())).ToList();

        float AllDreamsCount = AllDreams.Count;

        float TagDreamsCount = dreamswithTag.Count;

        float OtherDreamsCount = AllDreamsCount - TagDreamsCount;


        // create entries for chart
        var entries = new[]
        {
            new ChartEntry(OtherDreamsCount)
            {
                Color = SKColors.LimeGreen
            },
            new ChartEntry(dreamswithTag.Count)
            {
                Color = SKColor.Parse(tag.ColorHex)
            }
        };


        // create chart with entries
        DonutChart Mychart = new DonutChart();
        {
            Mychart.Entries = entries;
            Mychart.HoleRadius = 0.5f;
            Mychart.BackgroundColor = SKColors.Transparent;

        }

        
        DreamChart.Chart = Mychart;

        Box_1.Color = Colors.LimeGreen;

        Box_1_Label.Text = "Other Dreams: " + OtherDreamsCount;

        Box_2.Color = Color.Parse(tag.ColorHex);

        Box_2_Label.Text = tag.Name + ": " + dreamswithTag.Count + "  | " + Math.Round((TagDreamsCount / AllDreamsCount) * 100) + "%";

    }

}