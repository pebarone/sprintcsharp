// Local: sprintcsharp/Program.cs
using Microsoft.EntityFrameworkCore;
using sprintcsharp.Data;
using sprintcsharp.Models;
using sprintcsharp.Services; // Importa o novo ApiClientService

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configurar Serviços ---

// Pega a string de conexão do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("OracleConnection");

// Log da connection string (mascarar senha em produção)
if (!string.IsNullOrEmpty(connectionString))
{
    var preview = connectionString.Length > 50 ? connectionString.Substring(0, 50) : connectionString;
    Console.WriteLine($"[DEBUG] Connection String: {preview}...");
}
else
{
    Console.WriteLine("[WARN] Connection String não configurada!");
}

// Adiciona o DbContext (Requisito 1: EF Core)
builder.Services.AddDbContext<MeuDbContext>(options =>
{
    options.UseOracle(connectionString);
    // Habilita logs detalhados de SQL em Development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Adiciona o Repositório (que agora usa EF)
builder.Services.AddScoped<ProdutoInvestimentoRepository>();

// Adiciona o Swagger (Requisito 1: API e Documentação)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Investimentos - Sprint C#",
        Version = "v1",
        Description = @"
## 📊 API completa de Gestão de Produtos de Investimento

### Requisitos Atendidos:
- ✅ **ASP.NET Core Web API** com Entity Framework
- ✅ **CRUD Completo** de Produtos de Investimento
- ✅ **Consultas LINQ Avançadas** (filtros, ordenação, agregação, paginação)
- ✅ **Integração com API Externa** (dados de investimentos)
- ✅ **Documentação Swagger** completa
- ✅ **Publicação em Cloud** (via Docker)

### Desenvolvido por:
FIAP - Curso de Análise e Desenvolvimento de Sistemas
",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Suporte FIAP",
            Email = "suporte@fiap.com.br"
        }
    });
});

// Adiciona o HttpClient (para o Requisito 4)
builder.Services.AddHttpClient();
builder.Services.AddScoped<ApiClientService>();


var app = builder.Build();

// --- 2. Configurar o Pipeline (Middleware) ---

// Middleware de tratamento de exceções global
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        
        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        
        var errorResponse = new
        {
            error = "Internal Server Error",
            message = exception?.Message ?? "Erro desconhecido",
            details = app.Environment.IsDevelopment() ? exception?.StackTrace : null
        };
        
        Console.WriteLine($"[ERRO] {exception?.Message}");
        Console.WriteLine($"[ERRO STACK] {exception?.StackTrace}");
        
        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});

// Log de inicialização
Console.WriteLine("[INFO] Aplicação iniciando...");
Console.WriteLine($"[INFO] Ambiente: {app.Environment.EnvironmentName}");

// Habilita o Swagger (Interface gráfica da documentação)
// Movido para fora do "if" para que funcione no deploy do Render/Azure (Produção)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Investimentos v1");
    options.RoutePrefix = string.Empty; // Swagger na raiz (http://localhost:5000/)
    options.DocumentTitle = "API de Investimentos - Documentação";
});

if (app.Environment.IsDevelopment())
{
    // O "if" agora pode ficar vazio
}

// --- 3. Definir os Endpoints (CRUD) ---

// Endpoint principal - Redireciona para o Swagger
app.MapGet("/", () => Results.Redirect("/swagger"))
.ExcludeFromDescription();

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


// ===== ENDPOINTS COM LINQ AVANÇADO (Requisito 2: 10%) =====

// GET /produtos/tipo/{tipo} - Buscar por tipo
app.MapGet("/produtos/tipo/{tipo}", (string tipo, ProdutoInvestimentoRepository repo) =>
{
    var produtos = repo.BuscarPorTipo(tipo);
    return produtos.Any() ? Results.Ok(produtos) : Results.NotFound("Nenhum produto encontrado neste tipo.");
})
.WithName("BuscarPorTipo")
.WithTags("Consultas LINQ Avançadas")
.WithDescription("Busca produtos por tipo usando LINQ Where (Tipos: Renda Fixa, FII, Ações, BDR, Cripto)")
.Produces<List<ProdutoInvestimento>>(200)
.Produces(404);

// GET /produtos/preco/{minimo} - Buscar por preço mínimo
app.MapGet("/produtos/preco/{minimo}", (decimal minimo, ProdutoInvestimentoRepository repo) =>
{
    var produtos = repo.BuscarPorPrecoMinimo(minimo);
    return Results.Ok(produtos);
})
.WithName("BuscarPorPreco")
.WithTags("Consultas LINQ Avançadas")
.WithDescription("Busca produtos com preço mínimo usando LINQ Where + OrderByDescending")
.Produces<List<ProdutoInvestimento>>(200);

// GET /produtos/risco/{nivel} - Buscar por nível de risco
app.MapGet("/produtos/risco/{nivel}", (string nivel, ProdutoInvestimentoRepository repo) =>
{
    var produtos = repo.BuscarPorRisco(nivel);
    return produtos.Any() ? Results.Ok(produtos) : Results.NotFound("Nenhum produto encontrado com este nível de risco.");
})
.WithName("BuscarPorRisco")
.WithTags("Consultas LINQ Avançadas")
.WithDescription("Busca produtos por nível de risco usando LINQ Where + OrderBy (Riscos: Baixo, Médio, Alto)")
.Produces<List<ProdutoInvestimento>>(200)
.Produces(404);

// GET /produtos/estatisticas - Estatísticas agregadas
app.MapGet("/produtos/estatisticas", (ProdutoInvestimentoRepository repo) =>
{
    var stats = repo.ObterEstatisticas();
    return Results.Ok(stats);
})
.WithName("ObterEstatisticas")
.WithTags("Consultas LINQ Avançadas")
.WithDescription("Retorna estatísticas agregadas usando LINQ (Count, Average, Max, Min, GroupBy)")
.Produces<object>(200);

// GET /produtos/buscar - Busca com múltiplos filtros
app.MapGet("/produtos/buscar", (ProdutoInvestimentoRepository repo, string? tipo = null, string? risco = null, decimal? precoMinimo = null, string? ordenarPor = null) =>
{
    var produtos = repo.BuscarComFiltros(tipo, risco, precoMinimo, ordenarPor);
    return Results.Ok(produtos);
})
.WithName("BuscarComFiltros")
.WithTags("Consultas LINQ Avançadas")
.WithDescription("Busca com filtros múltiplos e ordenação dinâmica usando LINQ complexo")
.Produces<List<ProdutoInvestimento>>(200);

// GET /produtos/resumo - Projeção de dados
app.MapGet("/produtos/resumo", (ProdutoInvestimentoRepository repo) =>
{
    var resumo = repo.ObterResumo();
    return Results.Ok(resumo);
})
.WithName("ObterResumo")
.WithTags("Consultas LINQ Avançadas")
.WithDescription("Retorna resumo dos produtos usando LINQ Select (projeção)")
.Produces<List<object>>(200);

// GET /produtos/paginado - Paginação
app.MapGet("/produtos/paginado", (ProdutoInvestimentoRepository repo, int pagina = 1, int itensPorPagina = 10) =>
{
    var resultado = repo.ObterProdutosPaginados(pagina, itensPorPagina);
    return Results.Ok(resultado);
})
.WithName("ObterProdutosPaginados")
.WithTags("Consultas LINQ Avançadas")
.WithDescription("Retorna produtos paginados usando LINQ Skip e Take")
.Produces<object>(200);


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