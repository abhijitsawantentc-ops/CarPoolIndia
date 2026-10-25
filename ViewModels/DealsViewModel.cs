using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CarPoolIndia.Models;
using CarPoolIndia.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace CarPoolIndia.ViewModels;

public partial class DealsViewModel : BaseViewModel
{
    private readonly ICatalogService _catalogService;
    private List<PoolCampaign> _allPoolsCache = [];

    [ObservableProperty]
    private string _selectedCity = "All Cities";

    [ObservableProperty]
    private string _activeBannerNote = "🔥 Active Pools: Commit ₹5,000 refundable token to lock bulk pricing!";

    public ObservableCollection<PoolCampaign> DisplayedPools { get; } = [];

    public DealsViewModel(ICatalogService catalogService)
    {
        _catalogService = catalogService;
        Title = "Curated Deals";
    }

    [RelayCommand]
    public async Task LoadPoolsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            _allPoolsCache = await _catalogService.GetActivePoolsAsync();
            ApplyFilter();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to load curated pools: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public void FilterByCity(string city)
    {
        SelectedCity = city;
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        DisplayedPools.Clear();
        var filtered = SelectedCity == "All Cities"
            ? _allPoolsCache
            : _allPoolsCache.Where(p => p.TargetCity.Contains(SelectedCity, StringComparison.OrdinalIgnoreCase));

        foreach (var pool in filtered)
        {
            DisplayedPools.Add(pool);
        }
    }

    [RelayCommand]
    public async Task JoinPoolAsync(PoolCampaign? pool)
    {
        if (pool == null) return;

        string variant = pool.CarModel.AvailableVariants.FirstOrDefault()?.VariantName ?? "Standard Variant";

        bool proceed = await Shell.Current.DisplayAlertAsync(
            $"Join {pool.CarModel.Brand} {pool.CarModel.ModelName} Pool",
            $"Commitment Deposit: ₹{pool.TokenAmountINR:N0} (100% Refundable)\n\n" +
            $"Target City: {pool.TargetCity}\n" +
            $"Variant: {variant}\n" +
            $"Expected Savings: {pool.FormattedSavings}\n" +
            $"Offer Perk: {pool.OfferPerks}\n\n" +
            "Proceed with UPI token payment to join this group buying pool?",
            "Pay ₹5,000 via UPI",
            "Cancel"
        );

        if (proceed)
        {
            IsBusy = true;
            try
            {
                var booking = await _catalogService.CommitTokenBookingAsync(
                    pool.Id,
                    variant,
                    "Pearl Arctic White",
                    "Rohan Deshmukh",
                    "+91 98200 54321",
                    pool.TargetCity
                );

                await Shell.Current.DisplayAlertAsync(
                    "Token Committed Successfully! 🎉",
                    $"Booking ID: {booking.BookingId}\n\n" +
                    $"Your spot in the {pool.CarModel.FullDisplayName} group pool has been secured.\n\n" +
                    $"• Quorum: {pool.CurrentBuyers}/{pool.MinBuyers} committed.\n" +
                    $"• Token Paid: ₹5,000 (UPI Ref: {booking.UpiReferenceId})\n" +
                    "• Next Steps: Our offline admin team will coordinate allocations with authorized brand dealers once quorum completes.",
                    "View My Bookings"
                );

                // Switch to Bookings tab in Shell
                await Shell.Current.GoToAsync("//MyBookings");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Commitment Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                await LoadPoolsAsync();
            }
        }
    }
}
