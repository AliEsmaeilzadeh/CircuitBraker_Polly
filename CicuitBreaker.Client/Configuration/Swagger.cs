using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicuitBreaker.Client.Configuration
{
    public static class Swagger
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(op =>
            {
                op.SwaggerDoc("V1", new OpenApiInfo { Version = "V1", Title = "Circuit Breaker Client" });
            });
        }

    }
}
