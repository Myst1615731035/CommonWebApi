using Common.Model.ApiModel;
using Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;

namespace CommonApi.Filter
{
    public class GlobalExceptionFilter: IAsyncExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var res = new ContentJson<string>()
            {
                msg = "系统繁忙",
                status = 500,
            };

            if (_env.EnvironmentName.ObjToString().Equals("Development"))
            {
                res.data = context.Exception.ObjToString();//堆栈信息
            }
            LogHelper.Error(context.Exception);
            context.Result = new ContentResult() { Content = res.ToJson() };
        }
    }
}
