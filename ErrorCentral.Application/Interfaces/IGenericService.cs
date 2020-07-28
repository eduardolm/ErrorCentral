using System;
using System.Collections.Generic;
using ErrorCentral.Domain.Interfaces;

namespace ErrorCentral.Application.Interfaces
{
    public interface IGenericService<T> : IDisposable where T : class, IBaseEntity
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        T Create(T entity);

        T Update(T entity);

        IEnumerable<T> Delete(int id);
    }
}