using System.Collections.Generic;
using ErrorCentral.Domain.Models;

namespace ErrorCentral.Application.Interfaces
{
    public interface ILogService : IGenericService<Log>
    {
        IEnumerable<Log> GetByEnvironmentId(int id);
        Log GetFullLog(int id);
        IEnumerable<Log> GetAllFull();
        IEnumerable<Log> GetByEnvironmentIdAndLevel(int environmentId, int levelId);
        IEnumerable<Log> GetByEnvironmentAndLayer(int environmentId, int layerId);
        IEnumerable<Log> GetByEnvironmentAndDescription(int environmentId, string description);
    }
}