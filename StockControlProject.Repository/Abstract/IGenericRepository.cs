﻿using StockControlProject.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Repository.Abstract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        bool Add(T entity);
        bool Add(List<T> entities);
        bool Update(T entity);
        bool Remove(T entity);
        bool Remove(int id);
        bool RemoveAll(Expression<Func<T, bool>> exp);
        T GetById(int id);
        IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes);
        T GetByDefault(Expression<Func<T, bool>> exp);
        List<T> GetAll();
        List<T> GetActive();
        List<T> GetDefault(Expression<Func<T, bool>> exp);
        IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAll(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes);
        bool Activate(int id);
        bool Any(Expression<Func<T, bool>> exp);
        int Save();
    }
}
