using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tarea1.Interfaces;
using Tarea1.Models;

namespace Tarea1.Controllers
{
    [Route("latest")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private ICurrencyService _currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("{currency}")]
        public async Task<IActionResult> Get(CurrencyEnum currency)
        {
            var response = await _currencyService.CurrencyToRates(currency);

            if (!response.Success)
            {
                return Conflict(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpGet("{baseCurrency}-{versusCurrency}")]
        public async Task<IActionResult> Get(CurrencyEnum baseCurrency, CurrencyEnum versusCurrency)
        {
            var response = await _currencyService.CurrencyToRate(baseCurrency, versusCurrency);

            if (!response.Success)
            {
                return Conflict(response.Message);
            }

            return Ok(response.Data);
        }
    }
}