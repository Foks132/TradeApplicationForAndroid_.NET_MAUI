using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiAppTrade.Models
{
    public class DProductImage
    {
        public int id { get; set; }
        public int productId { get; set; }
        public byte[] image { get; set; }
        public string name { get; set; }
        [JsonIgnore]
        public virtual DProduct product { get; set; }

        /// <summary>
        /// Изображение товара
        /// </summary>
        [JsonIgnore]
        public ImageSource imageSource
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(this.image));
            }
        }
    }
}
