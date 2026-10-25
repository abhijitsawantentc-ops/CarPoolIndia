# CarPool India 🚗🇮🇳
### Android Group-Buying Platform for High-Volume Indian Vehicles

CarPool India aggregates individual car buyers in India into regional bulk-buying pools for top-selling SUVs (**Hyundai Creta, Tata Nexon, Maruti Suzuki Brezza, Mahindra Scorpio-N**). By pooling 15–30 buyers together, members unlock fleet-level wholesale discounts, complimentary zero-dep insurance, and priority allocation.

---

## 🏛️ Architectural Context & Operating Model

* **No Dealer-Facing Portal**: Dealerships in India operate under strict OEM franchise agreements, territory boundaries, and private monthly sales quotas. Exposing a public dealer portal creates channel conflict and dealer reluctance. Instead, **all dealer coordination, wholesale fleet pricing negotiation, VIN tagging, and regional dealer allocation are handled manually by the CarPool India admin desk offline**.
* **Buyer Flow**:
  1. **Browse**: Buyers explore live regional pools tailored to their local RTO (Bengaluru, Delhi NCR, Pune & Mumbai, Hyderabad).
  2. **Commit Token**: Buyers lock their spot with a **₹5,000 refundable token deposit via UPI**.
  3. **Quorum Lock**: Once minimum buyer count is reached (e.g. 14/20 Creta buyers), the pool status transitions to `Locked` / `Allocating`.
  4. **Dealership Invoicing & Delivery**: The admin coordinates allocations with authorized regional dealerships (e.g., Trident Hyundai, Tata Motors, Maruti Arena/Nexa). Buyers pay the remaining balance directly to the authorized dealer and take delivery with full OEM factory warranty and local RTO registration.

---

## 📱 Tech Stack

* **Framework**: .NET MAUI targeting `net10.0-android`
* **Architecture**: MVVM (Model-View-ViewModel) with Dependency Injection
* **MVVM Toolkit**: `CommunityToolkit.Mvvm 8.4` (`ObservableObject`, `[ObservableProperty]`, `[RelayCommand]`)
* **REST API Client**: `Refit 8.0` + `Refit.HttpClientFactory` (`ICatalogApi`)
* **Bottom Navigation**: `AppShell.xaml` with 3 tabs and custom SVG icons

---

## 📁 Project Structure

```
CarPoolIndia/
├── Models/
│   ├── CarModel.cs          # Brand, ModelName, Segment, ImageUrl, StartingPriceINR, AvailableVariants
│   ├── PoolCampaign.cs      # Id, CarModel, TargetCity, MinBuyers, CurrentBuyers, TargetDiscountPercent,
│   │                        # BaseExShowroomPrice, TokenAmountINR, ExpiryDate, Status [Active, Locked, Allocating, Completed]
│   └── BuyerBooking.cs      # BookingId, PoolId, BuyerName, PhoneNumber, CityRto, SelectedVariant, SelectedColor, PaymentStatus, BookingDate
│
├── Services/
│   ├── ICatalogApi.cs       # Refit REST API contract ([Get("/api/v1/pools")], [Post("/api/v1/bookings/commit")])
│   ├── ICatalogService.cs   # Business service abstraction
│   └── MockCatalogService.cs# Seeded repository returning the 4 live Indian vehicle pools
│
├── ViewModels/
│   ├── BaseViewModel.cs     # ObservableObject base with IsBusy & Title
│   ├── DealsViewModel.cs    # Pool catalog, city filtering, JoinPoolCommand (UPI commitment)
│   ├── BookingsViewModel.cs # User bookings, status details, cancel & refund command
│   └── SupportViewModel.cs  # How it works guide, WhatsApp concierge, and admin hotline
│
├── Views/
│   ├── DealsPage.xaml       # Curated deals feed with progress bars and UPI commit action
│   ├── BookingsPage.xaml    # My Bookings dashboard with offline dealer allocation tracker
│   └── SupportPage.xaml     # 4-step process guide and WhatsApp concierge button
│
├── Resources/
│   └── Images/
│       ├── car_icon.svg       # Deals tab icon
│       ├── clipboard_icon.svg # My Bookings tab icon
│       └── support_icon.svg   # Support tab icon
│
├── Platforms/Android/
│   └── AndroidManifest.xml   # Permissions and intent queries for WhatsApp/Dialer
│
├── AppShell.xaml             # Bottom TabBar configuration
├── MauiProgram.cs            # DI container registering Refit, Services, ViewModels, and Views
└── CarPoolIndia.csproj       # Project configuration targeting net10.0-android
```

---

## 🚘 Seeded Live Pools (`MockCatalogService.cs`)

| Vehicle & Variant | Target City / RTO | Min Buyers | Current | Discount & Value Perks |
| :--- | :--- | :---: | :---: | :--- |
| **Hyundai Creta**<br>SX (O) 1.5L Diesel AT | Bengaluru, Karnataka<br>*(KA-01 / KA-03 / KA-05)* | 20 | 14 *(70%)* | **6% Group Discount**<br>+ Complimentary 1st Year Zero-Dep Insurance (Worth ₹48,000) |
| **Tata Nexon**<br>Fearless Plus S 1.2L Turbo DCA | Pune & Mumbai, Maharashtra<br>*(MH-12 / MH-02)* | 25 | 21 *(84%)* | **8% Bulk Discount**<br>+ Free Genuine Tata Accessories Pack (Worth ₹32,000) |
| **Maruti Suzuki Brezza**<br>ZXi 1.5L Smart Hybrid 6AT | Delhi NCR<br>*(DL-01 to 12 & HR-26 Gurgaon)* | 15 | 9 *(60%)* | **5% Direct Cash Discount**<br>+ 3-Year Extended Warranty & Coating |
| **Mahindra Scorpio-N**<br>Z8 L 2.2L mHawk 4x4 AT | Hyderabad, Telangana<br>*(TS-09 / TS-10)* | 30 | 28 *(93%)* | **Flat ₹60,000 Bulk Bonus**<br>+ Fast-Track Priority Allocation & VIN Tagging |

---

## 🚀 Building & Running on Android

Ensure the .NET MAUI Android workload is installed:
```powershell
dotnet workload restore
```

To build and run directly on an Android emulator or connected device:
```powershell
cd CarPoolIndia
dotnet build -t:Run -f net10.0-android
```
Or open the solution in Visual Studio 2022 / VS Code with the .NET MAUI extension, select an Android emulator, and press **F5**.
