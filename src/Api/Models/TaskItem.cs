namespace Api.Models;

/// <summary>
/// Representa una tarea dentro del sistema de gestión.
/// </summary>
public class TaskItem
{
    /// <summary>Identificador único de la tarea.</summary>
    public int Id { get; set; }

    /// <summary>Título descriptivo de la tarea.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Descripción opcional con detalles adicionales.</summary>
    public string? Description { get; set; }

    /// <summary>Indica si la tarea ha sido completada.</summary>
    public bool IsCompleted { get; set; }

    /// <summary>Prioridad de la tarea: Low, Medium, High.</summary>
    public Priority Priority { get; set; } = Priority.Medium;

    /// <summary>Fecha y hora de creación (UTC).</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Fecha y hora de última modificación (UTC).</summary>
    public DateTime? UpdatedAt { get; set; }
// }

// /// <summary>Niveles de prioridad de una tarea.</summary>
// public enum Priority
// {
//     Low = 0,
//     Medium = 1,
//     High = 2
// }