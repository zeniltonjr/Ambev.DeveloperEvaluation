## Ambev.DeveloperEvaluation

### Visão Geral
Este projeto implementa os módulos de Produto (Product), Item de Venda (SaleItem) e Venda (Sale). Ele inclui CRUDs completos, validações de negócios e integração com a lógica de descontos baseada na quantidade de itens comprados.

### Estrutura do Projeto
O projeto segue uma arquitetura em camadas, separando responsabilidades em diferentes pastas:

Ambev.DeveloperEvaluation/<br>
├── /Adapters<br>
│   ├── /Driven (Infraestrutura da aplicação)<br>
│   │   ├── Ambev.DeveloperEvaluation.ORM<br>
│   │   │   ├── /Mapping/ → Mapeamento de entidades<br>
│   │   │   ├── /Migrations/ → Controle de versões do banco de dados<br>
│   │   │   ├── /Repositories/ → Implementação de repositórios para acesso aos dados<br>
│   │   │   ├── appsettings.json → Configuração do ambiente<br>
│   │   │   └── DefaultContext.cs → Configuração do Entity Framework Core<br>
│   │   ├── /Drivers (Interface de entrada, Web API)<br>
│   │   │   ├── Ambev.DeveloperEvaluation.WebApi<br>
│   │   │   │   ├── /Common/ → Utilitários comuns à API<br>
│   │   │   │   ├── /Features/ -> CRUDS<br>
│   │   │   │   │   ├── Auth → Autenticação de usuários<br>
│   │   │   │   │   ├── Branchs → Cadastro de filiais<br>
│   │   │   │   │   ├── Products → Cadastro de produtos<br>
│   │   │   │   │   ├── SaleItens → Cadastro de itens da venda<br>
│   │   │   │   │   ├── Sales → Registro de vendas<br>
│   │   │   │   │   └── Users → Cadastro de usuários<br>
│   │   │   │   ├── /Logs/ → Implementação de logs<br>
│   │   │   │   ├── /Mappings/ → Configuração do AutoMapper<br>
│   │   │   │   ├── /Middleware/ → Middleware para tratamento de erros e segurança<br>
│   │   │   │   ├── appsettings.json → Configuração da API<br>
│   │   │   │   ├── Dockerfile → Configuração para containerização<br>
│   │   │   │   └── Program.cs → Inicialização da API<br>
│   ├── /Core (Regra de Negócio)<br>
│   │   ├── /Application → Camada de aplicação com os casos de uso<br>
│   │   ├── /Domain → Contém as entidades e regras de negócio<br>
│   │   │   ├── /Common/ → Entidades base e utilitários<br>
│   │   │   ├── /Entities/ → Definição de objetos de domínio<br>
│   │   │   ├── /Enums/ → Enumerações utilizadas<br>
│   │   │   ├── /Events/ → Eventos de domínio<br>
│   │   │   ├── /Exceptions/ → Exceções personalizadas<br>
│   │   │   ├── /Repositories/ → Interfaces para repositórios<br>
│   │   │   ├── /Services/ → Serviços de domínio<br>
│   │   │   ├── /Specifications/ → Regras de validação<br>
│   │   │   └── /Validation/ → Validações de entidades<br>
│   ├── /Crosscutting (Módulos transversais)<br>
│   │   ├── /HealthChecks/ → Monitoramento da aplicação<br>
│   │   ├── /Logging/ → Configuração de logs<br>
│   │   ├── /Security/ → Segurança e autenticação<br>
│   │   ├── /Validation/ → Validações gerais<br>
│   │   └── /IoC/ → Injeção de dependências<br>
├── /Testes<br>
│   ├── /Functional/ → Testes funcionais<br>
│   ├── /Integration/ → Testes de integração<br>
│   ├── /Unit/ → Testes unitários<br>
└── README.md            # Documentação do projeto<br>

### Cadastro e Autenticação de Usuários
A autenticação no sistema segue um fluxo baseado em tokens JWT. O cadastro de usuários e a geração de tokens ocorrem conforme descrito abaixo:

Cadastro de Usuário

Requisição:
Endpoint: POST /api/users/register
```bash
{
  "username": "Aphex twin",
  "password": "@Fd123456789N",
  "phone": "11972802538",
  "email": "aphex@usp.com",
  "status": 1,
  "role": 2
}
```

Resposta:
```bash
{
  "data": {
    "id": "47edb46d-e560-4c76-9854-7850db7a44fa",
  },
  "success": true,
  "message": "User created successfully",
  "errors": {
    "$id": "3",
    "$values": []
  }
}
```

Autenticação de Usuário

Requisição:
Endpoint: POST /api/auth/login
```bash
{
  "email": "aphex@usp.com",
  "password": "@Fd123456789N",
}
```

Resposta:

```bash
{
  "data": {
          "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiSm9zw6kgMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYjFlM2FmYTgtMmM0My00YzZiLWFkYzMtNTk3YTk0MGI2MzdjIiwiZXhwIjoxNzQyODEzMzM4LCJpc3MiOiJSZXRpbmFTZXJ2aWNlIiwiYXVkIjoiTXlBcHBBbWJldiJ9.HsTwsyU3XeAKSEfinFz88wIO2kJZdF9G7ae2VtPt3WA",
            "email": "jose@hotmail.com",
            "name": "José 2",
            "role": "Customer"
        },
   "success": true,
   "message": "User authenticated successfully",
   "errors": {
       "$id": "4",
        "$values": []
    }
}
```

