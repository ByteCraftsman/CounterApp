namespace CounterApp.Views;

using CounterApp.Models;
using Syncfusion.Maui.Gauges;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

[QueryProperty(nameof(ItemId), nameof(ItemId))]

public partial class CounterPage : ContentPage
{
    public string ItemId
    {
        set { LoadCounter(value); }
    }

    public CounterPage()
    {
        InitializeComponent();
        String appDataPath = FileSystem.AppDataDirectory;
        String randomFileName = $"{Path.GetRandomFileName()}.json";

        LoadCounter(Path.Combine(appDataPath, randomFileName));

        defaultValuePointer.ValueChanged += (sender, e) => UpdateDefaultValueLabel();
    }

    private void LoadCounter(string fileName)
    {
        Models.Counter counterModel;

        if (File.Exists(fileName))
        {
            counterModel = Models.Counter.FromFile(fileName);
        }
        else
        {
            counterModel = new Models.Counter
            {
                Filename = fileName,
                Name = "",
                DefaultValue = 0,
                ColorHex = Colors.Blue.ToHex(),
                Date = DateTime.Now
            };
        }

        BindingContext = counterModel;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.Counter counter)
        {
            int numOfCounters = GetNumOfCounters();
            string defaultName = GenerateDefaultName();

            if (string.IsNullOrWhiteSpace(TextEntry.Text))
            {
                counter.Name = $"{defaultName}";
            }
            else if (!IsNameUnique(TextEntry.Text) && !File.Exists(counter.Filename))
            {
                await DisplayAlert("B³¹d", "Nazwa licznika musi byæ unikalna.", "OK");
                return;
            }
            else
            {
                counter.Name = TextEntry.Text;
            }


            counter.DefaultValue = (int)defaultValuePointer.Value;
            counter.Value = counter.DefaultValue;
            counter.Date = DateTime.Now;

            counter.Save();
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.Counter counter)
            if (File.Exists(counter.Filename))
                File.Delete(counter.Filename);
        await Shell.Current.GoToAsync("..");
    }

    private Button previousButton;
    private void ColorButton_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;

        if (previousButton != null)
        {
            previousButton.BorderColor = Colors.Transparent;
            previousButton.BorderWidth = 0;
        }

        button.BorderColor = Colors.White;
        button.BorderWidth = 4;

        previousButton = button;

        var selectedColor = button.BackgroundColor.ToHex();

        if (BindingContext is Models.Counter counter)
        {
            counter.ColorHex = selectedColor;
            Debug.WriteLine(counter.ColorHex);
        }
    }

    private bool IsNameUnique(string name)
    {
        var allCounters = new Models.AllCounters();
        allCounters.LoadCounters();
        return !allCounters.Counters.Any(counter => counter.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private int GetNumOfCounters()
    {
        string appDataPath = FileSystem.AppDataDirectory;

        var counterFiles = Directory.GetFiles(appDataPath, "*.json");
        return counterFiles.Length;
    }

    private void UpdateDefaultValueLabel()
    {
        double value = defaultValuePointer.Value; 
        double flooredValue = Math.Floor(value);
        defaultValueLabel.Text = flooredValue.ToString("F0");
    }

    private string GenerateDefaultName()
    {
        int numOfCounters = GetNumOfCounters();
        return $"Licznik {numOfCounters + 1}";
    }
}