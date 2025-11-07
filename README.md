# Financeira API  
API de gestão financeira desenvolvida em .NET 8, com suporte a PostgreSQL, Docker, autenticação JWT.

## 📌 Visão geral  
O Financeira API foi desenvolvido para gerenciamento de contratos e pagamentos:
- Cadastro, consulta e resumo de contratos.  
- Gestão de pagamentos vinculados a contratos, cálculo automático de status (em dia, antecipado, atrasado).  
- Resumo por cliente com indicadores (contratos ativos, parcelas pagas, parcelas em atraso, saldo devedor).  
- Integração com banco de dados PostgreSQL.  
- Containerização via Docker e Docker Compose para facilitar deploy local ou em produção.  
- Autenticação simples via token JWT fixo.
- Código com logging e correlation ID.

## 🔧 Tecnologias utilizadas  
- .NET 8 (ASP.NET Core) — back‑end web API.  
- Entity Framework Core — ORM para acesso ao PostgreSQL.  
- Npgsql — provedor PostgreSQL para .NET.  
- Docker & Docker Compose — para containerização do serviço e banco de dados.  
- PostgreSQL 15 — banco de dados relacional.  
- Serilog (opcional) — logging estruturado em arquivo e console.  
- JWT Bearer Authentication — autenticação HTTP baseada em token.  
- C# 11, record types, DI (Injeção de Dependência) e padrões modernos.

## 🏗 Estrutura do projeto  
```
/Financeira.sln
/Financeira/
├── Controllers/
│   ├── ContratoController.cs
│   ├── PagamentoController.cs
│   └── ClienteController.cs
├── DTO/
│   ├── ContratoDTO.cs
│   ├── PagamentoDTO.cs
│   └── ResumoClienteDTO.cs
├── Data/
│   └── AppDbContext.cs
├── Mappers/
│   ├── ContratoMapper.cs
│   ├── PagamentoMapper.cs
│   └── ClienteMapper.cs
├── Model/
│   ├── Contrato.cs
│   ├── Pagamento.cs
│   ├── Enums/
│   │   └── StatusPagamento.cs
├── Repository/
│   ├── IContratoRepository.cs
│   ├── IPagamentoRepository.cs
│   ├── ContratoRepository.cs
│   └── PagamentoRepository.cs
├── Service/
│   ├── IContratoService.cs
│   ├── IPagamentoService.cs
│   ├── IClienteService.cs
│   ├── ContratoService.cs
│   ├── PagamentoService.cs
│   └── ClienteService.cs
├── Util/
│   └── UriHelper.cs
├── Startup / Program.cs
├── appsettings.json
└── Dockerfile & docker‑compose.yml
```

## 🚀 Como rodar localmente  
### Pré‑requisitos  
- Docker Desktop instalado (Windows, Mac) ou Docker + Docker Compose (Linux).  
- .NET 8 SDK (somente para desenvolvimento; não necessário em produção se usar imagem docker).  
- PostgreSQL (pode usar container via compose ou banco local).

## Rodando a Aplicação Localmente (localhost:8080)

### 1. Criar a network Docker
```bash
docker network create financeira-network
```

### 2. Subir o container do PostgreSQL
```bash
docker run --name financeiradb -p 5432:5432 \
-e POSTGRES_USER=postgres \
-e POSTGRES_PASSWORD=postgres \
-e POSTGRES_DB=financeiradb \
--network financeira-network -d postgres:16.3
```

### 3. Subir o container do PgAdmin
```bash
docker run --name pgadmin4 -p 15432:80 \
-e PGADMIN_DEFAULT_EMAIL=admin@admin.com \
-e PGADMIN_DEFAULT_PASSWORD=admin \
--network financeira-network -d dpage/pgadmin4
```

