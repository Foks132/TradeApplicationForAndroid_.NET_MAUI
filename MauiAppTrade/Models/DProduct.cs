using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiAppTrade.Models
{
    public class DProduct
    {
        public int id { get; set; }
        public int productTypeId { get; set; }
        public string manufacturer { get; set; }
        public string name { get; set; }
        public decimal? price { get; set; }
        public int? inStock { get; set; }
        [JsonInclude]
        public virtual DProductType productType { get; set; }
        [JsonInclude]
        public virtual List<DProductImage> dProductImages { get; set; }

        /// <summary>
        /// Главное изображение товара
        /// </summary>
        [JsonIgnore]
        public ImageSource mainImage
        {
            get
            {
                if (this.dProductImages.Count > 0)
                {
                    return dProductImages.First().imageSource;
                }
                return null;
            }
        }
    }
}
