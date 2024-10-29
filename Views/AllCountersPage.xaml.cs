using CounterApp.Models;
using System.Diagnostics;

namespace CounterApp.Views;

public partial class AllCountersPage : ContentPage
{
    private AllCounters allCounters;
    public AllCountersPage()
    {
        InitializeComponent();

        allCounters = new Models.AllCounters();
        BindingContext = allCounters;
    }

    protected override void OnAppearing()
    {
        allCounters.LoadCounters();
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CounterPage));
    }

    private void IncrementButton_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var counter = (Counter)button.BindingContext;
        counter.Value++;
        UpdateCounter(counter);
    }

    private void DecrementButton_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var counter = (Counter)button.BindingContext;
        counter.Value--;
        UpdateCounter(counter);
    }


    private void UpdateCounter(Counter counter)
    {
        counter.Save();

        var index = allCounters.Counters.IndexOf(counter);
        if (index != -1)
        {
            allCounters.Counters[index] = counter;
        }
        allCounters.LoadCounters();
    }

    private void ResetButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        var counter = (Counter)imageButton.BindingContext;

        counter.Value = counter.DefaultValue;
        UpdateCounter(counter);
    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        var counter = (Counter)imageButton.BindingContext;

        if (File.Exists(counter.Filename))
        {
            File.Delete(counter.Filename);
        }

        allCounters.Counters.Remove(counter);

        allCounters.LoadCounters();
    }

    private async void EditButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        var counter = (Counter)imageButton.BindingContext;

        await Shell.Current.GoToAsync($"{nameof(CounterPage)}?{nameof(CounterPage.ItemId)}={counter.Filename}");

    }
}