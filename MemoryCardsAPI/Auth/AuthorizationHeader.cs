using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class AuthorizationHeader
{
    private readonly RequestDelegate _next;

    public AuthorizationHeader(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authenticationCookieName = "token";
        var cookie = context.Request.Cookies[authenticationCookieName];
        if (cookie != null)
        {
            context.Request.Headers.Remove("Authorization");
            context.Request.Headers.Append("Authorization", "Bearer " + cookie);
        }

        await _next.Invoke(context);
    }
}
