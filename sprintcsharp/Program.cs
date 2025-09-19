// Local: sprintcsharp/Program.cs

// 1. Importar as classes que criamos em outras pastas
using sprintcsharp.Data;
using sprintcsharp.Models;
using sprintcsharp.Services; // <-- ADICIONADO

// Instanciar nossos serviços e repositórios
var repositorio = new ProdutoInvestimentoRepository();
var fileService = new FileService(); // <-- ADICIONADO

// Loop principal do menu
while (true)
{
    Console.WriteLine("\n--- Gestor de Produtos de Investimento ---");
    Console.WriteLine("1. Listar todos os produtos");
    Console.WriteLine("2. Adicionar novo produto");
    Console.WriteLine("3. Atualizar produto existente");
    Console.WriteLine("4. Deletar produto");
    Console.WriteLine("5. Exportar produtos para JSON"); // <-- ADICIONADO
    Console.WriteLine("6. Importar produtos de JSON"); // <-- ADICIONADO
    Console.WriteLine("0. Sair");
    Console.Write("Escolha uma opção: ");

    var opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            ListarProdutos();
            break;
        case "2":
            AdicionarProduto();
            break;
        case "3":
            AtualizarProduto();
            break;
        case "4":
            DeletarProduto();
            break;
        case "5": // <-- ADICIONADO
            ExportarProdutos();
            break;
        case "6": // <-- ADICIONADO
            ImportarProdutos();
            break;
        case "0":
            Console.WriteLine("Saindo...");
            return;
        default:
            Console.WriteLine("Opção inválida! Tente novamente.");
            break;
    }
}

// ---- MÉTODOS PARA CADA OPÇÃO DO MENU ----
// (Os métodos Listar, Adicionar, Atualizar e Deletar continuam os mesmos)

void ListarProdutos()
{
    Console.WriteLine("\n--- Lista de Produtos ---");
    try
    {
        var produtos = repositorio.GetAll();
        if (produtos.Count == 0)
        {
            Console.WriteLine("Nenhum produto cadastrado.");
            return;
        }
        foreach (var produto in produtos)
        {
            Console.WriteLine(produto.ToString());
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ocorreu um erro ao listar os produtos: {ex.Message}");
    }
}

void AdicionarProduto()
{
    Console.WriteLine("\n--- Adicionar Novo Produto ---");
    try
    {
        Console.Write("Nome: ");
        var nome = Console.ReadLine();
        Console.Write("Tipo (ex: Renda Fixa, Ações): ");
        var tipo = Console.ReadLine();
        Console.Write("Risco (Baixo, Médio, Alto): ");
        var risco = Console.ReadLine();
        Console.Write("Preço: ");
        var preco = Convert.ToDecimal(Console.ReadLine());
        var novoProduto = new ProdutoInvestimento { Nome = nome, Tipo = tipo, Risco = risco, Preco = preco };
        repositorio.Add(novoProduto);
        Console.WriteLine("Produto adicionado com sucesso!");
    }
    catch (FormatException)
    {
        Console.WriteLine("Erro: O preço deve ser um número válido.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ocorreu um erro ao adicionar o produto: {ex.Message}");
    }
}

void AtualizarProduto()
{
    Console.WriteLine("\n--- Atualizar Produto ---");
    ListarProdutos();
    Console.Write("\nDigite o ID do produto que deseja atualizar: ");
    try
    {
        var id = Convert.ToInt32(Console.ReadLine());
        Console.Write("Novo Nome: ");
        var nome = Console.ReadLine();
        Console.Write("Novo Tipo: ");
        var tipo = Console.ReadLine();
        Console.Write("Novo Risco (Baixo, Médio, Alto): ");
        var risco = Console.ReadLine();
        Console.Write("Novo Preço: ");
        var preco = Convert.ToDecimal(Console.ReadLine());
        var produtoAtualizado = new ProdutoInvestimento { Id = id, Nome = nome, Tipo = tipo, Risco = risco, Preco = preco };
        repositorio.Update(produtoAtualizado);
        Console.WriteLine("Produto atualizado com sucesso!");
    }
    catch (FormatException)
    {
        Console.WriteLine("Erro: O ID e o preço devem ser números válidos.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ocorreu um erro ao atualizar o produto: {ex.Message}");
    }
}

void DeletarProduto()
{
    Console.WriteLine("\n--- Deletar Produto ---");
    ListarProdutos();
    Console.Write("\nDigite o ID do produto que deseja deletar: ");
    try
    {
        var id = Convert.ToInt32(Console.ReadLine());
        Console.Write($"Tem certeza que deseja deletar o produto com ID {id}? (s/n): ");
        var confirmacao = Console.ReadLine();
        if (confirmacao?.ToLower() == "s")
        {
            repositorio.Delete(id);
            Console.WriteLine("Produto deletado com sucesso!");
        }
        else
        {
            Console.WriteLine("Operação cancelada.");
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("Erro: O ID deve ser um número válido.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ocorreu um erro ao deletar o produto: {ex.Message}");
    }
}

// ---- NOVOS MÉTODOS PARA IMPORTAR E EXPORTAR ----

void ExportarProdutos() // <-- NOVO MÉTODO
{
    Console.WriteLine("\n--- Exportar Produtos para JSON ---");
    try
    {
        // 1. Busca todos os produtos do banco de dados
        var produtosDoBanco = repositorio.GetAll();
        // 2. Chama o FileService para salvá-los no arquivo
        fileService.ExportarParaJson(produtosDoBanco);
        Console.WriteLine("Produtos exportados com sucesso para o arquivo 'produtos_exportados.json'!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ocorreu um erro ao exportar os produtos: {ex.Message}");
    }
}

void ImportarProdutos() // <-- NOVO MÉTODO
{
    Console.WriteLine("\n--- Importar Produtos de JSON ---");
    try
    {
        // 1. Chama o FileService para ler a lista de produtos do arquivo
        var produtosDoArquivo = fileService.ImportarDeJson();
        if (produtosDoArquivo.Count == 0)
        {
            Console.WriteLine("Nenhum produto encontrado no arquivo de importação ou o arquivo não existe.");
            return;
        }

        // 2. Para cada produto encontrado no arquivo, adiciona no banco de dados
        foreach (var produto in produtosDoArquivo)
        {
            // O ID não é necessário, pois o banco irá gerar um novo
            repositorio.Add(produto);
        }
        Console.WriteLine($"{produtosDoArquivo.Count} produtos foram importados com sucesso para o banco de dados!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ocorreu um erro ao importar os produtos: {ex.Message}");
    }
}