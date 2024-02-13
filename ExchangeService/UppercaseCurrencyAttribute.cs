using ExchangeService.RequestModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExchangeService
{
    public class UppercaseCurrencyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("exchangeRequest", out object value) && value is ExchangeRequest request)
            {
                request.BaseCurrency = request.BaseCurrency?.ToUpperInvariant();
                request.TargetCurrency = request.TargetCurrency?.ToUpperInvariant();
                request.ClientIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            }
        }
    }
}
