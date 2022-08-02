using Common.Model.Base;
using SqlSugar;

namespace Common.Model.SysModels
{
    ///<summary>
    ///SysUser
    ///</summary>
    [SugarTable("Sys_SysUser")]
    public partial class SysUser : RootEntity<string>
    {

        [SugarColumn(IsNullable = false, ColumnDescription = "账户", ColumnDataType = "varchar", Length = 50)]
        public string Account { get; set; }

        [SugarColumn(IsNullable = false, ColumnDescription = "密码", ColumnDataType = "varchar", Length = 50)]
        public string Password { get; set; }

        [SugarColumn(IsNullable = false, ColumnDescription = "昵称", ColumnDataType = "varchar", Length = 50)]
        public string UserName { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "备注", ColumnDataType = "varchar", Length = 500)]
        public string Remark { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "头像", ColumnDataType = "varchar", Length = 500)]
        public string Avatar { get; set; }

        #region 个人信息
        [SugarColumn(IsNullable = true, ColumnDescription = "身份证明", ColumnDataType = "varchar", Length = 50)]
        public string IDCard { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "联系方式", ColumnDataType = "varchar", Length = 50)]
        public string Tel { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "邮箱", ColumnDataType = "varchar", Length = 50)]
        public string Email { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "住址", ColumnDataType = "varchar", Length = 50)]
        public string Address { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "性别", ColumnDataType = "int")]
        public int Sex { get; set; }
        #endregion
    }
}
