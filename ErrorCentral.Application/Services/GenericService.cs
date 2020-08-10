using System.Collections.Generic;
using Castle.Core.Internal;
using ErrorCentral.Application.Interfaces;
using ErrorCentral.Domain.Interfaces;
using ErrorCentral.Infra.Interfaces;
using FluentValidation;

namespace ErrorCentral.Application.Services
{
    public class GenericService<T> : IGenericService<T> where T : class, IBaseEntity
    {
        protected readonly IGenericRepository<T> _repository;
        protected readonly IValidator<T> _validator;

        public GenericService(IGenericRepository<T> repository, IValidator<T> validator)
        {
            _repository = repository;
            _validator = validator;
        }
        
        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetById(int id)
        {
            if (_repository.ToString().IsNullOrEmpty() || id < 0)
                return null;
            return _repository.GetById(id);
        }

        public T Create(T entity)
        {
            var result = _validator.Validate(entity);

            if (result.IsValid)
            {
                _repository.Create(entity);
                return _repository.GetById(entity.Id);
            }

            return null;
        }

        public T Update(T entity)
        {
            _repository.Update(entity);
            return _repository.GetById(entity.Id);
        }

        public IEnumerable<T> Delete(int id)
        {
            if (id.ToString().IsNullOrEmpty() || id < 0) return null;
            _repository.Delete(id);
            return _repository.GetAll();
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}