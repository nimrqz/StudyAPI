# StudyAPI - Documentação do Projeto

API RESTful para gerenciamento de tarefas de estudo, criada para aprender C# e ASP.NET Core.

## Estrutura do Projeto

```
StudyAPI/
├── Controllers/          # Endpoints da API (recebe requisições HTTP)
├── Models/               # Entidades de dados (tabelas do banco)
├── DTOs/                 # Data Transfer Objects (objetos de transferência)
├── Data/                 # Contexto do Entity Framework (conexão com banco)
├── Repositories/         # Acesso a dados (operações no banco)
├── Services/             # Lógica de negócio (regras e validações)
└── Validators/           # Validação de dados (FluentValidation)
```

## Como Funciona o Fluxo de uma Requisição

```
Cliente (Postman/Swagger)
    ↓ HTTP Request
Controller (recebe e valida a requisição)
    ↓ chama
Service (aplica regras de negócio)
    ↓ chama
Repository (acessa o banco de dados)
    ↓
Entity Framework Core → SQLite (banco de dados)
```

**Exemplo prático:** Quando você cria uma tarefa via POST:
1. O **Controller** recebe os dados JSON e converte para `CreateStudyTaskDto`
2. O **Service** verifica se a categoria existe e cria a entidade `StudyTask`
3. O **Repository** salva no banco via Entity Framework
4. O banco retorna o ID gerado
5. O **Service** converte para `StudyTaskResponseDto`
6. O **Controller** retorna HTTP 201 com os dados criados

---

## Camadas do Projeto

### 1. Models (`Models/`)

As classes que representam as tabelas do banco de dados.

**Category.cs** - Categorias de estudo (ex: "C# Básico", "ASP.NET Core")
- `Id` - Identificador único (chave primária)
- `Name` - Nome da categoria (obrigatório, máx 100 chars)
- `Description` - Descrição opcional
- `Color` - Cor hexadecimal para UI (ex: "#3498DB")
- `CreatedAt` - Data de criação (automática)
- `UpdatedAt` - Data da última atualização
- `StudyTasks` - Coleção de tarefas desta categoria (relacionamento 1:N)

**StudyTask.cs** - Tarefas de estudo
- `Id` - Identificador único
- `Title` - Título da tarefa (obrigatório, máx 200 chars)
- `Description` - Descrição detalhada
- `Status` - Status atual (Pending, InProgress, Completed, Cancelled)
- `Priority` - Prioridade (Low, Medium, High, Critical)
- `DueDate` - Data de vencimento
- `EstimatedMinutes` - Tempo estimado em minutos
- `ActualMinutes` - Tempo realmente gasto
- `Notes` - Notas pessoais
- `CategoryId` - ID da categoria (chave estrangeira)
- `Category` - Propriedade de navegação para a categoria

### 2. DTOs (`DTOs/`)

Objetos usados para enviar e receber dados pela API. Separados dos Models por segurança e flexibilidade.

**CreateCategoryDto** - Dados para criar uma categoria (Name, Description, Color)
**UpdateCategoryDto** - Dados para atualizar (todos opcionais)
**CategoryResponseDto** - Dados retornados pela API (inclui Id, CreatedAt, TaskCount)

**CreateStudyTaskDto** - Dados para criar uma tarefa (Title, Priority, CategoryId, etc.)
**UpdateStudyTaskDto** - Dados para atualizar (todos opcionais)
**StudyTaskResponseDto** - Dados retornados pela API (inclui resumo da categoria)
**StudyTaskQueryDto** - Parâmetros de busca (filtros, paginação, ordenação)
**PaginatedResponseDto** - Resposta paginada (Items, TotalPages, HasNextPage, etc.)

### 3. Data (`Data/`)

**StudyDbContext.cs** - Conexão com o banco de dados
- Configura as tabelas do banco via Fluent API
- Define relacionamentos (ex: StudyTask → Category)
- Cria dados iniciais (seed data) com 3 categorias padrão
- Usa SQLite como banco (cria arquivo `StudyAPI.db`)

### 4. Repositories (`Repositories/`)

Camada que acessa diretamente o banco de dados. Usa Entity Framework Core.

**ICategoryRepository / CategoryRepository**
- `GetAllAsync()` - Lista todas as categorias
- `GetByIdAsync(id)` - Busca por ID
- `GetByNameAsync(name)` - Busca por nome
- `CreateAsync(category)` - Cria nova categoria
- `UpdateAsync(category)` - Atualiza categoria
- `DeleteAsync(id)` - Remove categoria
- `ExistsAsync(id)` - Verifica se existe
- `CountAsync()` - Conta total

**IStudyTaskRepository / StudyTaskRepository**
- `GetFilteredAsync(query)` - Busca com filtros, paginação e ordenação
- `GetByIdAsync(id)` - Busca por ID com categoria
- `CreateAsync(task)` - Cria nova tarefa
- `UpdateAsync(task)` - Atualiza tarefa
- `DeleteAsync(id)` - Remove tarefa
- `CountByStatusAsync()` - Conta por status
- `CountByPriorityAsync()` - Conta por prioridade

