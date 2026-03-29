# Developer Evaluation Project

API de avaliação técnica implementada em `.NET 8` com foco no domínio de `sales`, seguindo arquitetura em camadas, `DDD`, `MediatR`, `EF Core` e `PostgreSQL`.

## Resumo

O projeto expõe uma API REST para gerenciamento completo de vendas, incluindo:

- criação de venda
- consulta por `id`
- listagem paginada com filtros e ordenação
- atualização de dados e itens
- cancelamento de venda
- reativação de venda
- exclusão física

Além do CRUD, a implementação contempla:

- regras de desconto por quantidade
- validação com `FluentValidation`
- persistência com `EF Core` e `Npgsql`
- `Unit of Work`
- publicação de eventos de domínio com `MediatR`
- tratamento centralizado de erros
- testes unitários e funcionais via HTTP

## Stack

- `.NET 8`
- `ASP.NET Core Web API`
- `Entity Framework Core`
- `PostgreSQL`
- `MediatR`
- `AutoMapper`
- `FluentValidation`
- `Serilog`
- `xUnit`
- `NSubstitute`
- `FluentAssertions`
- `Testcontainers` para testes funcionais com banco real

## Arquitetura

O projeto está organizado em camadas:

- [`src/Ambev.DeveloperEvaluation.Domain`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.Domain): entidades, eventos, regras de negócio e contratos
- [`src/Ambev.DeveloperEvaluation.Application`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.Application): casos de uso, handlers, validators e handlers de evento
- [`src/Ambev.DeveloperEvaluation.ORM`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.ORM): `DbContext`, mappings, repositórios, `UnitOfWork` e interceptor de eventos
- [`src/Ambev.DeveloperEvaluation.IoC`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.IoC): composição de dependências
- [`src/Ambev.DeveloperEvaluation.WebApi`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.WebApi): controllers, contratos HTTP, middlewares e bootstrap
- [`tests/Ambev.DeveloperEvaluation.Unit`](/D:/projetos/challenge/tests/Ambev.DeveloperEvaluation.Unit): testes unitários
- [`tests/Ambev.DeveloperEvaluation.Functional`](/D:/projetos/challenge/tests/Ambev.DeveloperEvaluation.Functional): testes funcionais via HTTP com banco real em container

## Funcionalidades de Sales

O recurso `sales` trabalha com snapshots desnormalizados de:

- cliente
- filial
- produtos

Cada venda armazena:

- `saleNumber`
- `saleDate`
- `customerExternalId`
- `customerName`
- `branchExternalId`
- `branchName`
- `totalAmount`
- `isCancelled`
- itens com produto, quantidade, preço unitário, desconto e total

### Regras de negócio

- compras com menos de `4` unidades do mesmo item não recebem desconto
- compras entre `4` e `9` unidades recebem `10%`
- compras entre `10` e `20` unidades recebem `20%`
- não é permitido vender mais de `20` unidades idênticas
- item cancelado deixa de contribuir para o total da venda
- venda cancelada zera o total e cancela os itens
- venda reativada restaura os itens e recalcula o total

## Rotas

Base URL local:

```text
http://localhost:8080
```

Rotas de `sales`:

- `POST /api/sales`
- `GET /api/sales`
- `GET /api/sales/{id}`
- `PUT /api/sales/{id}`
- `POST /api/sales/{id}/cancel`
- `POST /api/sales/{id}/activate`
- `DELETE /api/sales/{id}`

### Exemplo de criação

```json
{
  "saleDate": "2026-03-29T00:00:00Z",
  "customerExternalId": "customer-123",
  "customerName": "John Doe",
  "branchExternalId": "branch-001",
  "branchName": "Main Branch",
  "items": [
    {
      "productExternalId": "product-1",
      "productName": "Product One",
      "quantity": 4,
      "unitPrice": 10.0
    }
  ]
}
```

### Query params da listagem

`GET /api/sales` suporta:

- `_page`
- `_size`
- `_order`
- `saleNumber`
- `customerName`
- `branchName`
- `isCancelled`
- `saleDateFrom`
- `saleDateTo`

Valores aceitos em `_order`:

