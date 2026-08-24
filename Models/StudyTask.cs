namespace StudyAPI.Models;

/// <summary>
/// Status possíveis de uma tarefa de estudo
/// </summary>
public enum StudyTaskStatus
{
    /// <summary>Tarefa ainda não iniciada</summary>
    Pending = 0,

    /// <summary>Tarefa em andamento</summary>
    InProgress = 1,

    /// <summary>Tarefa concluída com sucesso</summary>
    Completed = 2,

    /// <summary>Tarefa cancelada</summary>
    Cancelled = 3
}

/// <summary>
/// Níveis de prioridade de uma tarefa
/// </summary>
public enum Priority
{
    /// <summary>Prioridade baixa - pode esperar</summary>
    Low = 0,

    /// <summary>Prioridade média - importante mas não urgente</summary>
    Medium = 1,

    /// <summary>Prioridade alta - precisa ser feita em breve</summary>
    High = 2,

    /// <summary>Prioridade urgente - fazer imediatamente</summary>
    Critical = 3
}

/// <summary>
/// Representa uma tarefa de estudo no sistema
/// Uma tarefa está associada a uma categoria e contém detalhes sobre o que precisa ser estudado
/// </summary>
public class StudyTask
{
    /// <summary>
    /// Identificador único da tarefa (chave primária)
    /// Gerado automaticamente pelo banco de dados
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Título da tarefa de estudo (ex: "Estular_classes_em_CSharp")
    /// Campo obrigatório, máximo 200 caracteres
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada do que precisa ser estudado
    /// Campo opcional, mas recomendado para documentar o conteúdo
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Status atual da tarefa (Pending, InProgress, Completed, Cancelled)
    /// Define o estágio em que a tarefa se encontra
    /// </summary>
    public StudyTaskStatus Status { get; set; } = StudyTaskStatus.Pending;

    /// <summary>
    /// Nível de prioridade da tarefa (Low, Medium, High, Critical)
    /// Ajuda a organizar a ordem de execução das tarefas
    /// </summary>
    public Priority Priority { get; set; } = Priority.Medium;

    /// <summary>
    /// Data e hora agendada para conclusão da tarefa
    /// Campo opcional, usado para prazos
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Estimativa de tempo necessário para completar a tarefa (em minutos)
    /// Campo opcional, útil para planejamento
    /// </summary>
    public int? EstimatedMinutes { get; set; }

    /// <summary>
    /// Tempo realmente gasto na tarefa (em minutos)
    /// Atualizado conforme o usuário avança nos estudos
    /// </summary>
    public int? ActualMinutes { get; set; }

    /// <summary>
    /// Notas pessoais sobre a tarefa
    /// Campo livre para o usuário registrar observações
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Identificador da categoria à qual esta tarefa pertence
    /// Chave estrangeira para o relacionamento com Category
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Propriedade de navegação para a categoria
    /// Permite acessar os dados da categoria a partir da tarefa
    /// </summary>
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Data e hora em que a tarefa foi criada
    /// Configurada automaticamente para o momento atual
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data e hora da última atualização da tarefa
    /// Atualizada sempre que a tarefa é modificada
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
