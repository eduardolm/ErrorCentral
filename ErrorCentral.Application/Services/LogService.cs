using System.Collections.Generic;
using System.Linq;
using ErrorCentral.Application.Interfaces;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Interfaces;
using FluentValidation;

namespace ErrorCentral.Application.Services
{
    public class LogService : GenericService<Log>, ILogService
    {
        private readonly MainContext _context;
        public LogService(IGenericRepository<Log> repository, IValidator<Log> validator, MainContext context) : base(repository, validator)
        {
            _context = context;
        }

        public IEnumerable<Log> GetByEnvironmentId(int id)
        {
            var result = (from l in _repository.GetAll()
                where l.EnvironmentId == id
                select l).ToList();

            return result.Select(item => GetFullLog(item.Id)).ToList();
        }

        public Log GetFullLog(int id)
        {
            var payloadBuilder = new LogBuilder(_context);
            var payload = payloadBuilder.GetPayload(_repository.GetById(id));
            
            return payload;
        }

        public IEnumerable<Log> GetAllFull()
        {
            var result = (from l in _repository.GetAll().ToList()
                select l);
            
            return result.Select(item => GetFullLog(item.Id)).ToList();
        }

        public IEnumerable<Log> GetByEnvironmentIdAndLevel(int environmentId, int levelId)
        {
            var result = (from l in _repository.GetAll().ToList()
                where l.EnvironmentId == environmentId
                where l.LevelId == levelId
                select l);

            return result.Select(item => GetFullLog(item.Id)).ToList();
        }

        public IEnumerable<Log> GetByEnvironmentAndLayer(int environmentId, int layerId)
        {
            var result = (from l in _repository.GetAll().ToList()
                where l.EnvironmentId == environmentId
                where l.LayerId == layerId
                select l);

            return result.Select(item => GetFullLog(item.Id)).ToList();
        }

        public IEnumerable<Log> GetByEnvironmentAndDescription(int environmentId, string description)
        {
            var result = (from l in _repository.GetAll().ToList()
                where l.EnvironmentId == environmentId
                where l.Description.ToLower().Contains(description.ToLower())
                select l);

            return result.Select(item => GetFullLog(item.Id)).ToList();
        }
    }
}