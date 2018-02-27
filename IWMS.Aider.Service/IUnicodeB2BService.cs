using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IWMS.Aider.Service
{
   public  interface IUnicodeB2BService
    {
        TEntity GetById<TEntity>(object key) where TEntity : class;
        bool Insert<TEntity>(TEntity entity) where TEntity : class;
        bool InsertRange<TEntity>(List<TEntity> entity) where TEntity : class;
        bool Update<TEntity>(TEntity entity) where TEntity : class;
        bool Delete<TEntity>(object key) where TEntity : class;
        DbSet<TEntity> Get<TEntity>() where TEntity : class;
    }
}
