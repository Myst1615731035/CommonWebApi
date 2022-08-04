using Common.Model.Base;
using SqlSugar;

namespace Common.Model.SysModels
{
    ///<summary>
    ///Permission
    ///</summary>
    [SugarTable("Sys_Permission")]
    public partial class Permission : RootEntity<string>
    {
        [SugarColumn(IsNullable = false, ColumnDescription = "父级主键，最顶级菜单的父级ID为空字符串", ColumnDataType = "varchar", Length = 50, DefaultValue = "")]
        public string Pid { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "数据查询的接口ID", ColumnDataType = "varchar", Length = 50)]
        public string Fid { get; set; }

        [SugarColumn(IsNullable = false, ColumnDescription = "菜单名称", ColumnDataType = "varchar", Length = 50)]
        public string Name { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "路径", ColumnDataType = "varchar", Length = 50)]
        public string Path { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "图标", ColumnDataType = "varchar", Length = 50)]
        public string Icon { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "描述", ColumnDataType = "varchar", Length = 50)]
        public string Description { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<Button> Buttons { get; set; } = new List<Button>();

        [SugarColumn(IsIgnore = true)]
        public List<Permission> Children { get; set; } = new List<Permission>();
    }
}
