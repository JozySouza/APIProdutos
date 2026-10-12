# Catálogo de Produtos

Aplicação educacional de cadastro de produtos, composta por uma API REST em ASP.NET Core, uma interface Angular e persistência em SQLite.

## Funcionalidades

- Listar todos os produtos.
- Buscar um produto por ID.
- Cadastrar produtos com nome, preço e quantidade.
- Editar os dados de um produto.
- Remover produtos.
- Conferir no SQLite os dados alterados pela interface.

## Tecnologias

- **API:** ASP.NET Core e .NET 10.
- **Persistência:** Entity Framework Core e SQLite.
- **Interface:** Angular standalone e TypeScript.
- **Testes do front-end:** Vitest.

## Estrutura

```text
.
├── Controllers/          # Endpoints HTTP
├── Data/                 # DbContext do Entity Framework Core
├── Migrations/           # Migrações para o SQLite
├── Models/               # Entidades da API
├── produtos-front/       # Aplicação Angular
│   └── src/app/
│       ├── models/       # Tipos do front-end
│       ├── services/     # Cliente HTTP para a API
│       ├── app.html      # Tela do catálogo
│       └── app.ts        # Estado e operações da tela
├── Program.cs            # Configuração da API, CORS e DI
└── APIProdutos.csproj
```

O back-end separa endpoints, lógica de negócio e persistência:

```text
ProdutosController → IProdutoService → IProdutosRepository → AppDBContext → SQLite
```

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js e npm](https://nodejs.org/)
- Opcional: [DBeaver](https://dbeaver.io/) para consultar o banco

O Angular CLI já é dependência local do projeto, portanto não é necessário instalá-lo globalmente.

## Executar localmente

Na raiz deste repositório, abra um terminal para a API:

```powershell
dotnet restore
dotnet run --launch-profile http
```

A API fica disponível em `http://localhost:5144`. No ambiente de desenvolvimento, a documentação Swagger está em [http://localhost:5144/swagger](http://localhost:5144/swagger) e o documento OpenAPI em [http://localhost:5144/openapi/v1.json](http://localhost:5144/openapi/v1.json).

Abra outro terminal, também na raiz, para o Angular:

```powershell
cd .\produtos-front
npm install
npm start
```

Acesse [http://localhost:4200](http://localhost:4200). O front-end consome `http://localhost:5144/api/Produtos`.

Para encerrar os servidores, pressione `Ctrl+C` no terminal correspondente. `npm install` só precisa ser repetido quando as dependências forem alteradas ou após um clone novo.

## SQLite

A conexão está configurada como `Data Source=produtos.db`. Execute a API a partir da raiz do repositório para que o banco seja criado nesse diretório.

O banco e seus arquivos auxiliares são ignorados pelo Git. Em um clone novo, aplique a migração inicial para criar o arquivo e a tabela:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.12
dotnet ef database update
```

Se `dotnet-ef` já estiver instalado, execute somente `dotnet ef database update`. Depois, inicie a API.

Para inspecionar os dados no DBeaver, conecte-se ao arquivo `produtos.db` na raiz deste repositório e execute:

```sql
SELECT * FROM Produtos ORDER BY Id;
```

A coluna `Id` usa SQLite `AUTOINCREMENT`. IDs removidos não são reutilizados, então lacunas na sequência são esperadas.

## API de produtos

Todos os endpoints usam o prefixo `/api/Produtos`.

| Método | Rota | Descrição | Respostas principais |
| --- | --- | --- | --- |
| `GET` | `/api/Produtos` | Lista os produtos | `200 OK` |
| `GET` | `/api/Produtos/{id}` | Busca por ID | `200 OK`, `400 Bad Request`, `404 Not Found` |
| `POST` | `/api/Produtos` | Cadastra um produto | `201 Created`, `400 Bad Request` |
| `PUT` | `/api/Produtos/{id}` | Atualiza os dados do produto | `200 OK`, `400 Bad Request`, `404 Not Found` |
| `DELETE` | `/api/Produtos/{id}` | Remove o produto | `204 No Content`, `400 Bad Request`, `404 Not Found` |

Exemplo de corpo JSON para `POST` e `PUT`:

```json
{
  "nome": "Lápis",
  "preco": 0.89,
  "quantidade": 40
}
```

O nome é obrigatório e limitado a 100 caracteres; o preço precisa ser maior que zero. A service também rejeita nomes duplicados. Uma busca por ID não altera os dados.

## CORS

A política `PermitirAngular`, em `Program.cs`, permite as origens `http://localhost:4200` e `http://localhost:4201`. Se o front-end for iniciado em outra origem, adicione-a à política e reinicie a API.

## Build e testes

Na raiz, compile a API:

```powershell
dotnet build
```

Compile e teste o front-end:

```powershell
cd .\produtos-front
npm test -- --watch=false
npm run build
```

O build de produção do Angular é gerado em `produtos-front/dist/produtos-front`.

## Licença

Este projeto é educacional. Inclua os termos de licença desejados antes de redistribuí-lo.
