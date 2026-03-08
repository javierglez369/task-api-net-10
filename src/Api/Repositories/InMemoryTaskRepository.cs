using Api.Models;

namespace Api.Repositories;

/// <summary>
/// Implementación en memoria del repositorio de tareas.
/// Usada durante el curso; se reemplazaría por una implementación con Entity Framework en producción.
/// </summary>
public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = new()
    {
        new TaskItem
        {
            Id = 1,
            Title = "Aprender Git",
            Description = "Dominar los comandos básicos de Git para trabajo en equipo.",
            IsCompleted = true,
            Priority = Priority.High,
            CreatedAt = DateTime.UtcNow.AddDays(-7)
        },
        new TaskItem
        {
            Id = 2,
            Title = "Aprender GitHub Actions",
            Description = "Crear workflows de CI/CD para proyectos .NET.",
            IsCompleted = false,
            Priority = Priority.High,
            CreatedAt = DateTime.UtcNow.AddDays(-3)
        },
        new TaskItem
        {
            Id = 3,
            Title = "Configurar branch protection",
            Description = "Proteger la rama main para requerir PRs y status checks.",
            IsCompleted = false,
            Priority = Priority.Medium,
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        }
    };

    private int _nextId = 4;

    public IEnumerable<TaskItem> GetAll() => _tasks.AsReadOnly();

    public TaskItem? GetById(int id) =>
        _tasks.FirstOrDefault(t => t.Id == id);

    public TaskItem Create(TaskItem task)
    {
        task.Id = _nextId++;
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = null;
        _tasks.Add(task);
        return task;
    }

    public TaskItem? Update(int id, TaskItem updatedTask)
    {
        var existing = _tasks.FirstOrDefault(t => t.Id == id);
        if (existing is null) return null;

        existing.Title = updatedTask.Title;
        existing.Description = updatedTask.Description;
        existing.IsCompleted = updatedTask.IsCompleted;
        existing.Priority = updatedTask.Priority;
        existing.UpdatedAt = DateTime.UtcNow;

        return existing;
    }

    public bool Delete(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task is null) return false;

        _tasks.Remove(task);
        return true;
    }
}