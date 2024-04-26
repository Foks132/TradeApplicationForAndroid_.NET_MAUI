using MauiAppTrade.Models;

namespace MauiAppTrade.Pages;

public partial class ProductDetailsPage : ContentPage
{
    private DProduct _currentProduct;
    public ProductDetailsPage()
    {
        InitializeComponent();
    }

    public ProductDetailsPage(DProduct currentProduct)
    {
        InitializeComponent();
        _currentProduct = currentProduct; // Присвоение переменной класса параметр из параметра конструктора перегрузки
    }

    /// <summary>
    /// Производит обновление данных
    /// </summary>
    private async void DataUpdate()
    {
        ApiContext apiContext = new ApiContext();
        _currentProduct = await apiContext.GetProduct(_currentProduct.id); // Служит для обновления при изменении
        CaruselViewProductImage.ItemsSource = _currentProduct.dProductImages.ToList(); // Вывод изображений товара
        BindingContext = _currentProduct; // Контекст привязки
    }

    private void BtnEditProduct_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddEditProductPage(_currentProduct)); // Навигация на страницу редактирования товара
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        DataUpdate();
    }
}
