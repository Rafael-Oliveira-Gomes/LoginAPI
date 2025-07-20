# LoginAPI

API RESTful para autenticação e gerenciamento de usuários, utilizando ASP.NET Core 9, Entity Framework Core e autenticação JWT. O projeto segue uma arquitetura em camadas, separando responsabilidades entre API, Application, Domain e Repository.

## Sumário
- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Configuração](#configuração)
- [Execução](#execução)
- [Endpoints Principais](#endpoints-principais)
- [Modelos de Dados](#modelos-de-dados)
- [Autenticação e Segurança](#autenticação-e-segurança)
- [Swagger](#swagger)

---

## Visão Geral
Esta API permite o cadastro, autenticação (login) e consulta de usuários, protegendo os endpoints com JWT. Ideal para servir como backend de autenticação para aplicações web/mobile.

## Arquitetura
O projeto está dividido em quatro camadas principais:
- **Login.API**: Camada de apresentação (controllers, middlewares, configuração de serviços).
- **Login.Application**: Regras de negócio e serviços de aplicação.
- **Login.Domain**: Entidades, DTOs e interfaces de domínio.
- **Login.Repository**: Persistência de dados e contexto do banco (PostgreSQL via Entity Framework Core).

## Tecnologias Utilizadas
- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9
- PostgreSQL
- Microsoft Identity
- JWT (JSON Web Token)
- Swashbuckle (Swagger)

## Configuração
1. **Banco de Dados**: Certifique-se de ter um PostgreSQL rodando. O padrão de conexão está em `Login/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "PostgresConnection": "Host=localhost;Port=5432;Database=login;Username=postgres;Password=root"
   }
   ```
   Ajuste conforme necessário.

2. **JWT**: As configurações do token JWT também estão em `appsettings.json`:
   ```json
   "JWT": {
     "ValidAudience": "http://localhost:4200",
     "ValidIssuer": "http://localhost:44369",
     "Secret": "fedaf7d8863b48e197b9287d492b708e"
   }
   ```

3. **Migrations**: O projeto já possui migrations para criação das tabelas. Para aplicar:
   ```bash
   dotnet ef database update --project Login.Repository
   ```

## Execução
No diretório do projeto, execute:
```bash
cd Login

dotnet run
```
A API estará disponível em: `http://localhost:5000/login-api` (ajustável via Program.cs).

## Endpoints Principais

### Autenticação
- `POST /api/auth/sign-up` — Cadastro de novo usuário
  - Body: `{ "username": "string", "email": "string", "password": "string", "passwordConfirm": "string" }`
- `POST /api/auth/sign-in` — Login
  - Body: `{ "email": "string", "password": "string" }`
  - Resposta: `{ "access_token": "string", "expiration": "datetime" }`
- `GET /api/auth/get-current-user` — Dados do usuário autenticado (JWT obrigatório)

## Modelos de Dados

### SignUpDTO
```json
{
  "username": "string",
  "email": "string",
  "password": "string",
  "passwordConfirm": "string"
}
```

### SignInDTO
```json
{
  "email": "string",
  "password": "string"
}
```

### SsoDTO (Resposta do login)
```json
{
  "access_token": "string",
  "expiration": "datetime"
}
```

### ApplicationUser
Herdado de `IdentityUser` do ASP.NET Core Identity.

## Autenticação e Segurança
- **JWT**: Todos os endpoints (exceto login/cadastro) exigem autenticação via Bearer Token.
- **CORS**: Permitido apenas para `http://localhost:4200` (frontend padrão Angular).
- **Senhas**: Armazenadas de forma segura via Identity.

## Swagger
A documentação interativa está disponível em `/login-api/index.html` após rodar a aplicação.

---

## Estrutura de Pastas
```
LoginAPI/
  Login/                # API principal (controllers, config, startup)
  Login.Application/    # Serviços e regras de negócio
  Login.Domain/         # Entidades, DTOs, interfaces
  Login.Repository/     # Persistência e contexto do banco
```

## Contribuição
Pull requests são bem-vindos! Abra uma issue para discutir mudanças.

## Licença
[MIT](LICENSE) 