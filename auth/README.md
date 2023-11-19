## DNIT Auth

Modulo responsável pela aunteticação e autorização dos sistemas.
A autenticação acontece por meio do [JWT](https://jwt.io/) e é compartilhada e sincronizada com todos os sistemas, ou seja,
com um único token é possivel autenticar nos serviços distribuídos.

### Empacotamento

O pacote, pra ser disponível para os serviços, está disponível no MyGet em [DnitEpsFga.auth](https://www.myget.org/feed/dnit-eps-fga/package/nuget/DnitEpsFga.auth)

#### Atualização

A seguir, os passos para atualização do pacote

- Atribuir uma nova versão no arquivo `auth.csproj` na tag `<version>VERSAO</version>`
- Builde o projeto `dotnet build --configuration Release`
- Empacote com `dotnet pack auth`. O pacote será salvo em `auth\bin\Release\DnitEpsFga.auth.<VERSAO>.nupkg`
- Va para o site do MyGet [DnitEpsFga/packages](https://www.myget.org/feed/Packages/dnit-eps-fga) clique em `Add Package`, vá para a aba `From a uploaded package` e faça o upload do arquivo `.nupkg`.
- Pelo cli: [TODO]