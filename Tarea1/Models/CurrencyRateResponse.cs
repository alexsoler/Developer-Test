using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tarea1.Utils;

namespace Tarea1.Models
{
    public class CurrencyRateResponse
    {
        public string Base { get; set; }
        public string Versus { get; set; }

        [DataType(DataType.Date)]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime Date { get; set; }
        public decimal Rate { get; set; }
    }
}