using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using <%= ProjectName %>.Models;

namespace <%= ProjectName %>.Repositories
{
    public interface IRepository
    {
        IEnumerable<T> Get<T>() where T : Entity;
        IEnumerable<T> Get<T>(int limit, int page) where T : Entity;
        T Get<T>(string id) where T : Entity;
        IEnumerable<T> Get<T>(Expression<Func<T,bool>> expression) where T : Entity;
        T Create<T>(T entity) where T : Entity;
        void Update<T>(T entity) where T : Entity;
        void Remove<T>(string id) where T : Entity;
    }
}