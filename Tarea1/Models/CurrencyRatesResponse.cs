using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tarea1.Utils;

namespace Tarea1.Models
{
    public class CurrencyRatesResponse
    {
        public CurrencyRatesResponse()
        {
            Rates = new Dictionary<string, decimal>();
        }
        public string Base { get; set; }

        [DataType(DataType.Date)]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}