- `saleDate_desc`
- `saleDate_asc`
- `saleNumber_desc`
- `saleNumber_asc`
- `customerName_desc`
- `customerName_asc`
- `branchName_desc`
- `branchName_asc`
- `totalAmount_desc`
- `totalAmount_asc`

## Eventos

Os eventos de domínio implementados para `sales` são:

- `SaleCreated`
- `SaleModified`
- `SaleCancelled`
- `ItemCancelled`

Fluxo atual:

- as entidades acumulam eventos de domínio
- o `EF Core` publica esses eventos via interceptor após `SaveChanges`
- os handlers `INotificationHandler<>` apenas registram logs estruturados

Arquivos principais:

- [`PublishDomainEventsInterceptor.cs`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.ORM/Interceptors/PublishDomainEventsInterceptor.cs)
- [`SaleCreatedEventHandler.cs`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.Application/Sales/EventHandlers/SaleCreatedEventHandler.cs)
- [`SaleModifiedEventHandler.cs`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.Application/Sales/EventHandlers/SaleModifiedEventHandler.cs)
- [`SaleCancelledEventHandler.cs`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.Application/Sales/EventHandlers/SaleCancelledEventHandler.cs)
- [`ItemCancelledEventHandler.cs`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.Application/Sales/EventHandlers/ItemCancelledEventHandler.cs)

## Banco de dados e migrations

O banco principal usado pelo projeto é `PostgreSQL`.

A aplicação está configurada para:

- tentar aplicar migrations automaticamente no startup
- aplicar apenas quando existirem migrations pendentes
- continuar executando mesmo que a tentativa de migration falhe

Configuração principal:

- [`InfrastructureModuleInitializer.cs`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.IoC/ModuleInitializers/InfrastructureModuleInitializer.cs)
- [`Program.cs`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.WebApi/Program.cs)

## Como executar

### Pré-requisitos

- `.NET SDK 8`
- `Docker` e `Docker Compose` para execução containerizada

### Executando com Docker

Suba a API e o PostgreSQL:

```powershell
docker compose up --build
```

Serviços:

- API: `http://localhost:8080`
- PostgreSQL: `localhost:5432`

### Executando localmente

Primeiro, garanta um PostgreSQL acessível com uma connection string compatível.

Connection string padrão em desenvolvimento:

```text
Host=localhost;Port=5432;Database=DeveloperEvaluation;Username=admin;Password=Dev@123456
```

Depois execute:

```powershell
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj
```

Se estiver em `Development`, o Swagger ficará disponível pela própria WebApi.

## Testes

### Testes unitários

```powershell
dotnet test tests/Ambev.DeveloperEvaluation.Unit/Ambev.DeveloperEvaluation.Unit.csproj
```

Cobrem:

- regras de domínio de `sales`
- validators
- handlers de aplicação
- middlewares
- handlers de evento

### Testes funcionais

```powershell
dotnet test tests/Ambev.DeveloperEvaluation.Functional/Ambev.DeveloperEvaluation.Functional.csproj
```

Cobrem via HTTP:

- criação
- consulta por id
- `404` no `GET`
- listagem
- atualização
- cancelamento
- reativação
- exclusão

Os testes funcionais usam `Testcontainers` para subir um PostgreSQL real durante a execução.

## Observações

- o projeto de [`tests/Ambev.DeveloperEvaluation.Integration`](/D:/projetos/challenge/tests/Ambev.DeveloperEvaluation.Integration/Ambev.DeveloperEvaluation.Integration.csproj) existe, mas no estado atual não possui cenários ativos
- o projeto possui um warning conhecido de vulnerabilidade no `AutoMapper 13.0.1`
- existe também um warning de nulabilidade em [`JwtTokenGenerator.cs`](/D:/projetos/challenge/src/Ambev.DeveloperEvaluation.Common/Security/JwtTokenGenerator.cs)

## Documentação complementar

- [`Overview`](/D:/projetos/challenge/.doc/overview.md)
- [`Tech Stack`](/D:/projetos/challenge/.doc/tech-stack.md)
- [`Frameworks`](/D:/projetos/challenge/.doc/frameworks.md)
- [`Project Structure`](/D:/projetos/challenge/.doc/project-structure.md)
