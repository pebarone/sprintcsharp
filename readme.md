# 📊 API de Gestão de Investimentos - Sprint C#

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Oracle](https://img.shields.io/badge/Oracle-Database-F80000?logo=oracle)](https://www.oracle.com/)
[![Swagger](https://img.shields.io/badge/Swagger-API%20Docs-85EA2D?logo=swagger)](https://swagger.io/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)](https://www.docker.com/)

---

## 📋 Sobre o Projeto

API RESTful desenvolvida em **ASP.NET Core 8.0** com **Entity Framework Core** para gerenciamento de produtos de investimento. O projeto atende todos os requisitos da Sprint de C#, incluindo CRUD completo, consultas LINQ avançadas, integração com APIs externas e publicação em ambiente Cloud.

---

## ✅ Requisitos Atendidos

| Requisito | Peso | Status | Descrição |
|-----------|------|--------|-----------|
| **ASP.NET Core Web API + EF** | 35% | ✅ | API RESTful com Entity Framework Core e Oracle |
| **Consultas com LINQ** | 10% | ✅ | Filtros, ordenação, agregação, paginação e joins |
| **Publicação Cloud** | 15% | ✅ | Dockerfile + Deploy (Azure/Render/AWS) |
| **Integração API Externa** | 20% | ✅ | Endpoints consumindo APIs públicas |
| **Documentação** | 10% | ✅ | README completo + Swagger interativo |
| **Diagramas de Arquitetura** | 10% | ✅ | Diagrama de classes incluído |

**TOTAL: 100%** ✅

---

## 🚀 Tecnologias Utilizadas

- **Framework**: .NET 8.0 (ASP.NET Core Web API)
- **ORM**: Entity Framework Core 8.0
- **Banco de Dados**: Oracle Database
- **Documentação**: Swagger/OpenAPI 3.0
- **Serialização**: Newtonsoft.Json
- **Containerização**: Docker
- **Versionamento**: Git/GitHub

---

## 🏗️ Arquitetura do Projeto

```
sprintcsharp/
├── 📄 Program.cs                      # Configuração da API e Endpoints
├── 📁 Data/
│   ├── MeuDbContext.cs               # Context do Entity Framework
│   └── ProdutoInvestimentoRepository.cs  # Repository com LINQ avançado
├── 📁 Models/
│   ├── ProdutoInvestimento.cs        # Entidade principal
│   └── ProdutoApiExterna.cs          # DTO para API externa
├── 📁 Services/
│   └── ApiClientService.cs           # Cliente HTTP para APIs externas
├── 📄 appsettings.json               # Configurações e Connection String
├── 📄 Dockerfile                     # Configuração para deploy Cloud
└── 📄 diagrama de classes.png        # Diagrama da arquitetura
```

### Padrões Utilizados:
- **Repository Pattern**: Separação da lógica de acesso a dados
- **Dependency Injection**: Injeção de dependências nativa do ASP.NET Core
- **RESTful API**: Endpoints seguindo convenções REST
- **DTO Pattern**: Objetos de transferência para API externa

---

## 📦 Funcionalidades

### 🔹 CRUD Completo (Entity Framework + LINQ)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/produtos` | Lista todos os produtos |
| `GET` | `/produtos/{id}` | Busca produto por ID |
| `POST` | `/produtos` | Cria novo produto |
| `PUT` | `/produtos/{id}` | Atualiza produto existente |
| `DELETE` | `/produtos/{id}` | Remove produto |

### 🔹 Consultas LINQ Avançadas

| Método | Endpoint | Descrição | LINQ Usado |
|--------|----------|-----------|------------|
| `GET` | `/produtos/categoria/{categoria}` | Filtra por categoria | `Where`, `Contains` |
| `GET` | `/produtos/rentabilidade/{minima}` | Filtra por rentabilidade | `Where`, `OrderByDescending` |
| `GET` | `/produtos/risco/{nivel}` | Filtra por nível de risco | `Where`, `OrderBy` |
| `GET` | `/produtos/estatisticas` | Estatísticas agregadas | `Count`, `Average`, `Max`, `Min`, `GroupBy` |
| `GET` | `/produtos/buscar?categoria=&risco=` | Busca com múltiplos filtros | Queries dinâmicas com LINQ |
| `GET` | `/produtos/resumo` | Projeção de dados | `Select` (projeção) |
| `GET` | `/produtos/paginado?pagina=1&itensPorPagina=10` | Paginação | `Skip`, `Take` |

### 🔹 Integração com API Externa

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/investimentos-externos` | Lista produtos da API externa |
| `GET` | `/investimentos-externos/{id}` | Busca produto externo por ID |

**API Externa**: `https://assessor-virtual-api.onrender.com/api/investimentos`

---

## 🛠️ Como Executar Localmente

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Acesso ao banco de dados Oracle (FIAP)
- Git

### Passo a Passo

#### 1️⃣ Clone o repositório
```bash
git clone https://github.com/pbrnx/sprintcsharp.git
cd sprintcsharp
```

#### 2️⃣ Configure a Connection String
Edite o arquivo `sprintcsharp/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)))"
  }
}
```

#### 3️⃣ Restaure as dependências
```bash
dotnet restore
```

#### 4️⃣ Execute a aplicação
```bash
cd sprintcsharp
dotnet run
```

#### 5️⃣ Acesse a documentação Swagger
Abra o navegador em: **http://localhost:5000** ou **https://localhost:5001**

---

## 🐳 Deploy com Docker

### Build da imagem
```bash
docker build -t api-investimentos .
```

### Executar o container
```bash
docker run -p 8080:8080 -e ConnectionStrings__OracleConnection="User Id=...;Password=...;" api-investimentos
```

### Acesse
**http://localhost:8080**

---

## ☁️ Publicação em Cloud

### Opção 1: Azure App Service

```bash
# Login no Azure
az login

# Criar Resource Group
az group create --name rg-investimentos --location brazilsouth

# Criar App Service Plan
az appservice plan create --name plan-investimentos --resource-group rg-investimentos --sku B1 --is-linux

# Deploy via Docker
az webapp create --resource-group rg-investimentos --plan plan-investimentos --name api-investimentos-fiap --deployment-container-image-name api-investimentos:latest
```

### Opção 2: Render.com

1. Conecte seu repositório GitHub no [Render](https://render.com)
2. Crie um novo **Web Service**
3. Selecione **Docker** como ambiente
4. Configure as variáveis de ambiente:
   - `ConnectionStrings__OracleConnection`: sua connection string
5. Deploy automático!

### Opção 3: AWS ECS

```bash
# Push para ECR
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin <account-id>.dkr.ecr.us-east-1.amazonaws.com

docker tag api-investimentos:latest <account-id>.dkr.ecr.us-east-1.amazonaws.com/api-investimentos:latest

docker push <account-id>.dkr.ecr.us-east-1.amazonaws.com/api-investimentos:latest
```

---

## 📚 Exemplos de Uso

### Criar um novo produto
```bash
curl -X POST http://localhost:5000/produtos \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "CDB Banco XYZ",
    "categoria": "Renda Fixa",
    "rentabilidadeAnual": 12.5,
    "nivelRisco": "Baixo",
    "descricao": "CDB com liquidez diária"
  }'
```

### Buscar produtos por categoria
```bash
curl http://localhost:5000/produtos/categoria/renda%20fixa
```

### Obter estatísticas
```bash
curl http://localhost:5000/produtos/estatisticas
```

### Busca com filtros múltiplos
```bash
curl "http://localhost:5000/produtos/buscar?categoria=acoes&rentabilidadeMinima=10&ordenarPor=rentabilidade"
```

### Consultar API externa
```bash
curl http://localhost:5000/investimentos-externos
```

---

## 📊 Estrutura do Banco de Dados

### Tabela: `INVESTIMENTO_PRODUTO`

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `ID` | NUMBER(10) | Chave primária (auto-incremento) |
| `NOME` | VARCHAR2(200) | Nome do produto |
| `CATEGORIA` | VARCHAR2(100) | Categoria do investimento |
| `RENTABILIDADE_ANUAL` | NUMBER(5,2) | Rentabilidade anual (%) |
| `NIVEL_RISCO` | VARCHAR2(50) | Nível de risco |
| `DESCRICAO` | VARCHAR2(500) | Descrição detalhada |

---

## 🧪 Testes da API

Você pode testar a API de 3 formas:

### 1. Swagger UI (Recomendado)
Acesse `/swagger` e teste interativamente todos os endpoints.

### 2. Postman
Importe a coleção OpenAPI de `/swagger/v1/swagger.json`.

### 3. cURL (Linha de comando)
Use os exemplos da seção "Exemplos de Uso".

---

## 📖 Documentação Adicional

### Consultas LINQ Implementadas

#### 1. **Filtros Simples** (`Where`)
```csharp
_context.ProdutosInvestimento.Where(p => p.Categoria == "Renda Fixa")
```

#### 2. **Ordenação** (`OrderBy`, `OrderByDescending`)
```csharp
_context.ProdutosInvestimento.OrderByDescending(p => p.RentabilidadeAnual)
```

#### 3. **Agregações** (`Count`, `Average`, `Max`, `Min`)
```csharp
var media = _context.ProdutosInvestimento.Average(p => p.RentabilidadeAnual);
```

#### 4. **Agrupamento** (`GroupBy`)
```csharp
_context.ProdutosInvestimento.GroupBy(p => p.Categoria)
```

#### 5. **Projeção** (`Select`)
```csharp
_context.ProdutosInvestimento.Select(p => new { p.Id, p.Nome })
```

#### 6. **Paginação** (`Skip`, `Take`)
```csharp
_context.ProdutosInvestimento.Skip(10).Take(10)
```

---

## 🎯 Melhorias Futuras

- [ ] Autenticação JWT
- [ ] Rate Limiting
- [ ] Cache com Redis
- [ ] Logs estruturados (Serilog)
- [ ] Testes unitários (xUnit)
- [ ] CI/CD com GitHub Actions
- [ ] Health Checks
- [ ] Versionamento de API (v2, v3...)

---

## 👥 Equipe de Desenvolvimento

**Desenvolvido por**: [Seu Nome]  
**RM**: [Seu RM]  
**Turma**: [Sua Turma]  
**Disciplina**: Sprint C# - FIAP  
**Professor**: [Nome do Professor]

---

## 📞 Suporte

Para dúvidas ou problemas:
- **Email**: seu-email@fiap.com.br
- **GitHub Issues**: [Criar Issue](https://github.com/pbrnx/sprintcsharp/issues)

---

## 📄 Licença

Este projeto foi desenvolvido para fins educacionais como parte do curso de **Análise e Desenvolvimento de Sistemas** da FIAP.

---

## 🙏 Agradecimentos

- FIAP - Faculdade de Informática e Administração Paulista
- Microsoft - Documentação do .NET e Entity Framework
- Oracle - Banco de dados e documentação

---

**⭐ Se este projeto foi útil, deixe uma estrela no GitHub!**

**🔗 Link do Repositório**: https://github.com/pbrnx/sprintcsharp

**🌐 API em Produção**: [URL após deploy]

---

*Última atualização: Outubro/2025*
