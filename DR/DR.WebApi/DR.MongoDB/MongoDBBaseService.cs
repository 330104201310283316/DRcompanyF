using System;
using System.Collections.Generic;
using System.Text;
using DR.Models;
using MongoDB.Driver;


namespace DR.MongoDB
{
    public class MongoDBBaseService<T> where T : ModelBase
    {
        private readonly IMongoCollection<T> _collection;   //数据表操作对象

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        public MongoDBBaseService(string tableName)
        {
            //MongoClient client = new MongoClient(Configs.Configs.ConnectionStrings.MongoDB);    //获取链接字符串
            MongoClient client = new MongoClient("mongodb://127.0.0.1");    //获取链接字符串

            IMongoDatabase database = client.GetDatabase("DR_Test");   //数据库名 （不存在自动创建）

            //获取对特定数据表集合中的数据的访问
            _collection = database.GetCollection<T>(tableName);// （不存在自动创建）
        }

        //Find<T> – 返回集合中与提供的搜索条件匹配的所有文档。
        //InsertOne – 插入提供的对象作为集合中的新文档。
        //ReplaceOne – 将与提供的搜索条件匹配的单个文档替换为提供的对象。
        //DeleteOne – 删除与提供的搜索条件匹配的单个文档。

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public List<T> Get()
        {
            return _collection.Find(T => true).ToList();
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field">条件查询</param>
        /// <returns></returns>
        public T Get(long id)
        {
            return _collection.Find<T>(T => T.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="Ts">要添加进db中的实体</param>
        /// <returns></returns>
        public T[] Create(params T[] Ts)
        {
            _collection.InsertMany(Ts);
            return Ts;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id">需要更新的对象的id</param>
        /// <param name="TIn">需要更新的对象的实体,实体id不建议更改</param>
        public void Update(long id, T TIn)
        {
            _collection.ReplaceOne(T => T.Id == id, TIn);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="TIns">需要删除的对象的实体</param>
        public void Remove(params T[] TIns)
        {
            foreach (var TIn in TIns)
            {
                _collection.DeleteOne(T => T.Id == TIn.Id);
            }
        }

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="ids">需要删除的对象的id</param>
        public void Remove(params long[] ids)
        {
            foreach (var id in ids)
            {
                _collection.DeleteOne(T => T.Id == id);
            }
        }
    }
}
