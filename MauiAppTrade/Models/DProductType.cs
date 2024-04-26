using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiAppTrade.Models
{
    public class DProductType
    {
        public int id { get; set; }
        public string name { get; set; }
        [JsonIgnore]
        public virtual List<DProduct> dProducts { get; set; }   
    }
}
