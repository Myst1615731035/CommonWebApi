using Common.Utils.DBHelper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Common.Extentions.Setup
{
    public static class AuthorizeSetup
    {
        /// <summary>
        /// 系统 授权服务 配置
        /// </summary>
        /// <param name="service"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddAuthorizationSetup(this IServiceCollection service)
        {
            if(service == null) throw new ArgumentNullException(nameof(service));

            service.AddScoped<ISqlSugarClient>(service =>
            {
                return DBHelper.db;
            });
        }
    }
}
