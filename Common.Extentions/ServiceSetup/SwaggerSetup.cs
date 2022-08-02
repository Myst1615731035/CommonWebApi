using Common.Utils;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Common.Extentions.Setup
{
    public static class SwaggerSetup
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SwaggerSetup));

        public static void AddSwaggerSetup(this IServiceCollection service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            service.AddSwaggerGen(option =>
            {
                var apiName = AppConfig.Get("Program", "Name");
                option.SwaggerDoc("V1", new OpenApiInfo()
                {
                    Version = "V1",
                    Title = $"{apiName} 接口文档——.Net Core 6.x",
                    Description = $"{apiName} HTTP API V1"
                });
                option.OrderActionsBy(t => t.RelativePath);
            });
            //service.AddSwaggerGenNewtonsoftSupport();
        }
    }
}
