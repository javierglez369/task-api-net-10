using Api.Models;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Controller para operaciones CRUD sobre tareas.
/// Ruta base: /api/tasks
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;

    // Inyección de dependencias: ASP.NET Core entrega el repositorio automáticamente.
    public TasksController(ITaskRepository repository)
    {
        _repository = repository;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GET /api/tasks
    // Retorna todas las tareas. Opcionalmente filtra por estado de completado.
    // ─────────────────────────────────────────────────────────────────────────
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskItem>), StatusCodes.Status200OK)]
    public IActionResult GetAll([FromQuery] bool? isCompleted = null)
    {
        var tasks = _repository.GetAll();

        if (isCompleted.HasValue)
            tasks = tasks.Where(t => t.IsCompleted == isCompleted.Value);

        return Ok(tasks);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GET /api/tasks/{id}
    // Retorna una tarea por su ID. 404 si no existe.
    // ─────────────────────────────────────────────────────────────────────────
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var task = _repository.GetById(id);

        return task is not null
            ? Ok(task)
            : NotFound(new { message = $"Tarea con ID {id} no encontrada." });
    }

    // ─────────────────────────────────────────────────────────────────────────
    // POST /api/tasks
    // Crea una nueva tarea. Retorna 201 con Location header.
    // ─────────────────────────────────────────────────────────────────────────
    [HttpPost]
    [ProducesResponseType(typeof(TaskItem), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] TaskItem newTask)
    {
        if (string.IsNullOrWhiteSpace(newTask.Title))
            return BadRequest(new { message = "El título de la tarea es obligatorio." });

        var created = _repository.Create(newTask);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PUT /api/tasks/{id}
    // Reemplaza completamente una tarea existente. 404 si no existe.
    // ─────────────────────────────────────────────────────────────────────────
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TaskItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] TaskItem updatedTask)
    {
        if (string.IsNullOrWhiteSpace(updatedTask.Title))
            return BadRequest(new { message = "El título de la tarea es obligatorio." });

        var result = _repository.Update(id, updatedTask);

        return result is not null
            ? Ok(result)
            : NotFound(new { message = $"Tarea con ID {id} no encontrada." });
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PATCH /api/tasks/{id}/complete
    // Marca una tarea como completada. Operación idempotente.
    // ─────────────────────────────────────────────────────────────────────────
    [HttpPatch("{id:int}/complete")]
    [ProducesResponseType(typeof(TaskItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Complete(int id)
    {
        var task = _repository.GetById(id);
        if (task is null)
            return NotFound(new { message = $"Tarea con ID {id} no encontrada." });

        var completed = new TaskItem
        {
            Title = task.Title,
            Description = task.Description,
            IsCompleted = true,
            Priority = task.Priority
        };

        var result = _repository.Update(id, completed);
        return Ok(result);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // DELETE /api/tasks/{id}
    // Elimina una tarea. 204 si se eliminó, 404 si no existía.
    // ─────────────────────────────────────────────────────────────────────────
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var deleted = _repository.Delete(id);

        return deleted
            ? NoContent()
            : NotFound(new { message = $"Tarea con ID {id} no encontrada." });
    }
}