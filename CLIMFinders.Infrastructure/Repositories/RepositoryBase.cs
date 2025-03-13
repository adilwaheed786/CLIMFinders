
using CLIMFinders.Application.Interfaces;
using CLIMFinders.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions; 

namespace CLIMFinders.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> table;
        public RepositoryBase(ApplicationDbContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }
        public IEnumerable<T> GetAllNoTracking()
        {
            return table.AsNoTracking().ToList();
        }
        public IEnumerable<T> GetAllFiltered(Expression<Func<T, bool>> expression)
        {
            var a = table.Where(expression).ToList();
            return a;
        }
        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return table.FirstOrDefault(expression);
        }
        public T GetById(object id)
        {
            return table.Find(id);
        }
        public T GetByInclude(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;

            IQueryable<T> dbQuery = table;

            //Apply eager loading
            foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<T, object>(navigationProperty);

            item = dbQuery
                .AsNoTracking() //Don't track any changes for the selected item
                .FirstOrDefault(where); //Apply where clause

            return item;
        }
        public IList<T> GetAllInclude(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;

            IQueryable<T> dbQuery = table;

            //Apply eager loading
            foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<T, object>(navigationProperty);

            list = dbQuery
                .AsNoTracking()
                .ToList<T>();

            return list;
        }

        public T Insert(T obj)
        {
            table.Add(obj);
            return obj;
        }
         
        public List<T> InsertRange(List<T> obj)
        {
            table.AddRange(obj);
            return obj;
        }
        public T Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            return obj;
        }
        public T UpdateRelated(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Detached;
            return obj;
        }
        public List<T> UpdateRange(List<T> obj)
        {
            obj.ToList().ForEach(e =>
            {
                _context.Entry(e).State = EntityState.Modified;
            });
            return obj;
        }
        public void Delete(object id)
        { 
            T existing = table.Find(id);
            table.Remove(existing);

        }
        public void DeleteRange(Expression<Func<T, bool>> expression)
        {
            var list = table.Where(expression).ToList();
            table.RemoveRange(list);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
