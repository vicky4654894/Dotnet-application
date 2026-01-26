using System;
using Application_1.DataAccess.Data;
using Application_1.DataAccess.Repository.IRepository;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Application_1.DataAccess.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            int? pageNumber = null,
            int? pageSize = null)
        {
            IQueryable<T> query = dbSet;
            if(filter != null){
                query = query.Where(filter);
            }
           
            if(!String.IsNullOrEmpty(includeProperties)){
                foreach(var prop in includeProperties.Split(
                    new Char[]{','},StringSplitOptions.RemoveEmptyEntries
                ))
                {
                    query=query.Include(prop);
                }
            }

            if(pageNumber.HasValue && pageSize.HasValue){
                int skip = (pageNumber.Value-1)*pageSize.Value;
                query = query.Skip(skip).Take(pageSize.Value);
            }

            return query.ToList();
        }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter,string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if(!String.IsNullOrEmpty(includeProperties)){
                foreach(var prop in includeProperties.Split(
                    new Char[]{','},StringSplitOptions.RemoveEmptyEntries
                ))
                {
                    query=query.Include(prop);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
        


    public int Count(Expression<Func<T,bool>> ? filter){
        IQueryable<T> query = dbSet;

        if(filter != null){
            query = query.Where(filter);
        }

        return query.Count();

    }

    }
}