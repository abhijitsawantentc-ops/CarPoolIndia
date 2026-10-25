using CarPoolIndia.ViewModels;
using Microsoft.Maui.Controls;

namespace CarPoolIndia.Views;

public partial class SupportPage : ContentPage
{
    public SupportPage(SupportViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
