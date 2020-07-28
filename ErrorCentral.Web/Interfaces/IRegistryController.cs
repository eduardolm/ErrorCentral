using ErrorCentral.Domain.Models;
using ErrorCentral.Web.Controllers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ErrorCentral.Web.Interfaces
{
    public interface IRegistryController : IGenericController<User>
    {
        IActionResult CreateNewUser([FromBody] User user);
    }
}