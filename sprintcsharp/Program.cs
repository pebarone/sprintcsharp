// Local: sprintcsharp/Program.cs
using Microsoft.EntityFrameworkCore;
using sprintcsharp.Data;
using sprintcsharp.Models;
using sprintcsharp.Services; // Importa o novo ApiClientService

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configurar Serviços ---

// Pega a string de conexão do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("OracleConnection");

// Adiciona o DbContext (Requisito 1: EF Core)
builder.Services.AddDbContext<MeuDbContext>(options =>
    options.UseOracle(connectionString)
);

// Adiciona o Repositório (que agora usa EF)
builder.Services.AddScoped<ProdutoInvestimentoRepository>();

// Adiciona o Swagger (Requisito 1: API e Documentação)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona o HttpClient (para o Requisito 4)
builder.Services.AddHttpClient();
builder.Services.AddScoped<ApiClientService>();


var app = builder.Build();

// --- 2. Configurar o Pipeline (Middleware) ---

// Habilita o Swagger (Interface gráfica da documentação)
// Movido para fora do "if" para que funcione no deploy do Render (Produção)
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    // O "if" agora pode ficar vazio
}

// --- 3. Definir os Endpoints (CRUD) ---

// Endpoint principal
app.MapGet("/", () => "API de Produtos de Investimento está online.");

// GET /produtos (Listar todos)
app.MapGet("/produtos", (ProdutoInvestimentoRepository repo) =>
{
    return Results.Ok(repo.GetAll()); // Usa o repositório com LINQ
})
.WithTags("CRUD Produtos (EF Core)")
.Produces<List<ProdutoInvestimento>>(200);

// GET /produtos/{id} (Buscar por ID)
app.MapGet("/produtos/{id}", (int id, ProdutoInvestimentoRepository repo) =>
{
    var produto = repo.GetById(id);
    return produto != null ? Results.Ok(produto) : Results.NotFound();
})
.WithTags("CRUD Produtos (EF Core)")
.Produces<ProdutoInvestimento>(200)
.Produces(404);

// POST /produtos (Adicionar novo)
app.MapPost("/produtos", (ProdutoInvestimento produto, ProdutoInvestimentoRepository repo) =>
{
    repo.Add(produto);
    return Results.Created($"/produtos/{produto.Id}", produto);
})
.WithTags("CRUD Produtos (EF Core)")
.Produces<ProdutoInvestimento>(201);

// PUT /produtos/{id} (Atualizar)
app.MapPut("/produtos/{id}", (int id, ProdutoInvestimento produto, ProdutoInvestimentoRepository repo) =>
{
    if (id != produto.Id) return Results.BadRequest("ID da URL não bate com o ID do corpo");
    
    var existente = repo.GetById(id);
    if (existente == null) return Results.NotFound();

    repo.Update(produto);
    return Results.Ok(produto);
})
.WithTags("CRUD Produtos (EF Core)")
.Produces<ProdutoInvestimento>(200)
.Produces(400)
.Produces(404);

// DELETE /produtos/{id} (Deletar)
app.MapDelete("/produtos/{id}", (int id, ProdutoInvestimentoRepository repo) =>
{
    var existente = repo.GetById(id);
    if (existente == null) return Results.NotFound();
    
    repo.Delete(id);
    return Results.NoContent();
})
.WithTags("CRUD Produtos (EF Core)")
.Produces(204)
.Produces(404);


// --- 4. Endpoint para o Requisito 4 (Conectar com outra API) ---

app.MapGet("/investimentos-externos", async (ApiClientService apiClient) =>
{
    var produtosExternos = await apiClient.GetProdutosDaApiExterna();
    if (produtosExternos == null)
    {
        return Results.StatusCode(502); // Bad Gateway (erro ao falar com a outra API)
    }
    return Results.Ok(produtosExternos);
})
.WithTags("API Externa (Requisito 4)")
.Produces<List<ProdutoApiExterna>>(200)
.Produces(502); // Erro


// --- NOVO ENDPOINT ADICIONADO ---

app.MapGet("/investimentos-externos/{id}", async (int id, ApiClientService apiClient) =>
{
    var produtoExterno = await apiClient.GetProdutoDaApiExternaPorId(id);
    
    if (produtoExterno == null)
    {
        // Retorna 404 (Não Encontrado) se a API externa não encontrar o produto
        return Results.NotFound($"Produto com ID {id} não encontrado na API externa.");
    }
    
    return Results.Ok(produtoExterno);
})
.WithTags("API Externa (Requisito 4)")
.Produces<ProdutoApiExterna>(200)
.Produces(404); // Não Encontrado


// --- 5. Rodar a API ---
app.Run();