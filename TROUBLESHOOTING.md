# 🔧 Guia de Troubleshooting - Docker Internal Server Error

## 📋 Diagnóstico Rápido

Execute o script de diagnóstico:

```cmd
cd C:\Users\labsfiap\Desktop\sprintcsharp
docker-diagnostico.bat
```

---

## 🔍 Causas Comuns do Erro 500

### **1. Banco de dados Oracle inacessível**

O container Docker não consegue acessar `oracle.fiap.com.br` porque:
- Está em uma rede isolada
- Firewall bloqueando
- DNS não resolve

**Solução:**

```cmd
REM Testar se resolve DNS de dentro do container
docker exec -it sprintcsharp_test ping oracle.fiap.com.br

REM Se não resolver, use --network=host (apenas Linux)
docker run --rm -p 5000:8080 --network=host pbrnx/sprint-csharp:latest
```

### **2. Connection String incorreta**

Verifique os logs:

```cmd
docker logs sprintcsharp_test | findstr "ORA-"
docker logs sprintcsharp_test | findstr "Connection"
```

**Erros comuns:**
- `ORA-12154` - TNS: could not resolve service name
- `ORA-12514` - TNS: listener does not know of service
- `ORA-01017` - invalid username/password

### **3. Tabela não existe**

```cmd
docker logs sprintcsharp_test | findstr "ORA-00942"
```

**Solução:** Verifique se a tabela `INVESTIMENTO_PRODUTO` existe no schema do usuário RM99781.

---

## ✅ Passos de Solução

### **Passo 1: Rebuild com logs habilitados**

```cmd
cd C:\Users\labsfiap\Desktop\sprintcsharp
docker build -t pbrnx/sprint-csharp:latest .
```

### **Passo 2: Rodar container com logs detalhados**

```cmd
docker run --rm -p 5000:8080 --name sprintcsharp_test pbrnx/sprint-csharp:latest
```

**Aguarde a mensagem:**
```
[INFO] Aplicação iniciando...
[INFO] Ambiente: Production
Now listening on: http://[::]:8080
```

### **Passo 3: Testar endpoints**

Em **OUTRO terminal**, execute:

```cmd
REM Testar raiz (redireciona para Swagger)
curl http://localhost:5000

REM Testar Swagger
curl http://localhost:5000/swagger

REM Testar endpoint de produtos
curl http://localhost:5000/produtos

REM Se der erro, ver resposta completa
curl -v http://localhost:5000/produtos
```

### **Passo 4: Ver logs em tempo real**

```cmd
docker logs -f sprintcsharp_test
```

Procure por:
- `[ERRO]` - Mensagens de erro
- `ORA-` - Erros do Oracle
- `Exception` - Stack traces

---

## 🐛 Debug Avançado

### **Entrar no container**

```cmd
docker exec -it sprintcsharp_test /bin/bash
```

Dentro do container:

```bash
# Ver arquivos
ls -la

# Ver appsettings.json
cat appsettings.json

# Testar endpoint internamente
curl http://localhost:8080/produtos

# Ver variáveis de ambiente
env | grep Connection
```

### **Testar conexão Oracle de fora do container**

```cmd
REM Instale sqlplus ou SQL Developer e teste:
sqlplus RM99781/270904@oracle.fiap.com.br:1521/ORCL

SQL> SELECT COUNT(*) FROM INVESTIMENTO_PRODUTO;
```

Se não funcionar, o problema é de rede/firewall.

---

## 🌐 Alternativa: Rodar Localmente (Sem Docker)

Se o Docker continuar com problemas de rede:

```cmd
cd C:\Users\labsfiap\Desktop\sprintcsharp\sprintcsharp
dotnet run
```

Acesse: http://localhost:5000

**Isso funciona?**
- ✅ **SIM** → Problema é de rede no Docker
- ❌ **NÃO** → Problema é na connection string ou banco

---

## 🔧 Soluções Específicas

### **Solução 1: Usar IP em vez de hostname**

Se `oracle.fiap.com.br` não resolver:

```cmd
REM Descubra o IP
nslookup oracle.fiap.com.br

REM Use o IP na connection string
docker run --rm -p 5000:8080 ^
  -e ConnectionStrings__OracleConnection="User Id=RM99781;Password=270904;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=200.144.28.150)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)))" ^
  pbrnx/sprint-csharp:latest
```

### **Solução 2: Modo host network (Linux/Mac)**

```bash
docker run --rm --network=host pbrnx/sprint-csharp:latest
```

### **Solução 3: Adicionar DNS customizado**

```cmd
docker run --rm -p 5000:8080 ^
  --dns 8.8.8.8 ^
  --dns 8.8.4.4 ^
  pbrnx/sprint-csharp:latest
```

### **Solução 4: Testar com banco mock (sem Oracle)**

Adicione endpoint de teste no `Program.cs`:

```csharp
app.MapGet("/health", () => Results.Ok(new { 
    status = "OK", 
    timestamp = DateTime.Now 
}));
```

Se `/health` funcionar mas `/produtos` não, confirma que é problema do Oracle.

---

## 📊 Checklist de Verificação

- [ ] Container está rodando: `docker ps`
- [ ] Logs não mostram erros: `docker logs sprintcsharp_test`
- [ ] Porta 5000 está livre: `netstat -ano | findstr :5000`
- [ ] Oracle está acessível: `ping oracle.fiap.com.br`
- [ ] Connection string está correta
- [ ] Tabela INVESTIMENTO_PRODUTO existe
- [ ] Usuário RM99781 tem permissão na tabela
- [ ] Funciona localmente (sem Docker): `dotnet run`

---

## 🆘 Se Nada Funcionar

### **Opção 1: Deploy sem Docker**

Publique diretamente no IIS/Azure App Service sem container.

### **Opção 2: Usar SQLite para testes**

Troque o Oracle por SQLite temporariamente para validar a API.

### **Opção 3: Documentar o problema**

No README.md, adicione:

```markdown
## ⚠️ Limitações Conhecidas

- A API requer acesso ao Oracle da FIAP (`oracle.fiap.com.br`)
- Em ambientes Docker, pode haver problemas de rede/DNS
- Recomendado rodar localmente com `dotnet run` para testes
```

---

## 📞 Comandos Úteis

```cmd
REM Ver todos os containers
docker ps -a

REM Parar container
docker stop sprintcsharp_test

REM Remover container
docker rm sprintcsharp_test

REM Ver logs completos
docker logs sprintcsharp_test > logs.txt

REM Inspecionar container
docker inspect sprintcsharp_test

REM Ver uso de recursos
docker stats sprintcsharp_test

REM Limpar tudo
docker system prune -a
```

---

## 🎯 Próximos Passos

1. Execute `docker-diagnostico.bat`
2. Copie os logs e analise os erros
3. Teste localmente com `dotnet run`
4. Se funcionar localmente, o problema é de rede no Docker
5. Use uma das soluções alternativas acima

**Cole os logs aqui para análise mais detalhada!**
