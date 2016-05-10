using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Climb.Core
{
    /// <summary>
    /// 所有实体的基类
    /// </summary>
    [Serializable]
    public abstract class BaseEntity : ICloneable, IDisposable
    {
        public int  f_id { get; set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseEntity()
        {

        }

      

        #region 方法
         
        /// <summary>
        /// 判断两个实体是否是同一数据记录的实体
        /// </summary>
        /// <param name="obj">要比较的实体信息</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            BaseEntity entity = obj as BaseEntity;
            return entity != null && f_id.Equals(entity.f_id);
        }

        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns>
        /// 当前 <see cref="T:System.Object"/> 的哈希代码。
        /// </returns>
        public override int GetHashCode()
        {
            return f_id.GetHashCode();
        }

        /// <summary>
        /// 创建当前业务实体对象的一个副本。
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(buffer, this);
                buffer.Position = 0;
                BaseEntity obj = (BaseEntity)formatter.Deserialize(buffer);
                return obj;
            }
        }

        /// <summary>
        /// 通过属性名称获取该属性值。
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns></returns>
        public object GetPropertyValue(string name)
        {
            PropertyInfo pi = GetType().GetProperty(name);
            return pi.GetValue(this, null);
        }

        /// <summary>
        /// 通过属性名称设置该属性值。
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">要设置的值</param>
        public void SetPropertyValue(string name, object value)
        {
            PropertyInfo pi = this.GetType().GetProperty(name);
            pi.SetValue(this, value, null);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(true);
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="disposing">是否释放资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //do something..
            }
        }

        #endregion

    }
}
