
# ApplicationProduct

## Descrição
Projeto desenvolvido para gerenciar produtos, com arquitetura limpa (Clean Architecture) e utilizando .NET.

## Estrutura do Projeto
O projeto segue o padrão de camadas de uma arquitetura limpa:

- **Domain**: Contém as entidades e regras de negócios.
- **Infrastructure**: Contém implementações específicas de infraestrutura, como acesso a banco de dados.
- **Application**: Contém os casos de uso e interfaces para os serviços.
- **WebApi**: Camada de apresentação (API) para expor os endpoints.

## Tecnologias Utilizadas
- .NET
- Entity Framework Core
- Clean Architecture
- SQLite 
- Swagger

## Pré-requisitos
Certifique-se de ter os seguintes itens instalados no seu ambiente:

- .NET SDK (versão compatível com o projeto(.net core 8)
- SQLITE

## Configuração do Projeto
1. Clone o repositório:
   ```bash
   git clone https://github.com/NickSan123/ApplicationProduct/
   ```

2. Navegue até a pasta principal do projeto:
   ```bash
   cd ApplicationProduct
   ```

3. Restaure as dependências do projeto:
   ```bash
   dotnet restore
   ```

4. Configure a string de conexão com o banco de dados no arquivo `appsettings.json` do projeto **ApplicationProduct.WebApi**.

## Executando as Migrations
As migrations estão localizadas no projeto **ApplicationProduct.WebApi**. Para aplicá-las, siga os passos:

1. Abra o terminal na pasta **ApplicationProduct.WebApi**.

2. Execute o comando para aplicar as migrations ao banco de dados:
   ```bash
   cd WebApi
   ```
   ```bash
   dotnet ef database update
   ```

Se preferir criar uma nova migration, utilize o comando:
   ```bash
   dotnet ef migrations add <NomeDaMigration>
   ```

## Executando o Projeto
1. Navegue até o diretório do projeto **ApplicationProduct.WebApi**.

2. Execute o comando:
   ```bash
   dotnet run
   ```

3. Acesse a API via navegador ou ferramenta como Postman/Insomnia no endereço:
   ```
   http://localhost:<porta>/swagger
   ```

## Testes
Para executar os testes do projeto, use o comando:
```bash
dotnet test
```

## Contribuição
Sinta-se à vontade para contribuir com melhorias, corrigir bugs ou adicionar novos recursos. Envie um pull request para análise.

## Licença
Este projeto está licenciado sob a licença MIT. Veja o arquivo `LICENSE` para mais informações.