> PgAdmin disponível em [http://localhost:15432](http://localhost:15432)

### 4. Configuração da aplicação
Substituir o conteúdo do arquivo `appsettings.json` pelo `appsettings-local.json` para usar a configuração correta da porta e do banco de dados.

### 5. Executar a aplicação
No Visual Studio 2022 ou via CLI:

### Usando Docker Compose  
```bash
git clone https://github.com/bpmachado/financeira.git
cd financeira
docker-compose up --build -d
```
> Isso vai subir três serviços: banco PostgreSQL, pgAdmin (interface gráfica) e a API Financeira.  
> A API ficará acessível por padrão em `http://localhost:8080`.

### Usando apenas Dockerfile da API  
Se você já tiver o banco rodando em outro local:  
```bash
docker build -t financeira-api .
docker run -d -p 8080:8080 \
  -e DATASOURCE_URL="Host=host.docker.internal;Port=5432;Database=financeiradb;Username=postgres;Password=postgres" \
  financeira-api
```
> Ajuste a variável `DATASOURCE_URL` conforme seu ambiente.

### Acessando o banco via pgAdmin  
Se estiver usando o docker‑compose, acesse `http://localhost:8081`, faça login com:  
- Usuário: `admin@admin.com`  
- Senha: `admin`  
Conecte ao servidor `financeiradb` na porta `5432`.

## 🔐 Autenticação JWT (token fixo)  
Para rotas protegidas, utilize o header HTTP:  
```
Authorization: Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1c3VhcmlvLWZpeG8iLCJyb2xlIjoiYWRtaW4iLCJleHAiOjQwMDAwMDAwMDB9.fJ_QemGTuq69W2yocgC7qrSZwL6EXmoq9zGN2NWU3S0
```
Depois da autenticação, acesse uma rota protegida como:  
```
GET http://localhost:8080/api/v1/contratos/{id}
```
Se a autenticação estiver correta você receberá o resultado esperado, caso contrário erro 401 ou 403.

# 📝 Postman Collection

Para testar a API Financeira rapidamente, você pode importar a **Postman Collection** disponibilizada neste repositório.

## Download da Collection

Clique no link abaixo para baixar a collection diretamente:

[Download da Collection](https://raw.githubusercontent.com/bpmachado/financeira/main/REST%20API%20-%20Financeira.postman_collection.json)

## Como Importar no Postman

1. Abra o Postman.
2. Clique em **File → Import** ou no botão **Import** na tela inicial.
3. Selecione a aba **Link** e cole o link de download:
   ```
   https://raw.githubusercontent.com/bpmachado/financeira/main/REST%20API%20-%20Financeira.postman_collection.json
   ```
4. Clique em **Continue** e depois em **Import**.
5. A collection será adicionada ao seu workspace, permitindo testar todos os endpoints da API.

## 📚 Documentação da API

A documentação interativa da API está disponível via Swagger.  
Para acessar, rode a aplicação e abra o link abaixo no seu navegador:

[Swagger UI](https://localhost:7218/swagger/index.html)

## 📚 Endpoints principais  
### Contratos  
- `GET /api/v1/contratos` → lista todos os contratos (ou conforme filtros).  
- `GET /api/v1/contratos/{id}` → obtém detalhes do contrato.  
- `POST /api/v1/contratos` → cria novo contrato.  
- `PUT /api/v1/contratos/{id}` → atualiza contrato existente.  
- `DELETE /api/v1/contratos/{id}` → exclui contrato (e pagamentos relacionados).  

### Pagamentos  
- `GET /api/v1/contratos/{id}/pagamentos` → lista pagamentos do contrato.  
- `POST /api/v1/contratos/{id}/pagamentos` → registra novo pagamento para contrato.  
- `GET /api/v1/contratos/{id}/pagamentos/{pagamentoId}` → obtém detalhe de pagamento.  

### Clientes  
- `GET /api/v1/clientes/{cpfCnpj}/resumo` → retorna resumo financeiro do cliente (contratos ativos, saldo, indicadores). 

## 🧪 Qualidade de código & boas práticas  
- Uso de **DTOs** (`record` types) para transporte entre camada de serviço e API.  
- Mappers dedicados para conversão entre entidades e DTOs.  
- Camadas claramente separadas: Controller → Service → Repository → DbContext.  
- Injeção de dependência configurada no `Program.cs` — facilita testes e substituição de implementações.  
- Conexão configurável via variáveis de ambiente (`DATASOURCE_URL`, `ASPNETCORE_URLS`).  
- Multistage Dockerfile para construção e execução eficiente da imagem.  
- `.gitignore` configurado para ignorar diretórios de build (bin, obj) e logs.  
- Logging acionado com correlation ID e gravação em arquivo (quando usado Serilog).  

## 📂 Docker & Infraestrutura  
- `Dockerfile` construído em duas etapas (SDK build + runtime).  
- `docker-compose.yml` incluído para levantar a aplicação, banco e gerenciador de banco (pgAdmin).  
- Rede Docker isolada (`financeira-network`) e volume PostgreSQL persistente (`postgres_data`).  
- Healthcheck para PostgreSQL: garante que o serviço está pronto antes da aplicação tentar se conectar.  

## 🧑‍💻 Autor & Contato  
- Bruno Machado — desenvolvedor principal deste repositório.  
- GitHub: [https://github.com/bpmachado](https://github.com/bpmachado)  
- LinkedIn / E‑mail: [LinkedIn](https://www.linkedin.com/in/bruno-pereira-machado-46b34b18b/) / bpmachado@gmail.com
