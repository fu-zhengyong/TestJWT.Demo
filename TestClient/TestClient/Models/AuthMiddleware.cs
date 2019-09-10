using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace TestClient.Models
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var result = await httpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsync("Unauthorized");

                //HttpClient http=new HttpClient();
                //string jsonResult = await http.GetStringAsync("获取token服务器地址");
                //if (!string.IsNullOrEmpty(jsonResult))
                //{
                //    var json = JToken.Parse(jsonResult);

                //    httpContext.Request.Headers.Add("Authorization", "Bearer "+json["token"]);
                //}

            }
            else
            {
                httpContext.User = result.Principal;
                await _next.Invoke(httpContext);
            }
        }
    }
}
