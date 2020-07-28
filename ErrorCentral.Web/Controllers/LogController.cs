using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Castle.Core.Internal;
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
    public class LogController : GenericController<Log>, ILogController
    {
        private readonly ILogService _service;
        public LogController(ILogService service, IMapper mapper) : base(service, mapper)
        {
            _service = service;
        }
        
        // GET: log/environment/<id>
        [HttpGet("environment/{id:int}")]
        public IActionResult GetByEnvironmentId(int id)
        {
            var result = _service.GetByEnvironmentId(id);
            if (!result.Any()) return NoContent();
            return Ok(_mapper.Map<IEnumerable<Log>, IEnumerable<LogDTO>>(result));
        }

        // GET: log/<id>
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_mapper.Map<Log, LogDTO>(_service.GetFullLog(id)));
        }
        
        // GET: log
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<Log>, IEnumerable<LogDTO>>(_service.GetAllFull()));
        }

        // POST: log
        [HttpPost("create")]
        public IActionResult CreateLog([FromBody] Log log)
        {
            if (!ModelState.IsValid) return BadRequest("Verifique os dados digitados.");
            log.CreatedAt = DateTime.Now;
            return Ok(_mapper.Map<Log, LogDTO>(_service.Create(log)));
        }
        
        // GET: log/environment/<environmentId>/level/<levelId>
        [HttpGet("environment/{environmentId:int}/level/{levelId:int}")]
        public IActionResult GetByEnvironmentIdAndLevel(int environmentId, int levelId)
        {
            var result = _mapper.Map<IEnumerable<Log>, IEnumerable<LogDTO>>(
                _service.GetByEnvironmentIdAndLevel(environmentId, levelId));
            if (result.Any()) return Ok(result);
            return NoContent();
        }

        //GET: log/environment/<environemntId>/layer/<layerId>
        [HttpGet("environment/{environmentId:int}/layer/{layerId:int}")]
        public IActionResult GetByEnvironmentAndLayer(int environmentId, int layerId)
        {
            var result = _mapper.Map<IEnumerable<Log>, IEnumerable<LogDTO>>(
                _service.GetByEnvironmentAndLayer(environmentId, layerId));
            if (result.Any()) return Ok(result);
            return NoContent();
        }

        // GET: log/environment?environmentId=<id>&description=<description>
        [HttpGet("environment")]
        public IActionResult GetByEnvironmentAndDescription([FromQuery] int environmentId, string description)
        {
            if (!description.IsNullOrEmpty())
            {
                var result =
                    _mapper.Map<IEnumerable<Log>, IEnumerable<LogDTO>>(
                        _service.GetByEnvironmentAndDescription(environmentId, description.ToLower()));
                if (result.Any()) return Ok(result);
                return NoContent();
            }

            return NoContent();
        }
    }
}