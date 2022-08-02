using Common.Utils.DBHelper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Common.Extentions.Setup
{
    public static class SqlSugarSetup
    {
        public static void AddSqlSugarSetup(this IServiceCollection service)
        {
            if(service == null) throw new ArgumentNullException(nameof(service));

            service.AddScoped<ISqlSugarClient>(service =>
            {
                return DBHelper.db;
            });
        }
    }
}
