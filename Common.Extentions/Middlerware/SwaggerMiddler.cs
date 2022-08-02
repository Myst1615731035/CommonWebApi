using Common.Utils;
using log4net;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extentions.Middlerware
{
    public static class SwaggerMiddler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SwaggerMiddler));

        public static void UseSwaggerMiddle(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                var apiName = AppConfig.Get("Program", "Name");
                var version = "V1";
                o.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{apiName} {version}");
                o.RoutePrefix = "ApiDoc";
            });
        }
    }
}