### Acesso a Endpoints Protegidos

Para acessar recursos protegidos, inclua o token JWT no cabeçalho das requisições:

curl -H "Authorization: Bearer jwt-gerado-aqui"

## Configuração do Banco de Dados 

Para rodar a aplicação usando Entity framework, siga os passos abaixo:

Acesse a pasta do projeto ORM:

```bash
cd Adapters/Driven/Infrastructure/Ambev.DeveloperEvaluation.ORM
```

Execute o seguinte comando para criar as migrações:

```bash
dotnet ef migrations add InitialMigration
```

Atualize o banco de dados:

```bash
dotnet ef database update
```

## Regras de Negócio

### Filial (Branch)
A entidade Branch representa uma filial da empresa. Cada filial tem um nome e pode ter múltiplos produtos associados. As regras de negócios para o Branch incluem:

* Identificação única: Cada filial é identificada de forma única por seu ID.

* Nome da filial: O nome da filial deve ser definido na criação da filial e pode ser alterado posteriormente.

* Produtos associados: Cada filial pode ter vários produtos cadastrados, sendo possível cadastrar, atualizar ou remover produtos da filial.

### Produto (Product)
* Cada produto tem um identificador único, um nome e um preço unitário. Os produtos podem ser cadastrados, atualizados e removidos do sistema.

### Item de Venda (SaleItem)
Os itens de venda possuem as seguintes regras:

* Cada item pertence a uma venda específica (SaleId).

* Um item tem um produto associado (ProductId).

* O preço unitário e a quantidade são definidos na criação do item.

* Aplicação automática de descontos:

* Acima de 10 unidades → 20% de desconto.

* Entre 4 e 9 unidades → 10% de desconto.

* Limite máximo → Não é permitido vender mais de 20 unidades do mesmo produto.

### Venda (Sale)

* Uma venda é composta por um número de venda, data, filial e cliente associado. Regras:

* Uma venda não pode ser cancelada após a conclusão.

* O total da venda é calculado com base nos itens incluídos.

* Apenas produtos disponíveis podem ser vendidos.

## Rodando a Aplicação

Para rodar a API localmente, siga os passos:

Compilar a aplicação

```bash
dotnet build
```

* Rodar a API

```bash
dotnet run --project Adapters/Drivers/WebApi/Ambev.DeveloperEvaluation.WebApi
```

* Rodar a aplicação via Docker

```bash
docker-compose up --build
```

A API estará disponível em http://localhost:5000.

## Testes
O projeto Ambev Developer Evaluation contém uma suíte de testes abrangente, dividida em testes unitários, testes de integração e testes funcionais para garantir o funcionamento correto das funcionalidades de cada módulo: Branch, Sale, SaleItem e Product. Os testes estão organizados em pastas correspondentes a cada módulo, conforme a estrutura do código.

## Testes Funcionais
Os testes funcionais validam o comportamento esperado dos módulos em cenários de ponta a ponta. Eles cobrem:

* Branch: Testes para o fluxo de criação e gerenciamento de filiais, incluindo a validação dos dados da filial e a resposta da API.

*  Sale: Testes para verificar a criação e o processamento de vendas, incluindo a validação de descontos, totais e a integridade da venda.

*  SaleItem: Testes para garantir a correta manipulação de itens de venda, incluindo regras de quantidade e aplicação de descontos.

* Product: Testes para verificar a criação, atualização e remoção de produtos, além da validação do preço e da quantidade disponível.

## Testes de Integração
Os testes de integração garantem que os diferentes módulos interagem corretamente com os repositórios, banco de dados e outros serviços externos. Eles incluem:

* Branch: Testes para garantir que os dados de filial sejam corretamente salvos e recuperados do banco de dados.

* Sale: Testes para validar o fluxo completo de uma venda, incluindo a integração com os itens de venda e o cálculo de descontos.

* SaleItem: Testes para garantir que os itens da venda sejam corretamente inseridos e que as regras de desconto sejam aplicadas.

* Product: Testes para validar a criação e atualização de produtos no banco de dados e a correta interação com o sistema de vendas.

## Testes Unitários
Os testes unitários são responsáveis por testar componentes individuais de cada módulo. Eles garantem que cada função, serviço ou classe execute corretamente sua lógica de negócio, isoladamente:

* Branch: Testes unitários para validar a lógica de regras e comportamentos específicos de uma filial.

* Sale: Testes para validar as regras de cálculo do total da venda, descontos e status de vendas.

* SaleItem: Testes unitários para a lógica de adição de itens à venda, aplicação de descontos e restrições de quantidade.

* Product: Testes unitários para a validação do cadastro de produtos e regras de preço e estoque.

Execução dos Testes
Para rodar os testes de cada categoria, execute os seguintes comandos na raiz do projeto:

Testes Funcionais:

```bash
dotnet test --filter "Category=Functional"
```

Testes de Integração:

```bash
dotnet test --filter "Category=Integration"
```

Testes Unitários:

```bash
dotnet test --filter "Category=Unit"
```

Certifique-se de que o banco de dados esteja configurado corretamente e que as migrações sejam aplicadas antes de rodar os testes de integração.