### 5. Services (`Services/`)

Camada de lógica de negócio. Faz a conversão entre DTOs e Models.

**ICategoryService / CategoryService**
- Valida se o nome não está duplicado antes de criar
- Converte Category → CategoryResponseDto
- Trata erros e lança exceções apropriadas

**IStudyTaskService / StudyTaskService**
- Valida se a categoria existe antes de criar tarefa
- Converte StudyTask → StudyTaskResponseDto
- Monta resposta paginada completa
- Limita PageSize em 100 itens máximo

### 6. Controllers (`Controllers/`)

Endpoints da API. Recebem requisições HTTP e retornam respostas.

**CategoriesController** (`/api/categories`)
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | /api/categories | Lista todas as categorias |
| GET | /api/categories/{id} | Busca categoria por ID |
| POST | /api/categories | Cria nova categoria |
| PUT | /api/categories/{id} | Atualiza categoria |
| DELETE | /api/categories/{id} | Remove categoria |

**StudyTasksController** (`/api/studytasks`)
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | /api/studytasks | Lista tarefas (com filtros e paginação) |
| GET | /api/studytasks/{id} | Busca tarefa por ID |
| POST | /api/studytasks | Cria nova tarefa |
| PUT | /api/studytasks/{id} | Atualiza tarefa |
| DELETE | /api/studytasks/{id} | Remove tarefa |
| GET | /api/studytasks/stats/status | Contagem por status |
| GET | /api/studytasks/stats/priority | Contagem por prioridade |

### 7. Validators (`Validators/`)

Validação automática usando FluentValidation.

**CreateCategoryValidator** - Valida dados de criação de categoria
**UpdateCategoryValidator** - Valida dados de atualização
**CreateStudyTaskValidator** - Valida dados de criação de tarefa
**UpdateStudyTaskValidator** - Valida dados de atualização

Regras de exemplo:
- Name: obrigatório, 3-100 caracteres
- Title: obrigatório, 5-200 caracteres
- Color: formato hexadecimal (#RRGGBB)
- DueDate: deve ser no futuro
- CategoryId: deve ser maior que 0

---

## Endpoints Exemplos

### Criar uma categoria
```http
POST /api/categories
Content-Type: application/json

{
  "name": "JavaScript",
  "description": "Linguagem de programação para web",
  "color": "#F7DF1E"
}
```

### Criar uma tarefa
```http
POST /api/studytasks
Content-Type: application/json

{
  "title": "Estudar arrow functions",
  "description": "Aprender sintaxe e casos de uso",
  "priority": "High",
  "dueDate": "2026-12-31T23:59:59",
  "estimatedMinutes": 60,
  "categoryId": 1
}
```

### Buscar tarefas com filtros
```http
GET /api/studytasks?status=Pending&priority=High&page=1&pageSize=5&sortBy=DueDate&ascending=true
```

### Resposta paginada
```json
{
  "items": [...],
  "currentPage": 1,
  "totalItems": 42,
  "totalPages": 9,
  "hasNextPage": true,
  "hasPreviousPage": false
}
```

---

## Tecnologias Utilizadas

| Tecnologia | Versão | Para quê |
|------------|--------|----------|
| .NET | 10.0 | Framework da aplicação |
| ASP.NET Core | 10.0 | Framework web (API REST) |
| Entity Framework Core | 10.0 | ORM (mapeamento objeto-relacional) |
| SQLite | - | Banco de dados leve e local |
| FluentValidation | 11.x | Validação de dados |
| Swashbuckle | 10.x | Documentação Swagger/OpenAPI |

---

## Como Rodar

```bash
# Restaurar pacotes
dotnet restore

# Criar migração do banco
dotnet ef migrations add InitialCreate

# Rodar a aplicação
dotnet run

# Acessar Swagger
http://localhost:5000

# Acessar API diretamente
http://localhost:5000/api/categories
http://localhost:5000/api/studytasks
```

---

## Conceitos Aprendidos neste Projeto

1. **Arquitetura em Camadas** - Controllers → Services → Repositories → Database
2. **Repository Pattern** - Separa acesso a dados da lógica de negócio
3. **Dependency Injection** - Injeção de dependências do ASP.NET Core
4. **DTOs** - Separação entre entidades do banco e objetos de API
5. **Entity Framework Core** - ORM, migrações, Fluent API, relaciones
6. **FluentValidation** - Validação de dados declarativa
7. **REST API** - Padrões HTTP (GET, POST, PUT, DELETE), códigos de status
8. **Paginação** - Response com dados + metadados de paginação
9. **Swagger/OpenAPI** - Documentação interativa da API
10. **SQLite** - Banco de dados leve para desenvolvimento
