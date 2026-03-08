using Api.Models;

namespace Api.Repositories;

/// <summary>
/// Contrato del repositorio de tareas.
/// Cualquier implementación (en memoria, SQL, etc.) debe cumplir esta interfaz.
/// </summary>
public interface ITaskRepository
{
    IEnumerable<TaskItem> GetAll();
    TaskItem? GetById(int id);
    TaskItem Create(TaskItem task);
    TaskItem? Update(int id, TaskItem task);
    bool Delete(int id);
}