/*  app.config 配置属性
 *  <!--mongdb数据库连接-->
    <add key="mfgdb2" value="mfgdb2|dhs:dhs" />
    <!--mongdb集群列表-->
    <add key="mfghost" value="192.168.180.35:10001|192.168.180.36:10003|192.168.180.37:10003" />
    <!--设置副本集名称-->
    <add key="mfgreplicasetname" value="wssRep" />
    <!--mongdb集群链接超时时间-->
    <add key="mfgtimeout" value="60" />
    <!--mongdb集群最大连接池258-->
    <add key="maxPoolSize" value="258" />
    <!--mongdb集群最小连接池8-->
    <add key="minPoolSize" value="8" />
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Common.DB.MongoDB
{
    /// <summary>
    /// MongoDB数据操作类 http://api.mongodb.org/
    /// </summary>
    public class MongoDbHelper
    {
        /// <summary>获取MongoDB客户端配置
        /// </summary>
        public static MongoConfig Config = GetConfig();

        /// <summary>设置Mongo客户端配置
        /// </summary>
        public static readonly MongoClientSettings Client = GetClientSettings();

        /// <summary>读取配置文件配置信息
        /// </summary>
        /// <returns></returns>
        private static MongoConfig GetConfig()
        {
            MongoConfig config = new MongoConfig();
            var databaseDefault = ConfigurationManager.AppSettings["mfgdb2"];
            config.DataBaseName = databaseDefault.Split('|')[0];
            config.UserName = databaseDefault.Split('|')[1].Split(':')[0];
            config.PassWord = databaseDefault.Split('|')[1].Split(':')[1];
            config.ClusterAddress = ConfigurationManager.AppSettings["mfghost"].Trim();
            config.ClusterName = ConfigurationManager.AppSettings["mfgreplicasetname"].Trim();
            config.ClusterTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["mfgtimeout"].Trim());
            config.ClusterMaxPoolSize = Convert.ToInt32(ConfigurationManager.AppSettings["maxPoolSize"]);
            config.ClusterMinPoolSize = Convert.ToInt32(ConfigurationManager.AppSettings["minPoolSize"]);
            return config;
        }

        /// <summary>获取MongoDB的初始设置
        /// </summary>
        /// <returns></returns>
        public static MongoClientSettings GetClientSettings()
        {
            List<MongoServerAddress> servers = new List<MongoServerAddress>();
            string reg = @"^(?'server'\d{1,}.\d{1,}.\d{1,}.\d{1,}):(?'port'\d{1,})$";
            string[] serverList = Config.ClusterAddress.Split('|');
            foreach (var server in serverList)
            {
                //IP：Port格式判断
                MatchCollection mc = Regex.Matches(server, reg);
                if (mc.Count <= 0) continue;
                var host = mc[0].Groups["server"].ToString();
                var port = int.Parse(mc[0].Groups["port"].ToString());
                var address = new MongoServerAddress(host, port);
                servers.Add(address);
            }
            if (servers.Count < 1)
                return null;
            MongoClientSettings set = new MongoClientSettings();
            set.Servers = servers;
            //set.ReplicaSetName = Config.ClusterName;//设置副本集名称
            set.ConnectTimeout = new TimeSpan(0, 0, 0, Config.ClusterTimeOut, 0);//设置超时时间为5秒
            string userName = Config.UserName;
            string passWord = Config.PassWord;
            var credentials = MongoCredential.CreateMongoCRCredential(Config.DataBaseName, userName, passWord);//身份
            var ssl = new List<MongoCredential>();
            ssl.Add(credentials);
            set.Credentials = ssl;
            set.ReadPreference = new ReadPreference(ReadPreferenceMode.PrimaryPreferred);
            set.MaxConnectionPoolSize = Config.ClusterMaxPoolSize;
            set.MinConnectionPoolSize = Config.ClusterMinPoolSize;
            // set.MaxConnectionLifeTime = new TimeSpan(0, 0, 0, 300, 0);
            return set;
        }

        /// <summary>MongoDB聚合操作
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="pipeline">条件</param>
        /// <returns></returns>
        public static AggregateResult Aggregate(string collectionName, params BsonDocument[] pipeline)
        {
            AggregateResult result;
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            
            //获取数据库或者创建数据库（不存在的话）。
            IMongoDatabase database = server.GetDatabase(Config.DataBaseName);
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.Aggregate(pipeline);
            }
            return result;
        }

        /// <summary>更新并获取更新后的数据
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        /// <param name="query">查询条件</param>
        /// <param name="update">更新内容</param>
        /// <param name="sort">排序</param>
        /// <param name="returnNew">true：返回修改后的文档，false：返回修改前的文档</param>
        /// <returns></returns>
        public static FindAndModifyResult FindAndUpdate(string collectionName, IMongoQuery query, IMongoUpdate update, IMongoSortBy sort, bool returnNew)
        {
            FindAndModifyResult result;
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(Config.DataBaseName);
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.FindAndModify(query, sort, update, returnNew);
            }
            return result;
        }

        #region 新增文档操作
        /// <summary>插入一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">集合名称</param>
        /// <param name="entity">插入实体</param>
        /// <returns></returns>
        public static WriteConcernResult InsertOne<T>(string collectionName, T entity)
        {
            return InsertOne(Config.DataBaseName, collectionName, entity);
        }

        /// <summary>插入一条数据
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="entity">插入实体</param>
        /// <returns></returns>
        private static WriteConcernResult InsertOne<T>(string databaseName, string collectionName, T entity)
        {
            WriteConcernResult result;
            if (null == entity)
                return null;
            var mongo = new MongoClient(Client);
            //获取数据库或者创建数据库（不存在的话）。
            IMongoDatabase database = mongo.GetDatabase(databaseName);
            
            using (server.RequestStart(database)) //开始连接数据库。
            {
                //MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                IMongoCollection<T> myCollection = database.GetCollection<T>(collectionName);
                result = myCollection.InsertOne(entity);
            }
            return result;
        }

        /// <summary>插入一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">集合名称</param>
        /// <param name="entitys">数据集合体</param>
        /// <returns></returns>
        public static IEnumerable<WriteConcernResult> InsertAll<T>(string collectionName, IEnumerable<T> entitys)
        {
            return InsertAll(Config.DataBaseName, collectionName, entitys);
        }

        /// <summary>插入一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="entitys">集合体</param>
        /// <returns></returns>
        private static IEnumerable<WriteConcernResult> InsertAll<T>(string databaseName, string collectionName, IEnumerable<T> entitys)
        {
            IEnumerable<WriteConcernResult> result = null;
            if (null == entitys)
                return null;
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.InsertBatch(entitys);
            }
            return result;
        }

        #endregion

        #region 修改文档操作
        /// <summary>根据条件更新一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">集合名称</param>
        /// <param name="query">查询条件</param>
        /// <param name="update">更新语句</param>
        /// <returns></returns>
        public static WriteConcernResult UpdateOne<T>(string collectionName, IMongoQuery query, IMongoUpdate update)
        {
            return UpdateOne<T>(Config.DataBaseName, collectionName, query, update);
        }

        /// <summary>根据条件更新一条数据
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="collectionName">集合名称</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static WriteConcernResult UpdateOne<T>(string collectionName, T entity)
        {
            return UpdateOne<T>(Config.DataBaseName, collectionName, entity);
        }

        /// <summary>根据条件更新一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="query">查询条件</param>
        /// <param name="update">定义更新文档</param>
        /// <returns></returns>
        private static WriteConcernResult UpdateOne<T>(string databaseName, string collectionName, IMongoQuery query, IMongoUpdate update)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            WriteConcernResult result;
            using (server.RequestStart(database)) //开始连接数据库。
            {
                //获取Users集合
                MongoCollection collection = database.GetCollection(collectionName);
                //执行更新操作
                result = collection.Update(query, update);
            }
            return result;
        }
        /// <summary>根据条件更新一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="query">查询条件</param>
        /// <param name="update">更新语句</param>
        /// <returns></returns>
        public static WriteConcernResult UpdateOne<T>(string collectionName, IMongoQuery query, IMongoUpdate update, UpdateFlags flag)
        {
            return UpdateOne<T>(Config.DataBaseName, collectionName, query, update, flag);
        }

        /// <summary>根据条件更新一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="query">查询条件</param>
        /// <param name="update">定义更新文档</param>
        /// <returns></returns>
        private static WriteConcernResult UpdateOne<T>(string databaseName, string collectionName, IMongoQuery query, IMongoUpdate update, UpdateFlags flag)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            WriteConcernResult result;
            using (server.RequestStart(database)) //开始连接数据库。
            {
                //获取Users集合
                MongoCollection collection = database.GetCollection(collectionName);
                //执行更新操作
                result = collection.Update(query, update, flag);
            }
            return result;
        }

        /// <summary>
        /// 更新实体文档
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        private static WriteConcernResult UpdateOne<T>(string databaseName, string collectionName, T entity)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            WriteConcernResult result;
            using (server.RequestStart(database)) //开始连接数据库。
            {
                //获取Users集合
                MongoCollection collection = database.GetCollection(collectionName);
                //执行更新操作
                result = collection.Save(entity);
            }
            return result;
        }

        /// <summary>同时修改多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒") 或者 Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title","感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="update">更新设置。调用示例：Update.Set("Title", "yanc") 或者Update.Set("Title", "yanc").Set("Author", "yanc2") 等等</param>
        /// <returns></returns>
        public static WriteConcernResult UpdateAll<T>(string collectionName, IMongoQuery query, IMongoUpdate update)
        {
            return UpdateAll<T>(Config.DataBaseName, collectionName, query, update);
        }

        /// <summary>同时修改多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒") 或者Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title","感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="update">更新设置。调用示例：Update.Set("Title", "yanc") 或者Update.Set("Title", "yanc").Set("Author","yanc2")等等</param>
        /// <returns></returns>
        public static WriteConcernResult UpdateAll<T>(string databaseName, string collectionName, IMongoQuery query, IMongoUpdate update)
        {
            WriteConcernResult result;
            if (null == query || null == update)
            {
                return null;
            }
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.Update(query, update, UpdateFlags.Multi);
            }
            return result;
        }

        #endregion

        #region 删除文档操作
        /// <summary>删除一个实体
        /// </summary>
        /// <param name="collectionName">表名</param>
        /// <param name="id">删除实体的ID</param>
        /// <returns></returns>
        public static WriteConcernResult Delete(string collectionName, string id)
        {
            return Delete(Config.DataBaseName, collectionName, id);
        }

        /// <summary>删除一个实体
        /// </summary>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="_id">删除实体的ID</param>
        /// <returns></returns>
        public static WriteConcernResult Delete(string databaseName, string collectionName, string _id)
        {
            WriteConcernResult result;
            ObjectId id;
            if (!ObjectId.TryParse(_id, out id))
            {
                return null;
            }
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.Remove(Query.EQ("_id", id));
            }
            return result;
        }

        /// <summary>
        /// 按条件删除一个实体
        /// </summary>
        /// <param name="collectionName">表名</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static WriteConcernResult Delete(string collectionName, IMongoQuery query)
        {
            return Delete(Config.DataBaseName, collectionName, query);
        }

        /// <summary>
        /// 按条件删除一个实体
        /// </summary>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static WriteConcernResult Delete(string databaseName, string collectionName, IMongoQuery query)
        {
            WriteConcernResult result;
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.Remove(query);
            }
            return result;
        }

        /// <summary>删除整张表的数据
        /// </summary>
        /// <param name="collectionName">表名</param>
        /// <returns></returns>
        public static WriteConcernResult DeleteAll(string collectionName)
        {
            return DeleteAll(Config.DataBaseName, collectionName, null);
        }

        /// <summary>条件删除
        /// </summary>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public static WriteConcernResult DeleteAll(string collectionName, IMongoQuery query)
        {
            return DeleteAll(Config.DataBaseName, collectionName, query);
        }

        /// <summary>条件删除
        /// </summary>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title","感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public static WriteConcernResult DeleteAll(string databaseName, string collectionName, IMongoQuery query)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            WriteConcernResult result;
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                if (null == query)
                {
                    result = myCollection.RemoveAll();
                }
                else
                {
                    result = myCollection.Remove(query);
                }
            }
            return result;
        }

        #endregion

        #region 获取单个文档
        /// <summary>获取单条信息
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="_id">id</param>
        /// <returns></returns>
        public static T GetOne<T>(string collectionName, string _id)
        {
            return GetOne<T>(Config.DataBaseName, collectionName, _id);
        }
        /// <summary>获取单条信息
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="databaseName">数据名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="_id">id</param>
        /// <returns></returns>
        public static T GetOne<T>(string databaseName, string collectionName, string _id)
        {
            T result = default(T);
            ObjectId id;
            if (!ObjectId.TryParse(_id, out id))
            {
                return default(T);
            }
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.FindOneAs<T>(Query.EQ("_id", id));
            }
            return result;
        }

        /// <summary>根据条件查询出一个实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc"))等等</param>
        /// <returns></returns>
        public static T GetOne<T>(string collectionName, IMongoQuery query)
        {
            return GetOne<T>(Config.DataBaseName, collectionName, query);
        }

        /// <summary>根据条件查询出一个实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title","感冒"),Query.EQ("Author", "yanc"))等等</param>
        /// <returns></returns>
        public static T GetOne<T>(string databaseName, string collectionName, IMongoQuery query)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            T result = default(T);
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                if (null == query)
                {
                    result = myCollection.FindOneAs<T>();
                }
                else
                {
                    result = myCollection.FindOneAs<T>(query);
                }
            }
            return result;
        }

        #endregion

        #region 获取多个文档
        /// <summary>获得表中的所有数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="collectionName">表名</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName)
        {
            return GetAll<T>(Config.DataBaseName, collectionName);
        }

        /// <summary>如果不清楚具体的数量，一般不要用这个函数。
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string databaseName, string collectionName)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            var result = new List<T>();
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                foreach (T entity in myCollection.FindAllAs<T>())
                {
                    result.Add(entity);
                }
            }
            return result;
        }
        /// <summary>获得表中的一定数量的数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="count">获得数量</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, int count)
        {
            return GetAll<T>(collectionName, count, null, null);
        }

        /// <summary>根据条件，获得表中的一定数量的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="count">个数</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒")或者Query.EQ("Title", "感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, int count, IMongoQuery query)
        {
            return GetAll<T>(collectionName, count, query, null);
        }

        /// <summary>根据条件，获得表中的一定数量的数据(排名前后)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="count">个数</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒")或者Query.EQ("Title", "感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, int count, IMongoQuery query, int type)
        {
            return GetAll<T>(collectionName, count, query, null, type);
        }

        /// <summary>获得表中的一定数量的数据，并进行排序
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="count">个数</param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, int count, IMongoSortBy sortBy)
        {
            return GetAll<T>(collectionName, count, null, sortBy);
        }

        /// <summary>根据条件，获得表中的一定数量的数据，并进行排序。（只返回所需要的字段的数据）
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="count">个数</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="fields">只返回所需要的字段的数据。调用示例："Title" 或者 new string[]{ "Title", "Author" }等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, int count, IMongoQuery query, IMongoSortBy sortBy,
                                        params string[] fields)
        {
            var pagerInfo = new PagerInfo();
            pagerInfo.Page = 1;
            pagerInfo.PageSize = count;
            return GetAll<T>(Config.DataBaseName, collectionName, query, pagerInfo, sortBy, fields);
        }

        /// <summary>根据条件，获得表中的一定数量的数据，并进行排序。（只返回所需要的字段的数据）（前后排名的）
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="count">个数</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, int count, IMongoQuery query, IMongoSortBy sortBy, int type)
        {
            var pagerInfo = new PagerInfo();
            pagerInfo.Page = 1;
            pagerInfo.PageSize = count;
            return GetAll<T>(Config.DataBaseName, collectionName, query, pagerInfo, sortBy, type);
        }

        /// <summary>根据条件返回一个表中的数据，并进行分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pagerInfo"></param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, IMongoQuery query, PagerInfo pagerInfo)
        {
            return GetAll<T>(Config.DataBaseName, collectionName, query, pagerInfo, null);
        }

        /// <summary>根据条件返回一个表中的数据，并进行排序分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒")或者Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc"))等等</param>
        /// <param name="pagerInfo"></param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, IMongoQuery query, PagerInfo pagerInfo, IMongoSortBy sortBy)
        {
            return GetAll<T>(Config.DataBaseName, collectionName, query, pagerInfo, sortBy);
        }

        /// <summary>根据条件返回一个表中的数据，并进行分页。（只返回所需要的字段的数据）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title","感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pagerInfo"></param>
        /// <param name="fields">只返回所需要的字段的数据。调用示例："Title" 或者 new string[]{ "Title","Author"}等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, IMongoQuery query, PagerInfo pagerInfo, params string[] fields)
        {
            return GetAll<T>(Config.DataBaseName, collectionName, query, pagerInfo, null, fields);
        }

        /// <summary>根据条件返回一个表中的数据，并进行排序分页。（只返回所需要的字段的数据）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title","感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pagerInfo"></param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="fields">只返回所需要的字段的数据。调用示例："Title"或者new string[]{ "Title", "Author" }等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, IMongoQuery query, PagerInfo pagerInfo, IMongoSortBy sortBy, params string[] fields)
        {
            return GetAll<T>(Config.DataBaseName, collectionName, query, pagerInfo, sortBy, fields);
        }

        /// <summary>根据条件返回一个表中的数据，并进行排序分页。（只返回所需要的字段的数据）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pagerInfo">分页信息</param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="fields">只返回所需要的字段的数据。调用示例："Title" 或者 new string[]{ "Title", "Author" }等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string databaseName, string collectionName, IMongoQuery query, PagerInfo pagerInfo, IMongoSortBy sortBy, params string[] fields)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            var result = new List<T>();
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                MongoCursor<T> myCursor;
                if (null == query)
                {
                    myCursor = myCollection.FindAllAs<T>();
                }
                else
                {
                    myCursor = myCollection.FindAs<T>(query);
                }
                if (null != sortBy)
                {
                    myCursor.SetSortOrder(sortBy);
                }
                if (null != fields)
                {
                    myCursor.SetFields(fields);
                }
                foreach (
                    T entity in myCursor.SetSkip((pagerInfo.Page - 1) * pagerInfo.PageSize).SetLimit(pagerInfo.PageSize))
                //.SetSkip(100).SetLimit(10)是指读取第一百条后的10条数据。
                {
                    result.Add(entity);
                }
                int length = Convert.ToInt32(myCursor.Count((a) => { return true; }));

            }
            return result;
        }

        /// <summary>根据条件返回一个表中的数据，并进行排序分页。（只返回所需要的字段的数据）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>   
        /// <param name="pagerInfo">分页信息</param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="type">前后（0,前；1,后）</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string databaseName, string collectionName, IMongoQuery query, PagerInfo pagerInfo, IMongoSortBy sortBy, int type)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            var result = new List<T>();
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                MongoCursor<T> myCursor;
                if (null == query)
                {
                    myCursor = myCollection.FindAllAs<T>();
                }
                else
                {
                    myCursor = myCollection.FindAs<T>(query);
                }
                if (null != sortBy)
                {
                    myCursor.SetSortOrder(sortBy);
                }

                if (type == 0)
                {
                    int length = Convert.ToInt32(myCursor.Clone(typeof(T)).Count());
                    if (length >= pagerInfo.PageSize)
                    {
                        foreach (
                      T entity in myCursor.SetSkip(length - pagerInfo.PageSize).SetLimit(pagerInfo.PageSize))
                        //.SetSkip(100).SetLimit(10)是指读取第一百条后的10条数据。
                        {
                            result.Add(entity);
                        }
                    }
                    else
                    {
                        foreach (
                    T entity in myCursor.SetSkip(0).SetLimit(length))
                        //.SetSkip(100).SetLimit(10)是指读取第一百条后的10条数据。
                        {
                            result.Add(entity);
                        }
                    }

                }
                else
                {
                    int length = Convert.ToInt32(myCursor.Clone(typeof(T)).Count());

                    if (length >= pagerInfo.PageSize)
                    {
                        foreach (
                            T entity in myCursor.SetLimit(pagerInfo.PageSize))
                        //.SetSkip(100).SetLimit(10)是指读取第一百条后的10条数据。
                        {
                            result.Add(entity);
                        }
                    }
                    else
                    {
                        foreach (
                          T entity in myCursor.SetSkip(0).SetLimit(length))
                        //.SetSkip(100).SetLimit(10)是指读取第一百条后的10条数据。
                        {
                            result.Add(entity);
                        }
                    }
                }

            }
            return result;
        }

        /// <summary>排序返回数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, IMongoQuery query, IMongoSortBy sortBy)
        {
            return GetAll<T>(Config.DataBaseName, collectionName, query, sortBy);
        }

        /// <summary>排序返回数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string databaseName, string collectionName, IMongoQuery query, IMongoSortBy sortBy)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            var result = new List<T>();
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                MongoCursor<T> myCursor;
                if (null == query)
                {
                    myCursor = myCollection.FindAllAs<T>();
                }
                else
                {
                    myCursor = myCollection.FindAs<T>(query);
                }
                if (null != sortBy)
                {
                    myCursor.SetSortOrder(sortBy);
                }
                foreach (T entity in myCursor)
                {
                    result.Add(entity);
                }
            }
            return result;
        }

        /// <summary>排序返回数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, IMongoQuery query, IMongoSortBy sortBy, params string[] filds)
        {
            return GetAll<T>(Config.DataBaseName, collectionName, query, sortBy, filds);
        }

        /// <summary>排序返回数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string databaseName, string collectionName, IMongoQuery query, IMongoSortBy sortBy, params string[] filds)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            var result = new List<T>();
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                MongoCursor<T> myCursor;
                if (null == query)
                {
                    myCursor = myCollection.FindAllAs<T>();
                }
                else
                {
                    myCursor = myCollection.FindAs<T>(query);
                }
                if (null != sortBy)
                {
                    myCursor.SetSortOrder(sortBy);
                }
                if (null != filds)
                {
                    myCursor.SetFields(filds);
                }
                foreach (T entity in myCursor)
                {
                    result.Add(entity);
                }
            }
            return result;
        }

        #endregion

        #region 根据字段过滤数据，并进行排序得到数据
        /// <summary>
        /// 根据字段过滤数据，并进行排序得到数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="fields">过滤字段</param>
        /// <returns></returns>
        public static List<T> GetAllDis<T>(string collectionName, IMongoQuery query, IMongoSortBy sortBy, params string[] fields)
        {
            return GetAllDis<T>(Config.DataBaseName, collectionName, query, sortBy, fields);
        }

        /// <summary>根据字段过滤数据，并进行排序得到数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">表名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title","感冒")或者Query.EQ("Title","感冒")或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title")或者SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="fields">过滤字段</param>
        /// <returns></returns>
        public static List<T> GetAllDis<T>(string databaseName, string collectionName, IMongoQuery query, IMongoSortBy sortBy, params string[] fields)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            var result = new List<T>();
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                MongoCursor<T> myCursor;
                if (null == query)
                {
                    myCursor = myCollection.FindAllAs<T>();
                }
                else
                {
                    myCursor = myCollection.FindAs<T>(query);
                }
                if (null != sortBy)
                {
                    myCursor.SetSortOrder(sortBy);
                }
                if (null != fields)
                {
                    myCursor.SetFields(fields).Distinct();
                }
                foreach (T entity in myCursor)
                {
                    result.Add(entity);
                }
            }
            return result;
        }
        #endregion

        #region 获取满足条件记录数

        /// <summary>/// 获取集合调试
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="collectionName">集合名</param>
        /// <param name="query">query</param>
        /// <returns></returns>
        public static long GetCount(string collectionName, IMongoQuery query)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(Config.DataBaseName);

            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);

                return myCollection.Count(query);

            }

        }

        #endregion

        #region 索引（暂时不用）
        //public static void CreateIndex(string collectionName, params string[] keyNames)
        //{
        //    CreateIndex(Server, MongoDBHelper.database_Default, collectionName, keyNames);
        //}
        //public static void CreateIndex(MongoServerSettings mongoServerSettings, string databaseName, string collectionName, params string[] keyNames)
        //{
        //    WriteConcernResult result = new WriteConcernResult();
        //    if (null == keyNames)
        //    {
        //        return;
        //    }
        //    MongoServer server = MongoServer.Create(mongoServerSettings);
        //    //获取数据库或者创建数据库（不存在的话）。
        //    MongoDatabase database = server.GetDatabase(databaseName);
        //    using (server.RequestStart(database))//开始连接数据库。
        //    {
        //        MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
        //        if (!myCollection.IndexExists(keyNames))
        //        {
        //            myCollection.EnsureIndex(keyNames);
        //        }
        //    }
        //}
        #endregion

        #region 删除集合操作

        /// <summary>删除表集合
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="collectionName"></param>
        public static void Drop(string databaseName, string collectionName)
        {
            var mongo = new MongoClient(Client);
            MongoServer server = mongo.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);
            using (server.RequestStart(database)) //开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                myCollection.Drop();
            }
        }

        /// <summary>
        /// 删除某个集合
        /// </summary>
        /// <param name="collectionName">集合名称</param>
        public static void DropCollection(string collectionName)
        {
            Drop(Config.DataBaseName, collectionName);
        }
        #endregion
    }
}
