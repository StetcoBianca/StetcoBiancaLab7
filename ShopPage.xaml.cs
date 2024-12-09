namespace StetcoBiancaLab7;

using Microsoft.Maui.Devices.Sensors;
using Plugin.LocalNotification;
using StetcoBiancaLab7.Models;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (BindingContext is not Shop shop)
        {
            await DisplayAlert("Eroare", "Nu există date valide pentru a salva.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(shop.ShopName) || string.IsNullOrWhiteSpace(shop.Adress))
        {
            await DisplayAlert("Eroare", "Toate câmpurile trebuie completate.", "OK");
            return;
        }

        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }

    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        if (BindingContext is not Shop shop || string.IsNullOrWhiteSpace(shop.Adress))
        {
            await DisplayAlert("Eroare", "Adresa este obligatorie pentru afișarea hărții.", "OK");
            return;
        }

        var locations = await Geocoding.GetLocationsAsync(shop.Adress);
        var shopLocation = locations?.FirstOrDefault();

        if (shopLocation == null)
        {
            await DisplayAlert("Eroare", "Locația nu a fost găsită.", "OK");
            return;
        }

        var myLocation = await Geolocation.GetLocationAsync();
        if (myLocation == null)
        {
            await DisplayAlert("Eroare", "Nu am putut obține locația curentă.", "OK");
            return;
        }

        var distance = myLocation.CalculateDistance(shopLocation, DistanceUnits.Kilometers);

        if (distance < 5)
        {
            var request = new NotificationRequest
            {
                Title = "Ai de făcut cumpărături în apropiere!",
                Description = $"Adresa magazinului: {shop.Adress}",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
            LocalNotificationCenter.Current.Show(request);
        }

        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat"
        };

        await Map.OpenAsync(shopLocation, options);
    }
}
