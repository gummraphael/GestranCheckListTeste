# Gestran Checklist

O objetivo do projeto é realizar checklists de entrada e saída nos veículos, onde o executor irá analisar e verificar se as informações estão em conformidade.

## 🚀 Começando

Essas instruções permitirão que você obtenha uma cópia do projeto em operação na sua máquina local para fins de desenvolvimento e teste.

### 📋 Pré-requisitos

Antes de começar, você precisará ter instalado em sua máquina:

* [.NET 8 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
* SQL Server (12 ou mais atual)


### 🔧 Instalação

1. Clone o repositório:

```bash
git clone https://github.com/gummraphael/GestranCheckListTeste.git
```

2. Navegue até o diretório do projeto:
`cd gestran-checklist`

3. Restaure as dependências:

```bash
dotnet restore
```

## ⚙️ Configuração do Banco de Dados

Abra o arquivo `appsettings.json` e configure a string de conexão em `GestranCheckListConnectionString`.  
  
## Criação e Execução de Migrações

### Criar a Migração

Crie uma migração com o seguinte comando:

```bash
dotnet ef migrations add Minha_Migration
```
> [!WARNING]
> Nota: Não esqueça de trocar `Minha_Migration` por um nome da sua escolha.
    
### Aplicar a Migração ao Banco de Dados

Para aplicar a migração e criar as tabelas no banco de dados, execute:

```bash
dotnet ef database update
```

## Como usar
Para iniciar a aplicação, execute:

```bash
dotnet run
```
Acesse a aplicação em `http://localhost:5000` (ou outra porta configurada).
