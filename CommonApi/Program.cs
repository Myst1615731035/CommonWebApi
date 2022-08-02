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
    // 1. ��������
    var builder = WebApplication.CreateBuilder(args);

    #region 2. �������
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
            //��������ļ�
            config.Sources.Clear();
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //config.AddConfigurationApollo("appsettings.apollo.json");
        });
    #endregion

    #region 3. ��ӷ���
    // ���û�ȡ����
    builder.Services.AddSingleton(new AppConfig(builder.Configuration, builder.Environment.ContentRootPath, builder.Environment.WebRootPath));

    // ORM���ݿ����ӷ���
    builder.Services.AddSqlSugarSetup();

    // �ӿڿ�����ʷ���
    builder.Services.AddCorsSetup();

    // http�������
    builder.Services.AddHttpContextSetup();

    // �ӿ��ĵ�
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

    // ����ʽ����
    builder.Services
        .Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
        .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

    // ������
    builder.Services.AddControllers(o =>
    {
        //����ȫ�ֹ�����
        o.Filters.Add(typeof(GlobalExceptionFilter));
        o.Conventions.Insert(0, new GlobalRouteAuthorizeConvention());
        o.Conventions.Insert(0, new GlobalRoutePrefixFilter(new RouteAttribute(RoutePrefix.Name)));
    })
    .AddNewtonsoftJson(options =>
    {
        //json��ʽ��ȫ������
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

    #region 4.����Ӧ��, ���ùܵ�
    var app = builder.Build();

    // ��̬�ļ���Ĭ���ĵ�
    app.UseStaticFilesMiddler();

    // API Doc
    app.UseSwaggerMiddle();

    if (AppConfig.Get("HttpRequest", "Https").ObjToBool())
    {
        app.UseHttpsRedirection();
    }
    // �Զ�������������
    app.UseSeedMiddler();

    //·�����ս��ƥ�����
    app.UseRouting();

    #region Ȩ����֤����λ�� UseRouting() �� UseEndpoints()֮��
    // �û���֤
    app.UseAuthentication();
    // ��Ȩ�Ƿ�ͨ��
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
