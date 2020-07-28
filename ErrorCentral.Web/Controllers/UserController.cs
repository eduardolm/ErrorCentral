using System;
using System.Collections.Generic;
using AutoMapper;
using ErrorCentral.Application.Interfaces;
using ErrorCentral.Domain.DTOs;
using ErrorCentral.Domain.Models;
using ErrorCentral.Web.Controllers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErrorCentral.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : GenericController<User>, IUserController
    {
        public UserController(IGenericService<User> service, IMapper mapper) : base(service, mapper)
        {
        }
        
        // POST: user/cadastro
        [HttpPost("cadastro")]
        [AllowAnonymous]
        public IActionResult CreateNewUser([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest();
            user.CreatedAt = DateTime.Now;
            
            var result = _service.Create(user);
            if (result != null) return Ok(_mapper.Map<User, UserDTO>(result));
            return BadRequest("Dados inválidos. Verifique os dados digitados. Caso estejam corretos, seu nome " +
                              "e/ou e-mail já estão cadastrados em nosso sistema. Neste caso, entre em contato com o " +
                              "administrador");
        }

        // GET: user/<id>
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_mapper.Map<User, UserDTO>(_service.GetById(id)));
        }

        // GET: user
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(_service.GetAll()));
        }
        
        // POST: user
        [HttpPost("create")]
        public new IActionResult Create([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest();
            user.CreatedAt = DateTime.Now;

            var result = _service.Create(user);
            if (result != null) return Ok(_mapper.Map<User, UserDTO>(result));
            return BadRequest();
        }
    }
}