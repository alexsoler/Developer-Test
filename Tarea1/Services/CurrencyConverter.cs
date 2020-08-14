using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Configuration;

namespace Tarea1.Services
{
    public class CurrencyConverter
    {
        public IConfiguration Configuration { get; }
        
        public CurrencyConverter(IConfiguration configuration)
        {
            Configuration = configuration;
        }   
    }
}