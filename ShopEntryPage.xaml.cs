using Microsoft.Maui.Controls;
using StetcoBiancaLab7.Models;

namespace StetcoBiancaLab7;

public partial class ShopEntryPage : ContentPage
{
    public ShopEntryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Obține datele și le setează ca ItemsSource pentru CollectionView
            collectionView.ItemsSource = await App.Database.GetShopsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"Nu s-a putut încărca lista: {ex.Message}", "OK");
        }
    }

    async void OnShopAddedClicked(object sender, EventArgs e)
    {
        // Navighează către pagina pentru adăugarea unui magazin nou
        await Navigation.PushAsync(new ShopPage
        {
            BindingContext = new Shop()
        });
    }

    async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Obține elementul selectat
        if (e.CurrentSelection.FirstOrDefault() is Shop selectedShop)
        {
            await Navigation.PushAsync(new ShopPage
            {
                BindingContext = selectedShop
            });

            // Resetează selecția
            collectionView.SelectedItem = null;
        }
    }
}
