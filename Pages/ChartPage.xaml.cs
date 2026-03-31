using CommunityToolkit.Maui.Core;
using Dream_Journal_Project.Models;
using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Dream_Journal_Project.Pages;

public partial class ChartPage : ContentPage
{

    private List<Dream> Dreams { get; set; } = new();

    private int ChartId = 0;

    private Tag SelectedTag;

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

        var taglist = await _dataBaseService.GetTags();

        ExpanderList.ItemsSource = taglist;

        Dreams = await _dataBaseService.GetDreams();

        Box_1_Label.Text = "Total Dreams: " + Dreams.Count;


    }


    private async void ExpanderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedTag = e.CurrentSelection.FirstOrDefault() as Tag;



        if (SelectedTag != null)
        {

            await RemoveChart();
            try
            {
                await GenerateChart(SelectedTag);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generating chart: " + ex.Message);
            }



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


        // create default chart with entries

        switch (ChartId)
        {
            case 0:
                {
                    DonutChart Mychart = new DonutChart();
                    {
                        Mychart.Entries = entries;
                        Mychart.HoleRadius = 0.5f;
                        Mychart.BackgroundColor = SKColors.Transparent;

                    }

                    DreamChart.Chart = Mychart;

                    break;
                }

            case 1:
                {
                    PointChart Mychart = new PointChart();
                    {
                        Mychart.Entries = entries;
                        Mychart.BackgroundColor = SKColors.Transparent;

                    }

                    DreamChart.Chart = Mychart;

                    break;
                }
        }



        Box_1.Color = Colors.LimeGreen;

        Box_1_Label.Text = "Other Dreams: " + OtherDreamsCount + "  | "+ Math.Round((OtherDreamsCount / AllDreamsCount) * 100) + "%";

        Box_2.Color = Color.Parse(tag.ColorHex);

        Box_2_Label.Text = tag.Name + ": " + dreamswithTag.Count + "  | " + Math.Round((TagDreamsCount / AllDreamsCount) * 100) + "%";

    }

    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        ChartId = 0;
        if (SelectedTag != null)
        {
            await RemoveChart();
            await GenerateChart(SelectedTag);
        }
    }
    private async void ImageButton_Clicked_2(object sender, EventArgs e)
    {
        ChartId = 1;
        if (SelectedTag != null)
        {
            await RemoveChart();
            await GenerateChart(SelectedTag);
        }
    }


}