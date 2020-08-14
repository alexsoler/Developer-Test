using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tarea1.Models;

namespace Tarea1.Interfaces
{
    public interface ICurrencyService
    {
        Task<ServiceResponse<CurrencyRatesResponse>> CurrencyToRates(CurrencyEnum baseCurrency);
        Task<ServiceResponse<CurrencyRateResponse>> CurrencyToRate(CurrencyEnum baseCurrency, CurrencyEnum versusCurrency);
    }
}