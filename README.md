# 🏦 WlConsultings Bank Challenge - API de Gerenciamento de Carteiras Digitais

Este projeto é uma API desenvolvida em C# com .NET 8 para gerenciar carteiras digitais e transações financeiras. A API segue os princípios REST e utiliza autenticação JWT para garantir a segurança das operações.

## 📋 Requisitos do Projeto

- **Linguagem**: C#
- **Framework**: .NET 8
- **Banco de Dados**: PostgreSQL
- **Autenticação**: JWT (JSON Web Token)
- **Funcionalidades**:
  - Criar usuário
  - Consultar saldo da carteira
  - Adicionar saldo à carteira
  - Criar transferências entre usuários
  - Listar transferências com filtro por período

## 🚀 Como Executar o Projeto

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/) (opcional, para rodar o banco de dados em um contêiner)

### Passos para Execução

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/seu-usuario/wlconsultings-bank-challenge.git
   cd wlconsultings-bank-challenge

2 **Configuracao do Banco de Dados**:
- Crie um banco de dados PostgreSQL chamado BankChallenge.
- Atualize a string de conexão no arquivo appsettings.json na pasta src/WlConsultings.BankChallenge.WebApi
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=BankChallenge;Username=seu_usuario;Password=sua_senha"
}
```
- Execute as migrations 
```bash
dotnet tool install --global dotnet-ef
dotnet ef database update --startup-project src/WlConsultings.BankChallenge.WebApi --project src/WlConsultings.BankChallenge.Infra
```
3. **Executar Projeto**
```bash
dotnet run --project src/WlConsultings.BankChallenge.WebApi
```

## 🐳 Executando com Docker
### Passo 1:
```bash
docker compose up -d
```
### Passo 2:
- Acessar **http://localhost:8080/swagger/index.html**