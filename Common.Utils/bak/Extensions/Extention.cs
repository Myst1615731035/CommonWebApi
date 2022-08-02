using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace Common.Utils
{
    public static class Extention
    {
        #region 类型转换
        /// <summary>
        /// 对象转Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            if (obj.IsNotEmptyOrNull())
            {
                return JsonConvert.SerializeObject(obj);
            }
            return JsonConvert.SerializeObject(new { });
        }

        /// <summary>
        /// 字符串转byte数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string str)
        {
            return Encoding.Default.GetBytes(str);
        }

        /// <summary>
        /// byte数组转字符串
        /// </summary>
        /// <param name="byteArr"></param>
        /// <returns></returns>
        public static string ToString(this byte[] byteArr)
        {
            return Encoding.Default.GetString(byteArr);
        }

        /// <summary>
        /// 字符串转JObject
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static JObject ToJObject(this string str)
        {
            if (!str.IsNotEmptyOrNull())
            {
                return new JObject();
            }
            return JObject.Parse(str);
        }

        /// <summary>
        /// json转List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string json)
        {
            if (!json.IsNotEmptyOrNull())
            {
                return new List<T>();
            }
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        /// <summary>
        /// IEnumerator 转 List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IEnumerator<T> enumerator)
        {
            var list = new List<T>();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }
            return list;
        }

        /// <summary>
        /// 流转字符串
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        public static string ToString(this Stream sm)
        {
            if(sm.Length.IsNotEmptyOrNull())
            {
                return new StreamReader(sm).ReadToEnd();
            }
            return "";
        }
        #endregion

        #region 数据处理
        /// <summary>
        /// 字符串中多个连续空格转为一个空格
        /// </summary>
        /// <param name="str">待处理的字符串</param>
        /// <returns>合并空格后的字符串</returns>
        public static string MergeSpace(this string str)
        {
            if (str != string.Empty && str != null && str.Length > 0)
            {
                str = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(str, " ");
            }
            return str;
        }
        #endregion
    }
}
