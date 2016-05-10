using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DB.MongoDB
{
    /// <summary>
    /// MongoBD配置参数类
    /// </summary>
    public class MongoConfig
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseName { get; set; }
        /// <summary>
        /// 数据库用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 数据库密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// MongoDB集群地址
        /// 例 192.168.180.35:10001|192.168.180.36:10003|192.168.180.37:10003
        /// </summary>
        public string ClusterAddress { get; set; }
        /// <summary>
        /// 副本集名称
        /// </summary>
        public string ClusterName { get; set; }
        /// <summary>
        /// 集群链接超时时间
        /// </summary>
        public int ClusterTimeOut { get; set; }
        /// <summary>
        /// 集群最大连接池
        /// </summary>
        public int ClusterMaxPoolSize { get; set; }
        /// <summary>
        /// 集群最小连接池
        /// </summary>
        public int ClusterMinPoolSize { get; set; }
    }
}
