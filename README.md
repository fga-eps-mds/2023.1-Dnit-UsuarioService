# 2023.1-Dnit-UsuarioService

Serviço responsavel pelas funcionalidades relacionadas à autentificação (cadastro de usuarios, login, solicitação de recuperação de senha, redefinição de senha).

### Como instalar

A forma de instalação é igual para todos os serviços, havendo diferenças apenas com base no sistema operacional.


#### Windows e MacOs

##### Modo 1

- Abra um navegador da web e acesse o site oficial da Microsoft .NET: https://dotnet.microsoft.com/download/dotnet/6.0
- Role a página até a seção ".NET 6 SDK" e clique no botão de download adequado para seu sistema operacional (por exemplo, "macOS x64 Installer" para macOS 64 bits ou  "Windows x64 Installer" para Windows 64 bits).
- O arquivo de instalação será baixado. Depois que o download for concluído, clique duas vezes no arquivo para iniciá-lo.
- O instalador será aberto. Leia e aceite os termos de licença.
- Selecione as opções de instalação que você deseja.
- Clique no botão "Install" (Instalar) para iniciar a instalação do .NET 6.
- Após a conclusão da instalação, você verá uma tela informando que o .NET 6 SDK foi instalado com sucesso.
- Para verificar se a instalação foi bem-sucedida, abra o Prompt de Comando ou o PowerShell e execute o seguinte comando:

```bash
dotnet --version
```
- Isso exibirá a versão do .NET instalada, confirmando se o .NET 6 está configurado corretamente.

##### Modo 2

Basta instalar a IDE [Visual Studio](https://visualstudio.microsoft.com/pt-br/free-developer-offers/) escolhendo a versão gratuita (Versão Community). Após instalar o Visual Studio, ele automaticamente irá instalar o .NET com a versão mais estável.

#### Linux

Instale o SDK do *.*NET .

```bash
sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-6.0
```

Instale o runtime ASP.NET Core.

```bash
sudo apt-get update && \
  sudo apt-get install -y aspnetcore-runtime-6.0
```

Entre na pasta do serviço. Dentro da pasta "app" rode o comando:

```bash
dotnet run
```
### Clonar Aplicação

Para clonar o repostório, basta utilizar o comando abaixo:

##### UsuarioService
```
git clone https://github.com/fga-eps-mds/2023.1-Dnit-UsuarioService.git
```

### Como Rodar
### Utilizando docker-compose

#### Pré-requisitos
- Docker
- Docker-compose

#### Windows 
Rode o seguinte comando na pasta da aplicação.
```bash
docker-compose build && docker-compose up
```


#### Linux ou MacOS
Rode o seguinte comando na pasta da aplicação.
```bash
sudo docker-compose build && sudo docker-compose up
```

#### Usando Visual Studio

Para rodar uma aplicação usando Visual Studio, basta clicar no arquivo com extenção 'sln' e em seguida clicar no ícone para rodar aplicação conforme mostra abaixo:
<br>
![rodar](https://github.com/fga-eps-mds/2023.1-Dnit-EscolaService/assets/54676096/c7f08d0f-e1e7-45ab-b5a4-bbf1089ce1d8)

#### Usando Visual Studio Code

Para rodar utilizando o VS Code, basta seguir a seguinte instrução:

Entre na pasta do serviço. Dentro da pasta "app" rode o comando:

```bash
dotnet run
```

### Encerrando a aplicação

- No terminal em que a aplicação esta rodando, digite simultaneamente as teclas **ctrl**+**c**. 
- Caso esteja utilizando o Visual Studio, clique no ícone quadrado vermelho <br>.

![parar](https://github.com/fga-eps-mds/2023.1-Dnit-EscolaService/assets/54676096/45aedf91-bfb3-4475-afeb-6111a6feabe8)

### Documentação endpoints

Para documentar os endpoints estamos utilizando o Swagger. Caso queira visualizar, basta abrir a rota: 
```bash
http://localhost:7083/swagger/index.html
```

<img src="https://github.com/fga-eps-mds/2023.1-Dnit-UsuarioService/assets/54676096/2b2b5fef-7b52-4f40-ab91-c391aaae5d76" alt="swagger-usuarioservice" style="width:800px;">


### Licença

O projeto DnitUsuarioService está sob as regras aplicadas na licença [AGPL-3.0](https://github.com/fga-eps-mds/2023.1-Dnit-UsuarioService/blob/main/LICENSE
)

## Contribuidores

<a href="https://github.com/fga-eps-mds/2023.1-Dnit-UsuarioService/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=fga-eps-mds/2023.1-Dnit-UsuarioService" />
</a>
