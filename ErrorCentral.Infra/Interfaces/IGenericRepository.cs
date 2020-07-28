using System;
using System.Collections.Generic;
using ErrorCentral.Domain.Interfaces;

namespace ErrorCentral.Infra.Interfaces
{
    public interface IGenericRepository<T> : IDisposable where T : class, IBaseEntity
    {
        IEnumerable<T> GetAll();

        T GetById(int id);
        
        void Create(T entity);
        
        void Update(T entity);
        
        void Delete(int id);
    }
}