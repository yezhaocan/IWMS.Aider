using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IWMS.Aider.Service
{
    public class UnicodeB2BImp : IUnicodeB2BService
    {

        private readonly IWMSDbContext _dbContext;
        public UnicodeB2BImp(IWMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public TEntity GetById<TEntity>(object key) where TEntity : class
        {
            return _dbContext.Find<TEntity>(key);
        }
        public bool Update<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _dbContext.Update(entity);
                _dbContext.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                return true;
            }
        }
        public bool Insert<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _dbContext.Add(entity);
                _dbContext.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                return true;
            }
        }
        public bool InsertRange<TEntity>(List<TEntity> list) where TEntity : class
        {
            try
            {
                _dbContext.AddRange(list);
                _dbContext.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                return true;
            }
        }
        public bool Delete<TEntity>(object id) where TEntity : class
        {
            try
            {
                _dbContext.Remove(_dbContext.Find<TEntity>(id));
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return true;
            }
        }
        public DbSet<TEntity> Get<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }
    }
}
