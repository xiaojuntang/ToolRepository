using Climb.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public interface IDALBase<T>
    {
        #region 查询接口

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        T FindSingle(Query query);
        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<T> FindList(Query query);
        /// <summary>
        /// 查询实体列表（分页）
        /// </summary>
        /// <param name="query"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<T> FindList(Query query, int index, int count);
        /// <summary>
        /// 查询所有数据实体
        /// </summary>
        /// <returns></returns>
        List<T> FindListAll(string orderBy);
        /// <summary>
        /// 查询数据个数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        int FindCount(Query query);
        /// <summary>
        /// 获取前多少笔数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        List<T> FindList(Query query, int take);

        #endregion

        #region 更新接口

        /// <summary>
        /// 更新数据信息
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        int Update(Update update);
        int Save(T t);
        #endregion
    }
}
