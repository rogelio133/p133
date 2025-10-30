using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    namespace DataAccess.Repository
    {
        public class Repository<T> : IRepository<T> where T : class
        {
            private readonly ApplicationDbContext _db;
            internal DbSet<T> dbSet;
            public Repository(ApplicationDbContext db)
            {
                _db = db;
                this.dbSet = _db.Set<T>();
            }
            public void Add(T entity)
            {
                dbSet.Add(entity);
            }

            public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
            {
                IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

                query = query.Where(filter);
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return query.FirstOrDefault();


            }

            public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
            {
                IQueryable<T> query = dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }


                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }

                return query.ToList();
                //return dbSet.ToList();
            }

            public void Remove(T entity)
            {
                dbSet.Remove(entity);
            }

            public void RemoveRange(IEnumerable<T> entities)
            {
                dbSet.RemoveRange(entities);
            }

            public PagedResult<T> GetPaged(int pageNumber,int pageSize,Expression<Func<T, bool>>? filter = null,
                string? includeProperties = null,
                Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
            {
                IQueryable<T> query = dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                var result = new PagedResult<T>
                {
                    TotalRecords = query.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Items = query
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList()
                };

                return result;
            }
        }
    }
}
