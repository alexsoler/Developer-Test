using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Tarea1.Interfaces;
using Tarea1.Models;

namespace Tarea1.Services
{
    public class CurrencyService : ICurrencyService
    {
        public IConfiguration Configuration { get; }

        public CurrencyService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<CurrenciesContainer> GetCurrencies()
        {
            var currenciesContainer = new CurrenciesContainer();
            currenciesContainer.Currencies.Add(new Currency { Name = "EUR", Rate = 1M });

            var url = Configuration.GetValue<string>("ecburl");

            XDocument doc = await getXMLDocument(url);

            XElement dateNode = doc.Descendants("{http://www.ecb.int/vocabulary/2002-08-01/eurofxref}Cube").First(el => el.Attribute("time") != null);
            currenciesContainer.Date = DateTime.ParseExact(dateNode.Attribute("time").Value, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            foreach (var item in dateNode.Elements())
            {
                currenciesContainer.Currencies.Add(new Currency
                {
                    Name = item.Attribute("currency").Value,
                    Rate = Convert.ToDecimal(item.Attribute("rate").Value)
                });
            }

            return currenciesContainer;
        }

        private async Task<XDocument> getXMLDocument(string url)
        {
            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync(url);

            var stream = await result.Content.ReadAsStreamAsync();

            return XDocument.Load(stream);
        }

        private decimal ConvertCurrency(decimal amount, Currency x, Currency y)
        {
            var eur = amount / x.Rate;
            return decimal.Round(eur * y.Rate, 4);
        }

        public async Task<ServiceResponse<CurrencyRatesResponse>> CurrencyToRates(CurrencyEnum baseCurrency)
        {
            CurrenciesContainer container = new CurrenciesContainer();
            try
            {
                container = await GetCurrencies();
            }
            catch (HttpRequestException ex)
            {
                return new ServiceResponse<CurrencyRatesResponse>{
                    Success = false,
                    Message = @"A problem was found in an external HTTP request to 
                    https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml, try again"
                };
            }

            var data = new CurrencyRatesResponse();
            data.Base = baseCurrency.ToString();
            data.Date = container.Date;

            var currency1 = container.Currencies.FirstOrDefault(x => x.Name == baseCurrency.ToString());

            foreach (var item in Enum.GetValues(typeof(CurrencyEnum)))
            {
                var currency2 = container.Currencies.FirstOrDefault(x => x.Name == item.ToString()
                    && x.Name != baseCurrency.ToString()
                );

                if (currency2 != null)
                {
                    var convertResult = ConvertCurrency(1M, currency1, currency2);

                    data.Rates.Add(currency2.Name, convertResult);
                }
            }

            return new ServiceResponse<CurrencyRatesResponse> {
                Success = true,
                Data = data
            };
        }

        public async Task<ServiceResponse<CurrencyRateResponse>> CurrencyToRate(CurrencyEnum baseCurrency, CurrencyEnum versusCurrency)
        {
            CurrenciesContainer container = new CurrenciesContainer();
            try
            {
                container = await GetCurrencies();
            }
            catch (HttpRequestException ex)
            {
                return new ServiceResponse<CurrencyRateResponse>{
                    Success = false,
                    Message = @"A problem was found in an external HTTP request to 
                    https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml, try again"
                };
            }

            var data = new CurrencyRateResponse();
            data.Base = baseCurrency.ToString();
            data.Versus = versusCurrency.ToString();
            data.Date = container.Date;

            var currency1 = container.Currencies.FirstOrDefault(x => x.Name == baseCurrency.ToString());
            var currency2 = container.Currencies.FirstOrDefault(x => x.Name == versusCurrency.ToString());

            data.Rate = ConvertCurrency(1M, currency1, currency2);

            return new ServiceResponse<CurrencyRateResponse> {
                Success = true,
                Data = data
            };
        }
    }
}