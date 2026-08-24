using FluentValidation;
using StudyAPI.DTOs;

namespace StudyAPI.Validators;

public class CreateStudyTaskValidator : AbstractValidator<CreateStudyTaskDto>
{
    public CreateStudyTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título da tarefa é obrigatório.")
            .MinimumLength(5).WithMessage("O título deve ter pelo menos 5 caracteres.")
            .MaximumLength(200).WithMessage("O título deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("A descrição deve ter no máximo 2000 caracteres.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("O ID da categoria deve ser maior que 0.");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.DueDate.HasValue)
            .WithMessage("A data de vencimento deve ser no futuro.");

        RuleFor(x => x.EstimatedMinutes)
            .GreaterThan(0)
            .When(x => x.EstimatedMinutes.HasValue)
            .WithMessage("A estimativa de tempo deve ser maior que 0 minutos.");
    }
}

public class UpdateStudyTaskValidator : AbstractValidator<UpdateStudyTaskDto>
{
    public UpdateStudyTaskValidator()
    {
        RuleFor(x => x.Title)
            .MinimumLength(5).WithMessage("O título deve ter pelo menos 5 caracteres.")
            .MaximumLength(200).WithMessage("O título deve ter no máximo 200 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("A descrição deve ter no máximo 2000 caracteres.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("O ID da categoria deve ser maior que 0.")
            .When(x => x.CategoryId.HasValue);

        RuleFor(x => x.EstimatedMinutes)
            .GreaterThan(0)
            .When(x => x.EstimatedMinutes.HasValue)
            .WithMessage("A estimativa de tempo deve ser maior que 0 minutos.");

        RuleFor(x => x.ActualMinutes)
            .GreaterThan(0)
            .When(x => x.ActualMinutes.HasValue)
            .WithMessage("O tempo real deve ser maior que 0 minutos.");
    }
}
