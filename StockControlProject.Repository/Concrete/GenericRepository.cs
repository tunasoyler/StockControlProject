using Microsoft.EntityFrameworkCore;
using StockControlProject.Entities.Entities;
using StockControlProject.Repository.Abstract;
using StockControlProject.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StockControlProject.Repository.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StockControlContext db;

        public GenericRepository(StockControlContext _db)
        {
            db = _db;
        }

        public bool Activate(int id)
        {
            T item = GetById(id);
            item.State = true;
            return Update(item);
        }

        public bool Add(T entity)
        {
            try
            {
                db.Set<T>().Add(entity);
                return Save() > 0; //1 satır etkileniyorsa true döner
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool Add(List<T> entities)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    foreach (T entity in entities)
                    {
                        db.Set<T>().Add(entity);
                    }

                    ts.Complete(); //tüm işlemler başarılı olduğunda complete metotu çalışacaktır.

                    return Save() > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Any(Expression<Func<T, bool>> exp) => db.Set<T>().Any(exp);

        public List<T> GetActive() => db.Set<T>().Where(x => x.State == true).ToList();

        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = db.Set<T>().Where(x => x.State == true);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public List<T> GetAll() => db.Set<T>().ToList();

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = db.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = db.Set<T>().Where(exp);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public T GetByDefault(Expression<Func<T, bool>> exp) => db.Set<T>().FirstOrDefault(exp);

        public T GetById(int id) => db.Set<T>().Find(id);

        public IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = db.Set<T>().Where(x => x.Id == id);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, includes) => current.Include(includes));
            }
            return query;
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp) => db.Set<T>().Where(exp).ToList();

        public bool Remove(T entity)
        {
            entity.State = false;
            return Update(entity);
        }

        public bool Remove(int id)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    T item = GetById(id);
                    item.State = false;
                    ts.Complete();
                    return Update(item);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    int count = 0;
                    List<T> collection = GetDefault(exp); //verilen şarta uyanlar collection'a atıldı
                    foreach (T item in collection)
                    {
                        item.State = false;
                        bool result = Update(item);
                        if (result) count++;
                    }
                    if (collection.Count() == count)
                    {
                        ts.Complete();
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int Save()
        {
            return db.SaveChanges();
        }

        public bool Update(T entity)
        {
            try
            {
                entity.ModifiedDate = DateTime.Now;
                db.Set<T>().Update(entity);
                return Save() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
