using AutoMapper;
using Castle.Core.Internal;
using ErrorCentral.Application.Interfaces;
using ErrorCentral.Domain.Interfaces;
using ErrorCentral.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ErrorCentral.Web.Controllers
{
    public class GenericController<T> : ControllerBase, IGenericController<T> where T : class, IBaseEntity
    {
        protected readonly IGenericService<T> _service;
        protected readonly IMapper _mapper;

        public GenericController(IGenericService<T> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        
        // POST: <controller>
        [HttpPost]
        public IActionResult Create([FromBody] T entity)
        {
            if (ModelState.IsValid)
            {
                var result = _service.Create(entity);
                if (result != null) return Ok(_service.Create(entity));
                return BadRequest("Dados inválidos. Favor verificar os dados digitados.");
            }

            return BadRequest("Dados inválidos. Favor verificar os dados digitados.");
        }

        // PUT: <controller>
        [HttpPut]
        public IActionResult Update([FromBody] T entity)
        {
            if (ModelState.IsValid)
            {
                return Ok(_service.Update(entity));
            }

            return BadRequest("Dados inválidos. Favor verificar os dados digitados.");        }

        // DELETE: <controller>/<id>
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id.ToString().IsNullOrEmpty() || id < 0) return BadRequest();
            _service.Delete(id);
            return Ok("Item removido com sucesso.");
        }
    }
}