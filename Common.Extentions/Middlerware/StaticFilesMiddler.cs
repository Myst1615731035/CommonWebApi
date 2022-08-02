using Common.Utils;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extentions.Middlerware
{
    /// <summary>
    /// 扩展静态文件配置，与默认文档配置
    /// </summary>
    public static class StaticFilesMiddler
    {
        private static ILog log = LogManager.GetLogger(typeof(StaticFilesMiddler));

        public static void UseStaticFilesMiddler(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            #region 默认文件配置
            app.UseDefaultFiles();

            //扩展
            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            #endregion

            #region 静态文件配置
            app.UseStaticFiles();

            //扩展：添加额外的静态文件目录配置
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(AppConfig.ContentRootPath, "Static")),
            //    RequestPath = "/Static"
            //});
            #endregion
        }
    }
}
