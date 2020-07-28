# Documentação
## Instruções
Para testar esta API, é necessário utilizar algum programa similar ao Postman, que possibilita a execução de simulações do comportamento da API, como se fosse uma outra API consumindo recursos da API Central de Erros.

O primeiro passo para a utilização da API é cadastrar um usuário, utilizando o primeiro endpoint da listagem de endpoints (N/S = Não seguro).

Feito o cadastro, as seguintes informações devem ser passadas para o endpoint de login do usuário, para a geração do token de acesso:

Essas informações são passadas como _x-www-form-urlencoded_:

**Nome do campo** | **Valor**
----- | -----
client_id | codenation.api_client
client_secret | codenation.api_secret
grant_type | client_credentials
username | {email do usuário}
password | {senha do usuário} 

## URL

`https://errorcentralv102.azurewebsites.net`

## Endpoints

### Usuários
**Ação** | **Endpoint** | **Método**
------ | ----- | -----
Cadastro de usuário (N/S) | _/user/cadastro_ | POST
Login de usuário | _/connect/token_ | POST
Listar todos os usuários | _/user_ | GET
Retornar usuário por id | _/user/{id}_ | GET
Cadastrar usuário (S) | _/user_ | POST
Alterar usuário | _/user_ | PUT
Apagar usuário | _/user/{id}_ | DELETE

### Logs de Erros
**Ação** | **Endpoint** | **Método**
----- | ----- | -----
Cadastro de Log | _/log_ | POST
Listar todos os logs | _/log_ | GET
Listar log por id | _/log/{id}_ | GET
Listar log por env_id | _/log/environment/{id}_ | GET
Listar log por env_id & level_id | _/log/environment/{id}/level/{id}_ | GET
Listar log por env_id & layer_id | _/log/environment/{id}/layer/{id}_ | GET
Listar log por env_id & descrição | _/log/environment?environmentId={id}&description={description}_ | GET
Alterar log | _/log_ | PUT
Apagar log | _/log/{id}_ | DELETE

## Layout dos Payloads

### Usuário:
    {
        "Id": 1,
        "FullName": "José da Silva",
        "Email": "jose@mail.com",
        "Password": "sfvds45",
        "CreatedAt": "2020-07-15T10:25:32"
    }

#### Logs de erros:

    {
        "Id": 1,
        "Name": "acceleration.Service.AddCandidate: <forbidden>",
        "Description": "Erro ao tentar cadastrar um cadidato. Usuário não está autorizado a acessar este endpoint",
        "CreatedAt": "2019-05-24T10:15:00-07:00",
        "UserId": 1,
        "EnvironmentId": 1,
        "LayerId": 1,
        "LevelId": 1,
        "StatusId": 1
    }
    
#### Ambiente:

    {
        "id": 1,
        "name": "Dev"
    }

#### Origem:

    {
        "id": 1,
        "name": "Backend"
    }
    
#### Severidade:

    {
        "id": 1,
        "name": "Debug"
    }

#### Status:

    {
        "id": 1,
        "name": "Arquivado"
    }

