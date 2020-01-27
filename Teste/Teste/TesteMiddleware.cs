using App.Infrastructure.Domain;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Teste.Teste
{
    public class TesteMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TesteContext _contextDb;


        public TesteMiddleware(RequestDelegate next, TesteContext contextDb)
        {
            _next = next;
            _contextDb = contextDb;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                var error = new ServerError()
                {
                    Message = ex.Message
                };

                _contextDb.ServerErrors.Add(error);
                await _contextDb.SaveChangesAsync();

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var dto = new TesteDTO();

                dto.error = ex.Message;
                dto.instance = context.Request.Path;


                string jsonString = JsonConvert.SerializeObject(dto);
                //await context.Response.WriteAsync(jsonString, Encoding.UTF8);
                //await context.Response.WriteAsync(json);

                if (ex is ApplicationException)
                {
                    //await context.Response.WriteAsync(json);

                    await context.Response.WriteAsync(jsonString, Encoding.UTF8);

                }

                if (ex is Exception)
                {
                    //await context.Response.(json);
                    await context.Response.WriteAsync(jsonString, Encoding.UTF8);

                }

            }
        }
    }
}
