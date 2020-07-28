using ErrorCentral.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ErrorCentral.Web.Controllers.Interfaces
{
    public interface ILogController : IGenericController<Log>
    {
        IActionResult GetByEnvironmentId(int id);
    }
}