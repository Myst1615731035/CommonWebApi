using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public static class GlobalConfig
    {
        public const string AudienceSecretString = "OnMLgh6wDYvcqF2c0HV6Hf9TG8fD6z6d";
    }

    public static class AuthPermission
    {
        /// <summary>
        /// 默认授权策略
        /// </summary>
        public const string Name = "Permission";
        /// <summary>
        /// 当前项目是否启用IDS4权限方案
        /// true：表示启动IDS4
        /// false：表示使用JWT
        public static bool IsUseIds4 = false;
    }

    /// <summary>
    /// 自定义路由前缀
    /// </summary>
    public static class RoutePrefix
    {
        public const string Name = "";
    }
}
