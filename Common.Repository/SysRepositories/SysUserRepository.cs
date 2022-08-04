using Common.Model.SysModels;
using Common.IRepository;

namespace Common.Repository
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
