using Repo.Demo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo.Demo.Dal
{
    public class Class1
    {
    }

    /// <summary>
    /// 基础的数据操作规范
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 添加实体并提交到数据服务器
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        void Insert(TEntity item);

        /// <summary>
        /// 移除实体并提交到数据服务器
        /// 如果表存在约束，需要先删除子表信息
        /// </summary>
        /// <param name="item">Item to delete</param>
        void Delete(TEntity item);

        /// <summary>
        /// 修改实体并提交到数据服务器
        /// </summary>
        /// <param name="item"></param>
        void Update(TEntity item);

        /// <summary>
        /// 得到指定的实体集合（延时结果集）
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <returns>List of selected elements</returns>
        IQueryable<TEntity> GetModel();

        /// <summary>
        /// 根据主键得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Find(params object[] id);
    }

    public abstract class Repository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="entity"></param>
        public abstract bool Insert(TEntity entity);

        /// <summary>
        /// 批量新增对象
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public abstract bool Insert(List<TEntity> entitys);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity"></param>
        public abstract bool Delete(TEntity entity);

        /// <summary>
        /// 通过ID删除对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public abstract bool Delete(int Id);

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity"></param>
        public abstract bool Update(TEntity entity);

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract TEntity GetModel(TEntity entity);

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract List<TEntity> GetModels(TEntity entity);
    }


    public class UserDao : Repository<UserInfo>
    {
        public override bool Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(UserInfo entity)
        {
            throw new NotImplementedException();
        }

        public override UserInfo GetModel(UserInfo entity)
        {
            throw new NotImplementedException();
        }

        public override List<UserInfo> GetModels(UserInfo entity)
        {
            throw new NotImplementedException();
        }

        public override bool Insert(List<UserInfo> entitys)
        {
            throw new NotImplementedException();
        }

        public override bool Insert(UserInfo entity)
        {
            throw new NotImplementedException();
        }

        public override bool Update(UserInfo entity)
        {
            throw new NotImplementedException();
        }
    }
}
