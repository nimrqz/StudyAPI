namespace StudyAPI.Models;

/// <summary>
/// Representa uma categoria de estudo
/// Uma categoria agrupa tarefas relacionadas (ex: "C# Básico", "ASP.NET Core", "Entity Framework")
/// </summary>
public class Category
{
    /// <summary>
    /// Identificador único da categoria (chave primária)
    /// Gerado automaticamente pelo banco de dados
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome da categoria (ex: "C# Básico")
    /// Campo obrigatório, máximo 100 caracteres
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descrição opcional sobre o que essa categoria aborda
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Cor hexadecimal para identificação visual (ex: "#FF5733")
    /// Opcional, usado para interface do usuário
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Data e hora em que a categoria foi criada
    /// Configurada automaticamente para o momento atual
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data e hora da última atualização da categoria
    /// Atualizada sempre que a categoria é modificada
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Coleção de tarefas que pertencem a esta categoria
    /// Relacionamento um-para-muitos (Uma categoria tem muitas tarefas)
    /// </summary>
    public ICollection<StudyTask> StudyTasks { get; set; } = new List<StudyTask>();
}
