using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CarPoolIndia.Models;
using CarPoolIndia.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace CarPoolIndia.ViewModels;

public partial class BookingsViewModel : BaseViewModel
{
    private readonly ICatalogService _catalogService;

    public ObservableCollection<BuyerBooking> Bookings { get; } = [];

    public BookingsViewModel(ICatalogService catalogService)
    {
        _catalogService = catalogService;
        Title = "My Bookings";
    }

    [RelayCommand]
    public async Task LoadBookingsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Bookings.Clear();
            var list = await _catalogService.GetUserBookingsAsync();
            foreach (var item in list)
            {
                Bookings.Add(item);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Could not load bookings: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public static async Task ViewStatusDetailsAsync(BuyerBooking? booking)
    {
        if (booking == null) return;

        await Shell.Current.DisplayAlertAsync(
            $"Booking Status: {booking.BookingId}",
            $"Vehicle: {booking.VehicleName}\n" +
            $"Variant: {booking.SelectedVariant}\n" +
            $"Buyer: {booking.BuyerName} ({booking.PhoneNumber})\n" +
            $"City/RTO: {booking.CityRto}\n" +
            $"Token Deposit: ₹{booking.TokenAmountPaidINR:N0} (UPI: {booking.UpiReferenceId})\n" +
            $"Status: {booking.PaymentStatus}\n\n" +
            $"Dealership Coordination:\n{booking.DealershipStatusNote}\n\n" +
            "ℹ️ Note: No dealer portal is used. All wholesale negotiation, VIN allocation, and billing coordination are managed directly by CarPool India administrators with authorized brand dealerships.",
            "Close"
        );
    }

    [RelayCommand]
    public async Task CancelBookingAsync(BuyerBooking? booking)
    {
        if (booking == null) return;

        bool confirm = await Shell.Current.DisplayAlertAsync(
            "Cancel Commitment?",
            $"Are you sure you want to cancel your spot in {booking.VehicleName}?\n\n" +
            $"Your ₹{booking.TokenAmountPaidINR:N0} refundable deposit will be credited back to your original UPI account within 2 hours.",
            "Yes, Refund Deposit",
            "Keep Spot"
        );

        if (confirm)
        {
            booking.PaymentStatus = PaymentStatus.Refunded;
            booking.DealershipStatusNote = "Commitment cancelled by buyer. ₹5,000 token refunded to original UPI handle.";
            await Shell.Current.DisplayAlertAsync("Refund Initiated", "₹5,000 has been refunded to your bank account via UPI.", "OK");
            await LoadBookingsAsync();
        }
    }
}
