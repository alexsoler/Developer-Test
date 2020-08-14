using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tarea1.Models;

namespace Tarea1.Utils
{
    public class CurrencyConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out object value))
            {
                var parameterValueString = Convert.ToString(value,
                                                            CultureInfo.InvariantCulture);
                if (parameterValueString == null)
                {
                    return false;
                }

                return Enum.IsDefined(typeof(CurrencyEnum), parameterValueString.ToUpper());
            }

            return false;
        }
    }
}