using MauiAppTrade.Models;

namespace MauiAppTrade.Pages;

public partial class ProductsPage : ContentPage
{
    ApiContext apiContext = new ApiContext();
    List<DProduct> products = new List<DProduct>();
    List<DProduct> productsCollection = new List<DProduct>();

    public ProductsPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Производит загрузку данных
    /// </summary>
    private async void DataLoad()
    {
        if (products != null)
        {
            products.Clear();
            CollectionViewProducts.ItemsSource = null;
        }
        ActivityIndicator.IsRunning = true; // Индикатор загрузки
        products = await apiContext.GetProduct(); 
        List<DProductType> productTypes = await apiContext.GetProductType(); 
        productTypes.Insert(0, new DProductType() { name = "Все" }); // Добавление нового типа в список
        PickerProductType.ItemsSource = productTypes.ToList();
        PickerProductType.SelectedIndex = 0;
        ActivityIndicator.IsRunning = false; // Индикатор
    }

    /// <summary>
    /// Производит обновление данных
    /// </summary>
    private void DataUpdate()
    {
        productsCollection = products;
        if (PickerProductType.SelectedIndex != 0)
        {
            productsCollection = productsCollection.Where(x =>
                x.productTypeId == (PickerProductType.SelectedItem as DProductType).id).ToList();
        }
        if (!string.IsNullOrWhiteSpace(EntryProductSearch.Text))
        {
            productsCollection = productsCollection.Where(x =>
                x.name.ToLower().Contains(EntryProductSearch.Text.ToLower())).ToList();
        }
        CollectionViewProducts.ItemsSource = productsCollection;
    }

    private void PickerProductType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PickerProductType.SelectedIndex != -1)
        {
            DataUpdate();
        }
    }

    private void EntryProductSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        DataUpdate();
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        DataLoad();
    }

    private void CollectionViewProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Navigation.PushAsync(new ProductDetailsPage(e.CurrentSelection[0] as DProduct));
    }
}



