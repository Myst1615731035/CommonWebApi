using Common.IService.Base;
using Common.Model.ApiModel;
using Common.Model.SysModels;

namespace Common.IServices
{	
	/// <summary>
	/// IRolePermissionButtonServices
	/// </summary>	
	public partial interface IRolePermissionButtonServices :IBaseService<RolePermissionButton>
    {
        Task<List<PermissionItem>> GetPermissionItem();
    }
}
	