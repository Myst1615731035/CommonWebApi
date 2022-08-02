using Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extentions.Setup
{
    /// <summary>
    /// 配置SignalR
    /// </summary>
    public static class SignalRSetUp
    {
        public static void AddSignalRSetUp(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSignalR().AddNewtonsoftJsonProtocol();

            //使用时请在Program.cs中的UseEndpoint插件中使用下面这句代码，对SignalR路由进行配置
            //endpoints.MapHub<ChatHub>("/api/chatHub");
        }
    }
}
