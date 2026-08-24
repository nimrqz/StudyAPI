using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using StudyAPI.Data;
using StudyAPI.Repositories;
using StudyAPI.Services;
using StudyAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// 1. CONFIGURAÇÃO DO BANCO DE DADOS (SQLite)
// ============================================
// Adiciona o Entity Framework Core com SQLite ao container de dependências
// A string de conexão vem do appsettings.json
// O SQLite cria um arquivo .db no diretório da aplicação
builder.Services.AddDbContext<StudyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ============================================
// 2. REGISTRO DO REPOSITORY PATTERN
// ============================================
// AddScoped: uma instância por requisição HTTP
// Quando a requisição termina, a instância é descartada
// Isso garante que cada requisição tenha seu próprio contexto do banco
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IStudyTaskRepository, StudyTaskRepository>();

// ============================================
// 3. REGISTRO DOS SERVICES (Lógica de Negócio)
// ============================================
// AddScoped: mesma lifetime dos repositórios
// O Service depende do Repository, que será injetado automaticamente
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStudyTaskService, StudyTaskService>();

// ============================================
// 4. CONFIGURAÇÃO DO FLUENTVALIDATION
// ============================================
// Adiciona validadores automaticamente扫描 todos os AbstractValidator<T> no assembly
// Os validadores são chamados automaticamente quando [ApiController] está presente
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();

// ============================================
// 5. CONFIGURAÇÃO DOS CONTROLLERS
// ============================================
// Adiciona suporte a controllers com serialização JSON
// SwaggerCounts e ProducesResponseType para documentação
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configura serialização de enums como strings (não números)
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// ============================================
// 6. CONFIGURAÇÃO DO SWAGGER/OpenAPI
// ============================================
// Swagger: documentação interativa da API
// Permite testar endpoints diretamente no navegador
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ============================================
// 7. CONFIGURAÇÃO DO CORS (Cross-Origin Resource Sharing)
// ============================================
// Permite que aplicações web de outros domínios acessem a API
// Útil para testar com frontends em React, Angular, etc.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ============================================
// 8. APLICAR MIGRAÇÕES DO BANCO DE DADOS
// ============================================
// Cria o banco de dados automaticamente se não existir
// Aplica todas as migrações pendentes
// Executa o seed data (dados iniciais)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StudyDbContext>();
    dbContext.Database.Migrate();
}

// ============================================
// 9. CONFIGURAÇÃO DO PIPELINE HTTP
// ============================================

// Swagger: só em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "StudyAPI v1");
        options.RoutePrefix = string.Empty;  // Swagger na raiz: http://localhost:5000
    });
}

// Redireciona HTTP para HTTPS (segurança)
app.UseHttpsRedirection();

// Habilita CORS (deve vir antes de Authorization e MapControllers)
app.UseCors();

// Habilita autorização (será usada no futuro)
app.UseAuthorization();

// Mapeia os Controllers para as rotas definidas
// Isso faz com que [Route("api/[controller]")] funcione
app.MapControllers();

// ============================================
// 10. ENDPOINT RAIZ (opcional)
// ============================================
// Retorna informações básicas da API quando acessa http://localhost:5000
app.MapGet("/", () => new
{
    name = "StudyAPI",
    version = "1.0",
    description = "API de Estudo para C# e ASP.NET Core",
    swagger = "/swagger",
    endpoints = new
    {
        categories = "/api/categories",
        studyTasks = "/api/studytasks",
        statsStatus = "/api/studytasks/stats/status",
        statsPriority = "/api/studytasks/stats/priority"
    }
});

app.Run();
