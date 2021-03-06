﻿using ErrorCentral.Domain.Interfaces;
using ErrorCentral.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ErrorCentral.Web.Controllers.Interfaces
{
    public interface IGenericController<T> where T : class, IBaseEntity
    {
        IActionResult Create([FromBody] T entity);
        IActionResult Update([FromBody] T entity);
        IActionResult Delete(int id);

    }
}