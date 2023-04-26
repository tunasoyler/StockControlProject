using StockControlProject.Entities.Entities;
using StockControlProject.Repository.Abstract;
using StockControlProject.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Service.Concrete
{
    public class GenericService<T> : IGenericService<T> where T : BaseEntity
    {
        private readonly IGenericRepository<T> repository;

        public GenericService(IGenericRepository<T> _repository)
        {
            repository = _repository;
        }

        public bool Activate(int id)
        {
            if (id == 0 || GetById(id) == null)
                return false;
            else
                return repository.Activate(id);
        }

        public bool Add(T entity)
        {
            return repository.Add(entity);
        }

        public bool Add(List<T> entities)
        {
            return repository.Add(entities);
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            return repository.Any(exp);
        }

        public List<T> GetActive()
        {
            return repository.GetActive();
        }

        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            return repository.GetActive(includes);
        }

        public List<T> GetAll()
        {
            return repository.GetAll();
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return repository.GetAll(includes);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes)
        {
            return repository.GetAll(exp, includes);
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            return repository.GetByDefault(exp);
        }

        public T GetById(int id)
        {
            return repository.GetById(id);
        }

        public IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            return repository.GetById(id, includes);
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            return repository.GetDefault(exp);
        }

        public bool Remove(T entity)
        {
            if (entity == null)
                return false;
            else
                return repository.Remove(entity);
        }

        public bool Remove(int id)
        {
            if (id <= 0)
                return false;
            else
                return repository.Remove(id);


        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
