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
        #region �û���¼�Լ��û���Ϣ��ȡ
        Task<TokenModel> GetUserInfoToken(string account, string password);

        Task<object> GetUserInfo(string id);

        Task<List<Permission>> GetUserAuth(string id);
        #endregion
    }
}
