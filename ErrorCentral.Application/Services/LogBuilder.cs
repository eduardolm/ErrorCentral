using System.Linq;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using Environment = ErrorCentral.Domain.Models.Environment;

namespace ErrorCentral.Application.Services
{
    public class LogBuilder
    {
        private readonly MainContext _context;
        public LogBuilder(MainContext context)
        {
            _context = context;
        }
        public Log GetPayload(Log log)
        {
            log.Status = new Status();
            log.Layer = new Layer();
            log.Level = new Level();
            log.Environment = new Environment();
            log.User = new User();

            log.Layer = (from l in _context.Layer.ToList()
                where l.Id == log.LayerId
                select l).FirstOrDefault();
            log.Status = (from s in _context.Status.ToList()
                where s.Id == log.StatusId
                select s).FirstOrDefault();
            log.Level = (from l in _context.Level.ToList()
                where l.Id == log.LevelId
                select l).FirstOrDefault();
            log.Environment = (from e in _context.Environment.ToList()
                where e.Id == log.EnvironmentId
                select e).FirstOrDefault();
            log.User = (from u in _context.User.ToList()
                where u.Id == log.UserId
                select u).FirstOrDefault();

            return log;
        }
    }
}