using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace CarPoolIndia.ViewModels;

public partial class SupportViewModel : BaseViewModel
{
    public SupportViewModel()
    {
        Title = "How It Works & Support";
    }

    [RelayCommand]
    public async Task OpenWhatsAppAsync()
    {
        string whatsappUrl = "https://wa.me/919876543210?text=Hello%20CarPool%20India%20Admin,%20I%20need%20assistance%20with%20an%20active%20car%20group%20buying%20pool.";
        try
        {
            await Launcher.OpenAsync(new Uri(whatsappUrl));
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlertAsync("WhatsApp Concierge", "Reach out to CarPool India Admin Desk directly at +91 98765 43210 on WhatsApp.", "OK");
        }
    }

    [RelayCommand]
    public async Task CallConciergeAsync()
    {
        try
        {
            if (PhoneDialer.Default.IsSupported)
            {
                PhoneDialer.Default.Open("+919876543210");
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Helpline", "CarPool India Admin Desk: +91 98765 43210 (Mon-Sat, 9 AM - 8 PM IST)", "OK");
            }
        }
        catch
        {
            await Shell.Current.DisplayAlertAsync("Helpline", "CarPool India Admin Desk: +91 98765 43210", "OK");
        }
    }
}
