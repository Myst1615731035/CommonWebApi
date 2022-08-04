using Common.Model.ApiModel;
using Common.Utils;
using Common.Utils.DBHelper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repository
{
    public class BaseRepository<TEntity>: IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly SqlSugarScope _dbBase = DBHelper.db;

        #region SqlSugar实例
        public ISqlSugarClient Db
        {
            get
            {
                return _db;
            }
        }

        protected ISqlSugarClient _db
        {
            get
            {
                /* 如果要开启多库支持，
                 * 1、在appsettings.json 中开启MutiDBEnabled节点为true，必填
                 * 2、设置一个主连接的数据库ID，节点MainDB，对应的连接字符串的Enabled也必须true，必填
                 */
                if (DBConfig.MutilDb)
                {
                    if (typeof(TEntity).GetTypeInfo().GetCustomAttributes(typeof(SugarTable), true).FirstOrDefault((x => x.GetType() == typeof(SugarTable))) is SugarTable sugarTable && !string.IsNullOrEmpty(sugarTable.TableDescription))
                    {
                        _dbBase.ChangeDatabase(sugarTable.TableDescription.ToLower());
                    }
                    else
                    {
                        _dbBase.ChangeDatabase(DBConfig.MainDbConfigId);
                    }
                }
                return _dbBase;
            }
        }
        #endregion

        #region 单表方法
        #region 查
        public async Task<TEntity> QueryById(object objId)
        {
            var query = Db.Queryable<TEntity>().In(objId);
            return await query.SingleAsync();
        }
        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// 作　　者:CommonApi
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public async Task<TEntity> QueryByIdWithCache(object objId, bool blnUseCache = false)
        {
            var query = Db.Queryable<TEntity>().WithCacheIF(blnUseCache).In(objId);
            return await query.SingleAsync();
        }

        /// <summary>
        /// 功能描述:根据ID查询数据
        /// 作　　者:CommonApi
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
        {
            //return await Task.Run(() => Db.Queryable<TEntity>().In(lstIds).ToList());
            return await Db.Queryable<TEntity>().In(lstIds).ToListAsync();
        }
        #endregion

        #region 增
        /// <summary>
        /// (自增)写入实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<int> Add(TEntity entity)
        {
            return await Db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        /// <summary>
        /// (雪花)写入实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<long> AddLong(TEntity entity)
        {
            return await Db.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public async Task<TEntity> AddEntity(TEntity entity)
        {
            return await Db.Insertable(entity).ExecuteReturnEntityAsync();
        }

        /// <summary>
        /// 写入实体数据(写入部分列)
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>返回自增量列</returns>
        public async Task<int> Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            var insert = Db.Insertable(entity);
            if (insertColumns == null)
            {
                return await insert.ExecuteReturnIdentityAsync();
            }
            else
            {
                return await insert.InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
            }
        }

        /// <summary>
        /// 批量插入实体(速度快)
        /// </summary>
        /// <param name="list">实体集合</param>
        /// <returns>影响行数</returns>
        public async Task<int> Add(List<TEntity> list)
        {
            if (list.IsNotEmptyOrNull() && list.Count > 0)
            {
                return await Db.Insertable(list).ExecuteCommandAsync();
            }
            return 0;
        }

        /// <summary>
        /// 批量插入实体(雪花)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<List<long>> AddLong(List<TEntity> list)
        {
            if (list.IsNotEmptyOrNull() && list.Count > 0)
            {
                return await Db.Insertable(list).ExecuteReturnSnowflakeIdListAsync();
            }
            return new List<long>();
        }
        #endregion

        #region 改
        /// <summary>
        /// 更新实体数据 以主键为条件
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity)
        {
            return await Db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> Update(TEntity entity, string strWhere)
        {
            return await Db.Updateable(entity).Where(strWhere).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> Update(string strSql, SugarParameter[] parameters = null)
        {
            return await Db.Ado.ExecuteCommandAsync(strSql, parameters) > 0;
        }

        public async Task<bool> Update(object operateAnonymousObjects)
        {
            return await Db.Updateable<TEntity>(operateAnonymousObjects).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
        {
            IUpdateable<TEntity> up = Db.Updateable(entity);
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            {
                up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
            }
            if (lstColumns != null && lstColumns.Count > 0)
            {
                up = up.UpdateColumns(lstColumns.ToArray());
            }
            if (!string.IsNullOrEmpty(strWhere))
            {
                up = up.Where(strWhere);
            }
            return await up.ExecuteCommandHasChangeAsync();
        }
        #endregion

        #region 删
        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Delete(TEntity entity)
        {
            return await Db.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除实体集合
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Delete(List<TEntity> list)
        {
            return await Db.Deleteable(list).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteById(object id)
        {
            //var i = await Task.Run(() => Db.Deleteable<TEntity>(id).ExecuteCommand());
            //return i > 0;
            return await Db.Deleteable<TEntity>(id).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIds(object[] ids)
        {
            //var i = await Task.Run(() => Db.Deleteable<TEntity>().In(ids).ExecuteCommand());
            //return i > 0;
            return await Db.Deleteable<TEntity>().In(ids).ExecuteCommandHasChangeAsync();
        }
        #endregion

        #endregion

        #region 高级查询
        /// <summary>
        /// 功能描述:查询所有数据
        /// 作　　者:CommonApi
        /// </summary>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query()
        {
            var query = Db.Queryable<TEntity>();
            return await query.ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:CommonApi
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere)
        {
            //return await Task.Run(() => Db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
            return await Db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:CommonApi
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await Db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:按照特定列查询数据列表
        /// 作　　者:CommonApi
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            return await Db.Queryable<TEntity>().Select(expression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:按照特定列查询数据列表带条件排序
        /// 作　　者:CommonApi
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="whereExpression">过滤条件</param>
        /// <param name="expression">查询实体条件</param>
        /// <param name="strOrderByFileds">排序条件</param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Select(expression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:CommonApi
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await Db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(strOrderByFileds != null, strOrderByFileds).ToListAsync();
        }
        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await Db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:CommonApi
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
        {
            return await Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:CommonApi
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds)
        {
            return await Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:CommonApi
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds)
        {
            //return await Task.Run(() => Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToList());
            return await Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToListAsync();
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="strSql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>泛型集合</returns>
        public async Task<List<TEntity>> QuerySql(string strSql, SugarParameter[] parameters = null)
        {
            return await Db.Ado.SqlQueryAsync<TEntity>(strSql, parameters);
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="strSql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public async Task<DataTable> QueryTable(string strSql, SugarParameter[] parameters = null)
        {
            return await Db.Ado.GetDataTableAsync(strSql, parameters);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// 作　　者:CommonApi
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, string strOrderByFileds)
        {
            return await Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToPageListAsync(intPageIndex, intPageSize);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// 作　　者:CommonApi
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds)
        {
            return await Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToPageListAsync(intPageIndex, intPageSize);
        }

        /// <summary>
        /// 分页查询[使用版本，其他分页未测试]
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns></returns>
        public async Task<Pagination> QueryPage(Expression<Func<TEntity, bool>> expression, Pagination pageModel)
        {
            RefAsync<int> totalCount = 0;
            var list = Db.Queryable<TEntity>()
             .OrderByIF(!string.IsNullOrEmpty(pageModel.sort), pageModel.sort)
             .WhereIF(expression != null, expression);

            if (pageModel.isAll)
            {
                var data = await list.ToListAsync();
                pageModel.response = data;
                pageModel.total = data.Count;
                pageModel.pageCount = 1;
            }
            else
            {
                pageModel.response = await list.ToPageListAsync(pageModel.currentPage, pageModel.pageSize, totalCount);
                int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / pageModel.pageSize.ObjToDecimal())).ObjToInt();
                pageModel.total = totalCount;
                pageModel.pageCount = pageCount;
            }
            return pageModel;
        }


        /// <summary> 
        ///查询-多表查询
        /// </summary> 
        /// <typeparam name="T">实体1</typeparam> 
        /// <typeparam name="T2">实体2</typeparam> 
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param> 
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param> 
        /// <returns>值</returns>
        public async Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return await Db.Queryable(joinExpression).Select(selectExpression).ToListAsync();
            }
            return await Db.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).ToListAsync();
        }
        #endregion

        #region 分表方法
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public async Task<Pagination> QueryPageSplit(Expression<Func<TEntity, bool>> expression, Pagination page, string split)
        {
            await CreateSplitTable(typeof(TEntity), split);

            RefAsync<int> totalCount = 0;
            var list = Db.Queryable<TEntity>()
             .OrderByIF(!string.IsNullOrEmpty(page.sort), page.sort)
             .WhereIF(expression != null, expression)
             .SplitTable(tab => tab.ContainsTableNames(split));

            if (page.isAll)
            {
                var data = await list.ToListAsync();
                page.response = data;
                page.total = data.Count;
                page.pageCount = 1;
            }
            else
            {
                page.response = await list.ToPageListAsync(page.currentPage, page.pageSize, totalCount);
                int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / page.pageSize.ObjToDecimal())).ObjToInt();
                page.total = totalCount;
                page.pageCount = pageCount;
            }
            return page;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public async Task<TEntity> QuerySplit(Expression<Func<TEntity, bool>> expression, string split)
        {
            await CreateSplitTable(typeof(TEntity), split);

            return await Db.Queryable<TEntity>()
                .WhereIF(expression != null, expression)
                .SplitTable(tab => tab.ContainsTableNames(split))
                .FirstAsync();
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryListSplit(Expression<Func<TEntity, bool>> expression, string split)
        {
            await CreateSplitTable(typeof(TEntity), split);

            return await Db.Queryable<TEntity>()
                .WhereIF(expression != null, expression)
                .SplitTable(tab => tab.ContainsTableNames(split))
                .ToListAsync();
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSplit(TEntity entity, string split)
        {
            return (await Db.Updateable(entity).SplitTable(tab => tab.ContainsTableNames(split)).ExecuteCommandAsync()) > 0;
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSplit(Expression<Func<TEntity, bool>> expression, string split)
        {
            return (await Db.Deleteable<TEntity>().Where(expression).SplitTable(tab => tab.ContainsTableNames(split)).ExecuteCommandAsync()) > 0;
        }

        public async Task CreateSplitTable(Type type, string split)
        {
            var splitTableName = $"{type.Name}{split}";
            if (!Db.DbMaintenance.IsAnyTable(splitTableName) && DBConfig.SplitTables.Contains(type.Name))
            {
                Db.MappingTables.Add(type.Name, splitTableName);
                Db.CodeFirst.InitTables(type);
            }
        }
        #endregion
    }
}
