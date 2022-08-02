using Common.Model.Base;
using SqlSugar;

namespace Common.Model.SysModels
{
    ///<summary>
    ///RolePermissionButton
    ///</summary>
    [SugarTable("Sys_RolePermissionButton")]
    public partial class RolePermissionButton : RootEntity<string>
    {
        [SugarColumn(IsNullable = false, ColumnDescription = "角色ID", ColumnDataType = "varchar", Length = 500)]
        public string RoleId { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "菜单ID", ColumnDataType = "varchar", Length = 50)]
        public string PermissionId { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "按钮ID", ColumnDataType = "varchar", Length = 50)]
        public string ButtonId { get; set; }
    }
}
