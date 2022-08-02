using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class Extentions
    {
        #region ToJObject
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
        #endregion

        #region ToList || ToTree
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
        /// 将列表转换为树形结构
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">数据</param>
        /// <param name="rootwhere">根条件</param>
        /// <param name="childswhere">节点条件</param>
        /// <param name="addchilds">添加子节点</param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static List<T> ToTree<T>(this List<T> list, Func<T, T, bool> rootwhere, Func<T, T, bool> childswhere, Action<T, IEnumerable<T>> addchilds, T entity = default(T))
        {
            var treelist = new List<T>();
            //空树
            if (list == null || list.Count == 0)
            {
                return list;
            }
            //是否存在根节点，如果不存在，直接返回原数组
            if (!list.Any<T>(e => rootwhere(entity, e)))
            {
                return treelist;
            }
            //树根
            if (list.Any<T>(e => rootwhere(entity, e)))
            {
                treelist.AddRange(list.Where(e => rootwhere(entity, e)));
            }
            //树叶
            foreach (var item in treelist)
            {
                if (list.Any(e => childswhere(item, e)))
                {
                    var nodedata = list.Where(e => childswhere(item, e)).ToList();
                    foreach (var child in nodedata)
                    {
                        //添加子集
                        var data = list.ToTree(childswhere, childswhere, addchilds, child);
                        addchilds(child, data);
                    }
                    addchilds(item, nodedata);
                }
            }
            return treelist;
        }
        #endregion

        #region ToString
        /// <summary>
        /// 流转字符串
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        public static string ToString(this Stream sm)
        {
            if (sm.Length.IsNotEmptyOrNull())
            {
                return new StreamReader(sm).ReadToEnd();
            }
            return "";
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
        #endregion

        #region ToJson
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
        #endregion

        #region Type Extention
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = string.Empty;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();
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

        #region 其他
        /// <summary>
        /// 对象字段值继承
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T ExtendPartProperty<T, T1>(T1 data)
        {
            T entity = Activator.CreateInstance<T>();
            Type type = entity.GetType();
            var pros = data.GetType().GetProperties().ToList();
            foreach (var prop in type.GetProperties())
            {
                var pro = pros.Find(t => t.Name == prop.Name);
                if (pro != null)
                {
                    prop.SetValue(entity, pro.GetValue(data, null));
                }
            }
            return entity;
        }
        #endregion
    }
}
