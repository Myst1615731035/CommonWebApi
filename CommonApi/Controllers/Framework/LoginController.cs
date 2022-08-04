using Common.IService;
using Common.Model.ApiModel;
using Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommonApi.Controllers.Framework
{
    /// <summary>
    /// 登录控制
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        #region 注入
        private ISysUserServices services;
        public LoginController(ISysUserServices _services)
        {
            services = _services;
        }
        #endregion

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ContentJson<object>> Token([FromBody] dynamic data)
        {
            var res = new ContentJson<object>()
            {
                success = false,
                msg = "登录失败"
            };
            string account = data.account, password = data.password;
            password = MD5Helper.MD5Encrypt32(password);
            var tokenModel = await services.GetUserInfoToken(account, password);
            if(tokenModel.IsNotEmptyOrNull() && tokenModel.Uid.IsNotEmptyOrNull())
            {
                res = new ContentJson<object>()
                {
                    success = true,
                    msg = "登录成功",
                    data = new { token = JwtHelper.IssueJwt(tokenModel), expire = DateTime.Now.AddMinutes(AppConfig.Get("Program", "ExpiredTime").ObjToInt()) }
                };
            }
            else
            {
                res.msg += "; 账户或密码不正确";
            }
            return res;
        }
        #endregion
    }
}
