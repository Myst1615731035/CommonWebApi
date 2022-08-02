using Common.Model.ApiModel;
using SqlSugar;
using System.Data;
using System.Linq.Expressions;

namespace Common.IService.Base
{
    public interface IBaseService<TEntity> where TEntity : class,new ()
    {
        #region 单表方法
        #region 查
        Task<TEntity> QueryById(object objId);
        Task<TEntity> QueryById(object objId, bool blnUseCache = false);
        Task<List<TEntity>> QueryByIDs(object[] lstIds);
        #endregion

        #region 增
        Task<int> Add(TEntity model);
        Task<TEntity> AddEntity(TEntity model);

        Task<int> Add(List<TEntity> listEntity);
        #endregion

        #region 删
        Task<bool> DeleteById(object id);

        Task<bool> Delete(TEntity model);

        Task<bool> Delete(List<TEntity> list);

        Task<bool> DeleteByIds(object[] ids);
        #endregion

        #region 改
        Task<bool> Update(TEntity model);
        Task<bool> Update(TEntity entity, string strWhere);

        Task<bool> Update(object operateAnonymousObjects);

        Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "");
        #endregion
        #endregion 

        #region 高级查询
        Task<List<TEntity>> Query();
        Task<List<TEntity>> Query(string strWhere);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression);
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);
        Task<List<TEntity>> Query(string strWhere, string strOrderByFileds);
        Task<List<TEntity>> QuerySql(string strSql, SugarParameter[] parameters = null);
        Task<DataTable> QueryTable(string strSql, SugarParameter[] parameters = null);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds);
        Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, string strOrderByFileds);
        Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds);
        Task<Pagination> QueryPage(Expression<Func<TEntity, bool>> expression, Pagination page);
        Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();
        #endregion

        #region 分表方法
        Task<Pagination> QueryPageSplit(Expression<Func<TEntity, bool>> expression, Pagination page, string split);
        Task<TEntity> QuerySplit(Expression<Func<TEntity, bool>> expression, string split);
        Task<List<TEntity>> QueryListSplit(Expression<Func<TEntity, bool>> expression, string split);
        Task<bool> UpdateSplit(TEntity entity, string split);
        Task<bool> DeleteSplit(Expression<Func<TEntity, bool>> expression, string split);
        #endregion
    }
}
