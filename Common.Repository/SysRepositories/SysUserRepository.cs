using Common.Model.SysModels;
using Common.Repository.ISysRepositories;

namespace Common.Repository.SysRepositories
{
    /// <summary>
    /// SysRepository
    /// </summary>	
    public partial class SysUserRepository : BaseRepository<SysUser>, ISysUserRepository
    {
        public SysUserRepository() : base()
        {
        }
    }
}
