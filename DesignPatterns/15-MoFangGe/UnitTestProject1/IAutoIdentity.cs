using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public partial interface IAutoIdentity<T> 
        //where T : IAutoIdentity
    {
        /// <summary>
        /// 自增标识列名
        /// </summary>
        string AutoIdentityName
        {
            get;
        }

        /// <summary>
        /// 获取实现
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetInfo(int id);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>1：success 0:filed</returns>
        int Delete(int id);

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="dic">更新字段添加到的字典</param>
        /// <param name="id">自增ID</param>
        /// <returns>1成功  0、-1失败</returns>
        int Update(Dictionary<string, object> dic, int id);

        /// <summary>
        /// 保存实体信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="id">非自增ID</param>
        /// <returns>1成功  0、-1失败</returns>
        int Save(T t, int id);
    }

    public interface IAutoIdentity
    {
        /// <summary>
        /// 获取 自动增长标识
        /// </summary>
        [Browsable(false)]
        long AutoIdentity
        {
            get;
            set;
        }
    }
}
