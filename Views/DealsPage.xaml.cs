using CarPoolIndia.ViewModels;
using Microsoft.Maui.Controls;

namespace CarPoolIndia.Views;

public partial class DealsPage : ContentPage
{
    private readonly DealsViewModel _viewModel;

    public DealsPage(DealsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadPoolsCommand.ExecuteAsync(null);
    }
}
