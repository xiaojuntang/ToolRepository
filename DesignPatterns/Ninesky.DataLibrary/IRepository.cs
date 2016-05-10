using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ninesky.DataLibrary
{
    public interface IRepository<T> where T : class
    {


        //查找实体
        #region Find
        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="ID">实体主键值</param>
        /// <returns></returns>
        T Find(int ID);


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="where">查询Lambda表达式</param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> where);

        #endregion

        //查找实体列表
        #region FindList
        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <returns></returns>
        IQueryable<T> FindList();

        IQueryable<T> FindList(QueryParameter parameter);

        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="TKey">排序建类型</typeparam>
        /// <param name="order">排序键</param>
        /// <param name="asc">是否正序</param>
        /// <returns></returns>
        IQueryable<T> FindList<TKey>(Expression<Func<T, TKey>> order, bool asc);


        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="TKey">排序键类型</typeparam>
        /// <param name="order">排序键</param>
        /// <param name="asc">是否正序</param>
        /// <param name="number">获取的记录数量</param>
        /// <returns></returns>
        IQueryable<T> FindList<TKey>(Expression<Func<T, TKey>> order, bool asc, int number);


        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <param name="where">查询Lambda表达式</param>
        /// <returns></returns>
        IQueryable<T> FindList(Expression<Func<T, bool>> where);


        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <param name="where">查询Lambda表达式</param>
        /// <param name="number">获取的记录数量</param>
        /// <returns></returns>
        IQueryable<T> FindList(Expression<Func<T, bool>> where, int number);


        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="TKey">排序键类型</typeparam>
        /// <param name="where">查询Lambda表达式</param>
        /// <param name="order">排序键</param>
        /// <param name="asc">是否正序</param>
        /// <returns></returns>
        IQueryable<T> FindList<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, bool asc);


        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <typeparam name="TKey">排序键类型</typeparam>
        /// <param name="where">查询Lambda表达式</param>
        /// <param name="order">排序键</param>
        /// <param name="asc">是否正序</param>
        /// <param name="number">获取的记录数量</param>
        /// <returns></returns>
        IQueryable<T> FindList<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, bool asc, int number);

        #endregion

        #region FindPageList 查找实体分页列表

        /// <summary>
        /// 查找分页列表
        /// </summary>
        /// <param name="pageSize">每页记录数。必须大于1</param>
        /// <param name="pageIndex">页码。首页从1开始，页码必须大于1</param>
        /// <param name="totalNumber">总记录数</param>
        /// <returns></returns>
        IQueryable<T> FindPageList(int pageSize, int pageIndex, out int totalNumber);

        /// <summary>
        /// 查找分页列表
        /// </summary>
        /// <param name="pageSize">每页记录数。必须大于1</param>
        /// <param name="pageIndex">页码。首页从1开始，页码必须大于1</param>
        /// <param name="totalNumber">总记录数</param>
        /// <param name="order">排序键</param>
        /// <param name="asc">是否正序</param>
        /// <returns></returns>
        IQueryable<T> FindPageList<TKey>(int pageSize, int pageIndex, out int totalNumber, Expression<Func<T, TKey>> order, bool asc);

        /// <summary>
        /// 查找分页列表
        /// </summary>
        /// <param name="pageSize">每页记录数。必须大于1</param>
        /// <param name="pageIndex">页码。首页从1开始，页码必须大于1</param>
        /// <param name="totalNumber">总记录数</param>
        /// <param name="where">查询表达式</param>
        IQueryable<T> FindPageList(int pageSize, int pageIndex, out int totalNumber, Expression<Func<T, bool>> where);

        /// <summary>
        /// 查找分页列表
        /// </summary>
        /// <param name="pageSize">每页记录数。必须大于1</param>
        /// <param name="pageIndex">页码。首页从1开始，页码必须大于1</param>
        /// <param name="totalNumber">总记录数</param>
        /// <param name="where">查询表达式</param>
        /// <param name="order">排序键</param>
        /// <param name="asc">是否正序</param>
        IQueryable<T> FindPageList<TKey>(int pageSize, int pageIndex, out int totalNumber, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, bool asc);

        #endregion

        #region 通用操作方法

        /// <summary>
        /// 添加实体【立即保存】
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响的对象的数目</returns>
        int Add(T entity);

        /// <summary>
        /// 更新实体【立即保存】
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响的对象的数目</returns>
        int Update(T entity);

        /// <summary>
        /// 删除实体【立即保存】
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响的对象的数目</returns>
        int Delete(T entity);

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>受影响的对象的数目</returns>
        int Delete(IEnumerable<T> entities);

        /// <summary>
        /// 记录数
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// 记录数
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 记录是否存在
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <returns></returns>
        bool IsContains(Expression<Func<T, bool>> predicate);

        #endregion
    }
}
