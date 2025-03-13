using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CLIMFinders.Application.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllNoTracking();
        IEnumerable<T> GetAllFiltered(Expression<Func<T, bool>> expression);
        T GetByInclude(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        IList<T> GetAllInclude(params Expression<Func<T, object>>[] navigationProperties);
        T GetById(object id);
        T FirstOrDefault(Expression<Func<T, bool>> expression);
        T Insert(T obj);
        List<T> InsertRange(List<T> obj);
        T Update(T obj);
        T UpdateRelated(T obj);
        List<T> UpdateRange(List<T> obj);
        void Delete(object id);
        void Save();
        void DeleteRange(Expression<Func<T, bool>> expression);
    }
}
