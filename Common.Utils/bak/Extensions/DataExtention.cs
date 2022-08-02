using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Common.Utils
{
    public static class DataExtensions
    {
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

        /// <summary>
        /// 将List转换为DataTable
        /// </summary>
        /// <param name="list">请求数据</param>
        /// <returns></returns>
        public static DataTable ToTable<T>(this List<T> list)
        {
            //创建一个名为"tableName"的空表
            DataTable dt = new DataTable("tableName");

            //创建传入对象名称的列
            foreach (var item in list.FirstOrDefault().GetType().GetProperties())
            {
                dt.Columns.Add(item.Name);
            }
            //循环存储
            foreach (var item in list)
            {
                //新加行
                DataRow value = dt.NewRow();
                //根据DataTable中的值，进行对应的赋值
                foreach (DataColumn dtColumn in dt.Columns)
                {
                    int i = dt.Columns.IndexOf(dtColumn);
                    //基元元素，直接复制，对象类型等，进行序列化
                    if (value.GetType().IsPrimitive)
                    {
                        value[i] = item.GetType().GetProperty(dtColumn.ColumnName).GetValue(item).ObjToString();
                    }
                    else
                    {
                        value[i] = JsonConvert.SerializeObject(item.GetType().GetProperty(dtColumn.ColumnName).GetValue(item)).ObjToString();
                    }
                }
                dt.Rows.Add(value);
            }
            return dt;
        }

        /// <summary>
        /// 获取两个同类的对象的数据差异
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldObj"></param>
        /// <param name="newObj"></param>
        /// <param name="ignoreType">需要忽略的数据类型</param>
        /// <param name="ignoreColumn">需要忽略的属性字段名</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetDiffProp<T>(T oldObj, T newObj,List<Type> ignoreType = null, List<string> ignoreColumn = null)
        {
            var pairs = new Dictionary<string, object>();
            //获取属性集合
            var infos = typeof(T).GetProperties().ToList();
            //遍历属性值
            infos.ForEach(t =>
            {
                //如果不包含在需要忽略的数据类型或属性字段内，则进行处理
                if((!ignoreType.IsNotEmptyOrNull() || !ignoreType.Contains(t.PropertyType)) && (!ignoreColumn.IsNotEmptyOrNull() || !ignoreColumn.Contains(t.Name)))
                {
                    try
                    {
                        //如果两个对象的当前属性值不同
                        var from = oldObj.IsNotEmptyOrNull() ? t.GetValue(oldObj) : null;
                        var to = newObj.IsNotEmptyOrNull() ? t.GetValue(newObj) : null;
                        if (from.ObjToString() != to.ObjToString())
                        {
                            pairs.Add(t.Name, new { from, to });
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
            });
            return pairs;
        }

        /// <summary>
        /// 获取全文搜索表达式树
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetGSExpression<T>(Type type, string keyword, List<string> column = null)
        {
            Expression<Func<T, bool>> exp = LinqExtensions.IsTrue<T>();
            try
            {
                var infos = typeof(T).GetProperties().ToList();
                ParameterExpression parameter = Expression.Parameter(typeof(T), "param");
                infos.ForEach(p =>
                {
                    if (column.IsNotEmptyOrNull())
                    {
                        if (column.Contains(p.Name) && p.PropertyType == type)
                        {
                            exp = exp.Or(LinqExtensions.AssemblePropToExpression<T>(parameter, p.Name, keyword));
                        }
                    }
                    else
                    {
                        if (p.PropertyType == type)
                        {
                            exp = exp.Or(LinqExtensions.AssemblePropToExpression<T>(parameter, p.Name, keyword));
                        }
                    }
                });
                return exp;
            }
            catch(Exception ex)
            {
                return exp;
            }
        }
    }
}
