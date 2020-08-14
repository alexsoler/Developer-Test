using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tarea1.Models
{
    public class CurrenciesContainer
    {
        public CurrenciesContainer()
        {
            Currencies = new List<Currency>();
        }
        public DateTime Date { get; set; }
        public IList<Currency> Currencies { get; set; }
    }
}