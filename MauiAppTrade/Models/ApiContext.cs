using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace MauiAppTrade.Models
{
    class ApiContext
    {
        // Адрес сервера API
        string apiUrl = "http://192.168.88.223:5019/api/";
        HttpClient client = new HttpClient();

        public ApiContext()
        {
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Получение списка типов товара
        /// </summary>
        /// <returns>Список типов товара</returns>
        public async Task<List<DProductType>> GetProductType()
        {
            //получение список продуктов по адресу http://192.168.88.223:5019/api/DProductTypes
            List<DProductType> productTypeList = await client.GetFromJsonAsync<List<DProductType>>($"DProductTypes");
            return productTypeList;
        }

        /// <summary>
        /// Получение списка товаров
        /// </summary>
        /// <returns>Список товаров</returns>
        public async Task<List<DProduct>> GetProduct()
        {
            //получение список продуктов по адресу http://192.168.1.116:5019/api/DProducts
            List<DProduct> productList = await client.GetFromJsonAsync<List<DProduct>>($"DProducts");
            return productList;
        }

        /// <summary>
        /// Получение товара по Id
        /// </summary>
        /// <param name="id">Id товара</param>
        /// <returns>Товар</returns>
        public async Task<DProduct> GetProduct(int id)
        {
            //получение список продуктов по адресу http://192.168.1.116:5019/api/DProducts/id
            DProduct product = await client.GetFromJsonAsync<DProduct>($"DProducts/{id}");
            return product;
        }

        /// <summary>
        /// Добавляет товара
        /// </summary>
        /// <param name="product">Товар</param>
        /// <returns>Результат добавления</returns>
        public async Task<HttpResponseMessage> AddProduct(DProduct product)
        {
            return await client.PostAsJsonAsync<DProduct>($"DProducts", product);
        }

        /// <summary>
        /// Редактирование товара
        /// </summary>
        /// <param name="product">Товар</param>
        /// <returns>Резульат операции</returns>
        public async Task<bool> UpdateProduct(DProduct product)
        {
            var addResult = await client.PutAsJsonAsync<DProduct>($"DProducts/{product.id}", product);
            return addResult.IsSuccessStatusCode;
        }

        /// <summary>
        /// Получение списка изображений
        /// </summary>
        /// <returns>Список изображений</returns>
        public async Task<List<DProductImage>> GetProductImage()
        {
            List<DProductImage> productImageList = await client.GetFromJsonAsync<List<DProductImage>>($"DProductImages");
            return productImageList;
        }

        /// <summary>
        /// Добавляет изображение продукта
        /// </summary>
        /// <param name="productImage">Изображение продукта</param>
        /// <returns>Результат добавления</returns>
        public async Task<HttpResponseMessage> AddProductImage(DProductImage productImage)
        {
            return await client.PostAsJsonAsync<DProductImage>($"DProductImages", productImage);
        }

        /// <summary>
        /// Удаляет изображение продукта по Id
        /// </summary>
        /// <param name="id">Id изображения</param>
        /// <returns>Результат удаления</returns>
        public async Task<bool> DeleteProductImage(int id)
        {
            var addResult = await client.DeleteAsync($"DProductImages/{id}");
            return addResult.IsSuccessStatusCode;
        }
    }
}
