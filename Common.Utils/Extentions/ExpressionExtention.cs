using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public static class ExpressionExtention
    {
        //
        // 摘要:
        //     组合两个表达式
        //
        // 参数:
        //   expression:
        //     表达式1
        //
        //   extendExpression:
        //     表达式2
        //
        //   mergeWay:
        //     组合方式
        //
        // 类型参数:
        //   TSource:
        //     泛型类型
        //
        // 返回结果:
        //     新的表达式
        public static Expression<TSource> Compose<TSource>(this Expression<TSource> expression, Expression<TSource> extendExpression, Func<Expression, Expression, Expression> mergeWay)
        {
            Expression arg = ParameterReplaceExpressionVisitor.ReplaceParameters(expression.Parameters.Select((u, i) => new
            {
                u,
                Parameter = extendExpression.Parameters[i]
            }).ToDictionary(d => d.Parameter, d => d.u), extendExpression.Body);
            return Expression.Lambda<TSource>(mergeWay(expression.Body, arg), expression.Parameters);
        }

        //
        // 摘要:
        //     与操作合并两个表达式
        //
        // 参数:
        //   expression:
        //     表达式1
        //
        //   extendExpression:
        //     表达式2
        //
        // 类型参数:
        //   TSource:
        //     泛型类型
        //
        // 返回结果:
        //     新的表达式
        public static Expression<Func<TSource, bool>> And<TSource>(this Expression<Func<TSource, bool>> expression, Expression<Func<TSource, bool>> extendExpression)
        {
            return expression.Compose(extendExpression, Expression.AndAlso);
        }

        //
        // 摘要:
        //     与操作合并两个表达式，支持索引器
        //
        // 参数:
        //   expression:
        //     表达式1
        //
        //   extendExpression:
        //     表达式2
        //
        // 类型参数:
        //   TSource:
        //     泛型类型
        //
        // 返回结果:
        //     新的表达式
        public static Expression<Func<TSource, int, bool>> And<TSource>(this Expression<Func<TSource, int, bool>> expression, Expression<Func<TSource, int, bool>> extendExpression)
        {
            return expression.Compose(extendExpression, Expression.AndAlso);
        }

        //
        // 摘要:
        //     根据条件成立再与操作合并两个表达式
        //
        // 参数:
        //   expression:
        //     表达式1
        //
        //   condition:
        //     布尔条件
        //
        //   extendExpression:
        //     表达式2
        //
        // 类型参数:
        //   TSource:
        //     泛型类型
        //
        // 返回结果:
        //     新的表达式
        public static Expression<Func<TSource, bool>> AndIf<TSource>(this Expression<Func<TSource, bool>> expression, bool condition, Expression<Func<TSource, bool>> extendExpression)
        {
            if (!condition)
            {
                return expression;
            }

            return expression.Compose(extendExpression, Expression.AndAlso);
        }

        //
        // 摘要:
        //     根据条件成立再与操作合并两个表达式，支持索引器
        //
        // 参数:
        //   expression:
        //     表达式1
        //
        //   condition:
        //     布尔条件
        //
        //   extendExpression:
        //     表达式2
        //
        // 类型参数:
        //   TSource:
        //     泛型类型
        //
        // 返回结果:
        //     新的表达式
        public static Expression<Func<TSource, int, bool>> AndIf<TSource>(this Expression<Func<TSource, int, bool>> expression, bool condition, Expression<Func<TSource, int, bool>> extendExpression)
        {
            if (!condition)
            {
                return expression;
            }

            return expression.Compose(extendExpression, Expression.AndAlso);
        }

        //
        // 摘要:
        //     或操作合并两个表达式
        //
        // 参数:
        //   expression:
        //     表达式1
        //
        //   extendExpression:
        //     表达式2
        //
        // 类型参数:
        //   TSource:
        //     泛型类型
        //
        // 返回结果:
        //     新的表达式
        public static Expression<Func<TSource, bool>> Or<TSource>(this Expression<Func<TSource, bool>> expression, Expression<Func<TSource, bool>> extendExpression)
        {
            return expression.Compose(extendExpression, Expression.OrElse);
        }

        //
        // 摘要:
        //     或操作合并两个表达式，支持索引器
        //
        // 参数:
        //   expression:
        //     表达式1
        //
        //   extendExpression:
        //     表达式2
        //
        // 类型参数:
        //   TSource:
        //     泛型类型
        //
        // 返回结果:
        //     新的表达式
        public static Expression<Func<TSource, int, bool>> Or<TSource>(this Expression<Func<TSource, int, bool>> expression, Expression<Func<TSource, int, bool>> extendExpression)
        {
            return expression.Compose(extendExpression, Expression.OrElse);
        }

        //
        // 摘要:
        //     根据条件成立再或操作合并两个表达式
        //
        // 参数:
        //   expression:
        //     表达式1
        //
        //   condition:
        //     布尔条件
        //
        //   extendExpression:
        //     表达式2
        //
        // 类型参数:
        //   TSource:
        //     泛型类型
        //
        // 返回结果:
        //     新的表达式
        public static Expression<Func<TSource, bool>> OrIf<TSource>(this Expression<Func<TSource, bool>> expression, bool condition, Expression<Func<TSource, bool>> extendExpression)
        {
            if (!condition)
            {
                return expression;
            }

            return expression.Compose(extendExpression, Expression.OrElse);
        }

        //
        // 摘要:
        //     根据条件成立再或操作合并两个表达式，支持索引器
        //
        // 参数:
        //   expression:
        //     表达式1
        //
        //   condition:
        //     布尔条件
        //
        //   extendExpression:
        //     表达式2
        //
        // 类型参数:
        //   TSource:
        //     泛型类型
        //
        // 返回结果:
        //     新的表达式
        public static Expression<Func<TSource, int, bool>> OrIf<TSource>(this Expression<Func<TSource, int, bool>> expression, bool condition, Expression<Func<TSource, int, bool>> extendExpression)
        {
            if (!condition)
            {
                return expression;
            }

            return expression.Compose(extendExpression, Expression.OrElse);
        }

        //
        // 摘要:
        //     获取Lambda表达式属性名，只限 u=>u.Property 表达式
        //
        // 参数:
        //   expression:
        //     表达式
        //
        // 类型参数:
        //   TSource:
        //     泛型类型
        //
        // 返回结果:
        //     属性名
        public static string GetExpressionPropertyName<TSource>(this Expression<Func<TSource, object>> expression)
        {
            UnaryExpression unaryExpression = expression.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                return ((MemberExpression)unaryExpression.Operand).Member.Name;
            }

            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return memberExpression.Member.Name;
            }

            ParameterExpression parameterExpression = expression.Body as ParameterExpression;
            if (parameterExpression != null)
            {
                return parameterExpression.Type.Name;
            }

            throw new InvalidCastException("expression");
        }

        //
        // 摘要:
        //     是否是空集合
        //
        // 参数:
        //   sources:
        //     集合对象
        //
        // 类型参数:
        //   TSource:
        //     泛型类型
        //
        // 返回结果:
        //     是否为空集合
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> sources)
        {
            if (sources != null)
            {
                return !sources.Any();
            }

            return true;
        }

        /// <summary>
        /// 拼装参数形成Expression实现对实体类的全文搜索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> AssemblePropToExpression<T>(ParameterExpression parameter, string propertyName, object propertyValue)
        {
            //构造属性表达式：t.column
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            //构造常量表达式：keyword
            ConstantExpression constant = Expression.Constant(propertyValue.ToString(), typeof(string));

            //构造属性调用的方法
            MethodInfo ToString = member.Type.GetMethod("ToString", new Type[] { });
            MethodInfo Contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            //转换该属性的类型为string
            MethodCallExpression methodCall = Expression.Call(member, ToString);
            //构造包含方法的表达式
            methodCall = Expression.Call(methodCall, Contains, constant);
            //生成当次的表达式
            var where = Expression.Lambda<Func<T, bool>>(methodCall, parameter);

            return where;
        }

        /// <summary>
        /// 根据类创建Expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Create<T>()
        {
            return param => true;
        }

        //
        // 摘要:
        //     处理 Lambda 参数不一致问题
        internal sealed class ParameterReplaceExpressionVisitor : ExpressionVisitor
        {
            //
            // 摘要:
            //     参数表达式映射集合
            private readonly Dictionary<ParameterExpression, ParameterExpression> parameterExpressionSetter;

            //
            // 摘要:
            //     构造函数
            //
            // 参数:
            //   parameterExpressionSetter:
            //     参数表达式映射集合
            public ParameterReplaceExpressionVisitor(Dictionary<ParameterExpression, ParameterExpression> parameterExpressionSetter)
            {
                this.parameterExpressionSetter = parameterExpressionSetter ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            //
            // 摘要:
            //     替换表达式参数
            //
            // 参数:
            //   parameterExpressionSetter:
            //     参数表达式映射集合
            //
            //   expression:
            //     表达式
            //
            // 返回结果:
            //     新的表达式
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> parameterExpressionSetter, Expression expression)
            {
                return new ParameterReplaceExpressionVisitor(parameterExpressionSetter).Visit(expression);
            }

            //
            // 摘要:
            //     重写基类参数访问器
            //
            // 参数:
            //   parameterExpression:
            protected override Expression VisitParameter(ParameterExpression parameterExpression)
            {
                if (parameterExpressionSetter.TryGetValue(parameterExpression, out ParameterExpression value))
                {
                    parameterExpression = value;
                }

                return base.VisitParameter(parameterExpression);
            }
        }
    }
}
