using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Utils
{
    public static class DataExtensions
    {
        
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
            Expression<Func<T, bool>> exp = ExpressionExtention.Create<T>();
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
                            exp = exp.Or(ExpressionExtention.AssemblePropToExpression<T>(parameter, p.Name, keyword));
                        }
                    }
                    else
                    {
                        if (p.PropertyType == type)
                        {
                            exp = exp.Or(ExpressionExtention.AssemblePropToExpression<T>(parameter, p.Name, keyword));
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
