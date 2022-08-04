using Common.IService.Base;
using Common.Model.SysModels;
using Common.Utils;

namespace Common.IService
{
    /// <summary>
    /// ISysServices
    /// </summary>	
    public partial interface ISysUserServices : IBaseService<SysUser>
    {
        #region 用户登录以及用户信息获取
        Task<TokenModel> GetUserInfoToken(string account, string password);

        Task<object> GetUserInfo(string id);

        Task<List<Permission>> GetUserAuth(string id);
        #endregion
    }
}
