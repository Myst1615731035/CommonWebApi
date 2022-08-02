using Common.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extentions.Setup
{
    /// <summary>
    /// Cors 启动服务
    /// </summary>
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddCors(c =>
            {
                if (!AppConfig.Get(new string[] { "HttpRequest", "Cors", "EnableAllIPs" }).ObjToBool())
                {
                    c.AddPolicy(AppConfig.Get(new string[] { "HttpRequest", "Cors", "PolicyName" }),
                        policy =>
                        {
                            policy
                            .WithOrigins(AppConfig.Get(new string[] { "HttpRequest", "Cors", "IPs" }).Split(','))
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                        });
                }
                else
                {
                    //允许任意跨域请求
                    c.AddPolicy(AppConfig.Get(new string[] { "HttpRequest", "Cors", "PolicyName" }),
                        policy =>
                        {
                            policy
                            .SetIsOriginAllowed((host) => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                        });
                }
            });
        }
    }
}
