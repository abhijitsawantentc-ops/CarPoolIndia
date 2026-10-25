using CarPoolIndia.ViewModels;
using Microsoft.Maui.Controls;

namespace CarPoolIndia.Views;

public partial class BookingsPage : ContentPage
{
    private readonly BookingsViewModel _viewModel;

    public BookingsPage(BookingsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadBookingsCommand.ExecuteAsync(null);
    }
}
