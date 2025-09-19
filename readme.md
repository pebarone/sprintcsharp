# Gestor de Investimentos - Painel Administrativo

Este projeto consiste em uma aplicação de console desenvolvida em C# como um painel administrativo para gerenciar os dados da plataforma "Assessor de Investimentos Virtual".

A aplicação se conecta diretamente a um banco de dados Oracle para realizar operações de CRUD (Create, Read, Update, Delete) sobre as entidades do sistema, com foco inicial no gerenciamento de produtos de investimento.

## Funcionalidades

-   **CRUD de Produtos:** Gerenciamento completo do catálogo de produtos de investimento.
    -   Listar todos os produtos.
    -   Adicionar um novo produto.
    -   Atualizar as informações de um produto existente.
    -   Deletar um produto.
-   **Manipulação de Arquivos:**
    -   **Exportar:** Salva a lista completa de produtos do banco de dados em um arquivo local `produtos_exportados.json`.
    -   **Importar:** Lê um arquivo `produtos_exportados.json` e insere os registros no banco de dados.

## Tecnologias Utilizadas

-   **Linguagem:** C#
-   **Framework:** .NET 8
-   **Banco de Dados:** Oracle
-   **Driver de Acesso:** `Oracle.ManagedDataAccess.Core` (via NuGet)

## Estrutura do Projeto

O código foi organizado seguindo princípios de separação de responsabilidades para garantir um código limpo e de fácil manutenção:

-   **/Data**: Contém as classes responsáveis pela comunicação com o banco de dados.
    -   `DatabaseConnection.cs`: Gerencia a string de conexão e a abertura de conexões com o Oracle.
    -   `ProdutoInvestimentoRepository.cs`: Implementa os métodos de CRUD para os produtos.
-   **/Models**: Contém as classes que representam as entidades do banco de dados.
    -   `ProdutoInvestimento.cs`: Representa a tabela `investimento_produto`.
-   **/Services**: Contém a lógica para funcionalidades auxiliares.
    -   `FileService.cs`: Implementa a exportação e importação de dados para JSON.
-   `Program.cs`: Ponto de entrada da aplicação, responsável pela interface do usuário no console.

## Pré-requisitos

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   Acesso ao banco de dados Oracle da Fiap.

## Como Executar

1.  **Clone o repositório:**
    ```bash
    git clone [URL_DO_SEU_REPOSITORIO_AQUI]
    cd sprintcsharp
    ```

2.  **Restaure as dependências:**
    O .NET fará isso automaticamente na primeira vez que você executar, mas se necessário, use o comando:
    ```bash
    dotnet restore
    ```

3.  **Execute a aplicação:**
    ```bash
    dotnet run
    ```

    Após a execução, um menu interativo aparecerá no console, permitindo o uso de todas as funcionalidades.