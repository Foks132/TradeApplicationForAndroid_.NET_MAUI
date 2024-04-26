using MauiAppTrade.Pages;

namespace MauiAppTrade;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    private void BtnAddProduct_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddEditProductPage());
    }

    private void BtnProducts_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ProductsPage());
    }
}

