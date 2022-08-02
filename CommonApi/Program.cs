using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Extentions.Autofactory;
using Common.Extentions.Middlerware;
using Common.Extentions.Setup;
using Common.Utils;
using CommonApi.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Text;

try
{
    // 1. 创建环境
    var builder = WebApplication.CreateBuilder(args);

    #region 2. 添加配置
    builder
        .Host
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule(new AutofacModuleRegister());
            builder.RegisterModule<AutofacPropertityModuleReg>();
        })
        .ConfigureLogging((hostContext, config) =>
        {
            config.AddFilter("System", LogLevel.Error);
            config.AddFilter("Microsoft", LogLevel.Error);
            config.SetMinimumLevel(LogLevel.Error);
            config.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config"));
        })
        .ConfigureAppConfiguration((hostContext, config) =>
        {
            //添加配置文件
            config.Sources.Clear();
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //config.AddConfigurationApollo("appsettings.apollo.json");
        });
    #endregion

    #region 3. 添加服务
    // 配置获取服务
    builder.Services.AddSingleton(new AppConfig(builder.Configuration, builder.Environment.ContentRootPath, builder.Environment.WebRootPath));

    // ORM数据库链接服务
    builder.Services.AddSqlSugarSetup();

    // 接口跨域访问服务
    builder.Services.AddCorsSetup();

    // http请求服务
    builder.Services.AddHttpContextSetup();

    // 接口文档
    builder.Services.AddSwaggerSetup();

    builder.Services.AddAuthorizationSetup();
    if (AuthPermission.IsUseIds4)
    {
        builder.Services.AddAuthentication_Ids4Setup();
    }
    else
    {
        builder.Services.AddAuthentication_JWTSetup();
    }

    // 部署方式配置
    builder.Services
        .Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
        .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

    // 控制器
    builder.Services.AddControllers(o =>
    {
        //配置全局过滤器
        o.Filters.Add(typeof(GlobalExceptionFilter));
        o.Conventions.Insert(0, new GlobalRouteAuthorizeConvention());
        o.Conventions.Insert(0, new GlobalRoutePrefixFilter(new RouteAttribute(RoutePrefix.Name)));
    })
    .AddNewtonsoftJson(options =>
    {
        //json格式化全局配置
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    #endregion

    #region 4.创建应用, 配置管道
    var app = builder.Build();

    // 静态文件，默认文档
    app.UseStaticFilesMiddler();

    // API Doc
    app.UseSwaggerMiddle();

    if (AppConfig.Get("HttpRequest", "Https").ObjToBool())
    {
        app.UseHttpsRedirection();
    }
    // 自动生成种子数据
    app.UseSeedMiddler();

    //路由与终结点匹配规则
    app.UseRouting();

    #region 权限认证必须位于 UseRouting() 与 UseEndpoints()之间
    // 用户认证
    app.UseAuthentication();
    // 授权是否通过
    app.UseAuthorization();
    #endregion

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    });
    #endregion

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ObjToString());
}
