using SqlSugar;

namespace Common.Utils
{
    /// <summary>
    /// 对SqlSugar.SqlFunc进行扩展，并添加到SqlSugar服务中
    /// </summary>
    public static class SqlFuncExt
    {
        public static List<SqlFuncExternal> SqlFuncExtList()
        {
            List<SqlFuncExternal> list = new List<SqlFuncExternal>();
            #region ToDateString
            list.Add(new SqlFuncExternal()
            {
                UniqueMethodName = "ToDateString",
                MethodValue = (expInfo, dbType, expContext) =>
                {
                    if(dbType == DbType.SqlServer)
                    {
                        var dateSql = "";
                        string format = "yyyy-MM-dd HH:mm:ss";
                        if (expInfo.Args[1].MemberValue.IsNotEmptyOrNull())
                        {
                            format = expInfo.Args[1].MemberValue.ObjToString();
                        }
                        switch (format)
                        {
                            case "yyyyMM":
                                dateSql = $" REPLACE(CONVERT(VarChar(7), {expInfo.Args[0].MemberName}, 120), '-', '') ";
                                break;
                            case "yyyy-MM":
                                dateSql = $" CONVERT(VarChar(7), {expInfo.Args[0].MemberName}, 120) ";
                                break;
                            case "yyyy-MM-dd HH:mm:ss":
                            default:
                                dateSql = $" CONVERT(VarChar(20), {expInfo.Args[0].MemberName}, 20) ";
                                break;
                        }
                        return $"(case WHEN {expInfo.Args[0].MemberName} is not null then {dateSql} ELSE '' END)";
                    }
                    else
                    {
                        throw new Exception($"在{dbType.GetGenericTypeName()}中，此方法未实现");
                    }
                }
            });
            #endregion

            #region Stuff
            list.Add(new SqlFuncExternal()
            {
                UniqueMethodName = "Stuff",
                MethodValue = (expInfo, dbType, expContext) =>
                {
                    if (dbType == DbType.SqlServer)
                    {
                        return $"(stuff(( {expInfo.Args[0].MemberValue} FOR xml path ( '' ) ),1,1,'' ))";
                    }
                    else
                    {
                        throw new Exception($"在{dbType.GetGenericTypeName()}中，此方法未验证");
                    }
                }
            });
            #endregion
            return list;
        }

        public static string ToDateString(DateTime? dateTime, string format)
        {
            throw new NotSupportedException("Can only be used in expressions");
        }
        public static string Stuff(string sql)
        {
            throw new NotSupportedException("Can only be used in expressions");
        }
    }
}
