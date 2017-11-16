using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using <%= ProjectName %>.Models;
using System.Linq;
using MongoDB.Bson;

namespace <%= ProjectName %>.Repositories
{
    public class MongoGeneric: IRepository
    {
        IMongoDatabase _db;

        public MongoGeneric(string mongoUrl, string dataBase)
        {
            _db = new MongoClient(mongoUrl).GetDatabase(dataBase);
            ExecuteClassMap();
        }

        private void ExecuteClassMap()
        {
            BsonClassMap.RegisterClassMap<Entity>(cm =>  
            {
                cm.AutoMap();
                cm.MapMember(m => m.Id).SetIdGenerator(new StringObjectIdGenerator());
            });
        }

        private string CollectionName<T>() where T : Entity
        {
            return $"{typeof(T).ToString().Split('.').Last()}s";
        } 

        public IEnumerable<T> Get<T>() where T : Entity
        {
            return _db.GetCollection<T>(CollectionName<T>()).Find(new BsonDocument()).ToEnumerable();
        }

        public T Get<T>(string id) where T : Entity
        {
            return _db.GetCollection<T>(CollectionName<T>()).Find(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<T> Get<T>(Expression<Func<T,bool>> expression) where T : Entity
        {
            return _db.GetCollection<T>(CollectionName<T>()).Find(expression).ToEnumerable();
        }

        public T Create<T>(T entity) where T : Entity
        {
            _db.GetCollection<T>(CollectionName<T>()).InsertOne(entity);
            return entity;
        }

        public void Update<T>(T entity) where T : Entity
        {
            _db.GetCollection<T>(CollectionName<T>()).FindOneAndReplace(x => x.Id == entity.Id, entity);
        }

        public void Remove<T>(string id) where T : Entity
        {
            _db.GetCollection<T>(CollectionName<T>()).FindOneAndDelete(x => x.Id == id);
        }
    }
}