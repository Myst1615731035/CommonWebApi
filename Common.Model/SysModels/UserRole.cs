using Common.Model.Base;
using SqlSugar;

namespace Common.Model.SysModels
{
    ///<summary>
    ///UserRole
    ///</summary>
    [SugarTable("Sys_UserRole")]
    public partial class UserRole : RootEntity<string>
    {
        [SugarColumn(IsNullable = false, ColumnDescription = "用户ID", ColumnDataType = "varchar", Length = 50)]
        public string UserId { get; set; }

        [SugarColumn(IsNullable = false, ColumnDescription = "角色ID", ColumnDataType = "varchar", Length = 500)]
        public string RoleId { get; set; }
    }
}
