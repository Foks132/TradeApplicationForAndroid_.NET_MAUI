using MauiAppTrade.Models;
using Microsoft.Maui.Controls;
using System.Net.Http.Json;
using System.Text.Json;

namespace MauiAppTrade.Pages;

public partial class AddEditProductPage : ContentPage
{
    ApiContext apiContext = new ApiContext();
    public DProduct currentProduct { get; set; }
    public AddEditProductPage()
    {
        InitializeComponent();
        DataLoad();
    }

    public AddEditProductPage(DProduct _currentProduct)
    {
        InitializeComponent();
        currentProduct = _currentProduct;
        DataLoad();
    }

    /// <summary>
    /// Производит добавление товара
    /// </summary>
    /// <returns>Результат добавления</returns>
    private async Task<bool> AddProduct()
    {
        string productName;
        bool result;

        if (!string.IsNullOrWhiteSpace(EntryPrice.Text))
        {
            EntryPrice.Text.Replace(',', '.');
        }
        productName = currentProduct?.name;

        if (currentProduct.productType != null)
        {
            currentProduct.productTypeId = currentProduct.productType.id;
            currentProduct.productType = null;
        }

        if (currentProduct.id == 0) // Если товар новый (т.к. id товра = 0, это новый объект)
        {
            HttpResponseMessage httpResponse = await apiContext.AddProduct(currentProduct); // Добавление товара
            currentProduct = await httpResponse.Content.ReadFromJsonAsync<DProduct>();
            result = httpResponse.StatusCode == System.Net.HttpStatusCode.Created;
        }
        else
        {
            result = await apiContext.UpdateProduct(currentProduct); // Изменение товара
        }

        if (result)
        {
            await DisplayAlert("Успешно!", $"Продукт {productName} был сохранён!", "Ок");
            return true;
        }
        else
        {
            await DisplayAlert("Ошибка!", $"При сохранении продукта {productName} возникла ошибка!", "Ок");
            return false;
        }
    }

    /// <summary>
    /// Производит добавление изображения
    /// </summary>
    /// <param name="file"></param>
    private async Task AddProductImage(FileResult file)
    {
        Stream stream = await file.OpenReadAsync(); // Открывает файловый менеджер устройства, ожидает изображение
        MemoryStream memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        byte[] bytes = memoryStream.ToArray();
        int imageCount;
        if (currentProduct.dProductImages != null) // Если у товара есть изображение
        {
            imageCount = currentProduct.dProductImages.Count;
        }
        else
        {
            imageCount = 0;
        }
        DProductImage productImage = new DProductImage() // Новый объект изображения товара
        {
            image = bytes,
            productId = currentProduct.id,
            name = $"{currentProduct.name} ({imageCount + 1})",
        };
        bool result;
        HttpResponseMessage httpResponse = await apiContext.AddProductImage(productImage); // Добавление изображения товара
        result = httpResponse.StatusCode == System.Net.HttpStatusCode.Created;
        if (result)
        {
            await DisplayAlert("Успешно!", $"Изображение {productImage.name} было сохранёно!", "Ок");
            DataUpdate();
        }
        else
        {
            await DisplayAlert("Ошибка!", $"При сохранении изображения продукта {productImage.name} возникла ошибка!", "Ок");
        }
    }

    /// <summary>
    /// Производит удаление изображения
    /// </summary>
    /// <param name="productImage">Изображение продукта</param>
    private async Task DeleteProductImage(DProductImage productImage)
    {
        if (productImage == null) // Если изображения нет
        {
            return;
        }
        bool result;
        result = await apiContext.DeleteProductImage(productImage.id); // Удаление изображения товара
        if (result)
        {
            await DisplayAlert("Успешно!", $"Изображение {productImage.name} было удалено!", "Ок");
        }
        else
        {
            await DisplayAlert("Ошибка!", $"При удалении изображения продукта {productImage.name} возникла ошибка!", "Ок");
        }
    }

    /// <summary>
    /// Производит загрузку данных
    /// </summary>
    private async void DataLoad()
    {
        List<DProductType> productTypes = await apiContext.GetProductType(); // Получение типов товара
        PickerProductType.ItemsSource = productTypes.ToList();
        if (currentProduct != null) // Если товар получен
        {
            CaruselViewProductImage.ItemsSource = currentProduct.dProductImages.ToList(); // Вывод изображений товара
        }
        else
        {
            currentProduct = new DProduct();
        }
        BindingContext = currentProduct; // Контекст привязки
        PickerProductType.SelectedItem = productTypes.FirstOrDefault(x => x.id == currentProduct.productTypeId); // Задаёт тип
    }

    /// <summary>
    /// Производит обновление данных
    /// </summary>
    private async void DataUpdate()
    {
        currentProduct = await apiContext.GetProduct(currentProduct.id); // Получает товар по Id
        DataLoad();
    }

    private async void BtnAddImage_Clicked(object sender, EventArgs e)
    {
        if (currentProduct.id == 0) // Если товар ещё не сохранён
        {
            bool result = await DisplayAlert("Внимание!", $"Для добавления изображения необходимо сохранить товар." +
                $"\nСохранить товар {currentProduct.name}?",
                "Сохранить", "Отмена");
            if (result == false) // Если пользователь нажал кнопку отмена
            {
                return;
            }
            result = await AddProduct();
            if (result == false) // Если при добавлении товара возникла ошибка 
            {
                return;
            }
            DataUpdate();
        }
        var file = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Загрузка изображения товара"
        });

        if (file == null)
        {
            return;
        }
        await AddProductImage(file);
    }

    private async void BtnDeleteImage_Clicked(object sender, EventArgs e)
    {
        DProductImage productImage = CaruselViewProductImage.CurrentItem as DProductImage; // Текущее изображение в карусели
        if (productImage == null) 
        {
            return;
        }
        await DeleteProductImage(productImage);
        DataUpdate();
    }

    private async void BtnProductSave_Clicked(object sender, EventArgs e)
    {
        await AddProduct();
    }
}