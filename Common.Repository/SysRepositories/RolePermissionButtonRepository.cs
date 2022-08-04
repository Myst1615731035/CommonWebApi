using Common.Model.SysModels;
using Common.IRepository;

namespace Common.Repository
{
    /// <summary>
    /// RolePermissionButtonRepository
    /// </summary>	
    public partial class RolePermissionButtonRepository : BaseRepository<RolePermissionButton>, IRolePermissionButtonRepository
    {
        public RolePermissionButtonRepository() : base()
        {
        }
    }
}
