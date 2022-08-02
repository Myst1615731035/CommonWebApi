using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;


namespace Common.Utils
{
    public class Configs
    {
        private readonly static NameValueCollection settings = ConfigurationManager.AppSettings;

        #region 获取配置文件
        /// <summary>
        /// 获取单个配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfig(string key)
        {
            var value = "";
            if (key.IsNotEmptyOrNull())
            {
                value = settings[key];
            }
            return value.IsNotEmptyOrNull() ? value : "";
        }

        /// <summary>
        /// 获取配置文件并转为list
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetConfig(string key,char split)
        {
            var list = new List<string>();
            if (key.IsNotEmptyOrNull())
            {
                var value = settings[key];
                if (value.IsNotEmptyOrNull())
                {
                    list = value.Split(split, System.StringSplitOptions.RemoveEmptyEntries).ToList();
                    list.ForEach(t => t = t.Trim());
                }
            }
            return list;
        }
        #endregion
    }
}
