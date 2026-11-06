```markdown
# 🏗️ SA-Projetos - Sistema de Gestão e Controle de Obras Externas

![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![Vue.js 3](https://img.shields.io/badge/Vue.js-3.x-4FC08D?style=flat&logo=vuedotjs)
![.NET MAUI](https://img.shields.io/badge/.NET-MAUI-512BD4?style=flat&logo=dotnet)
![Status](https://img.shields.io/badge/Status-Em_Desenvolvimento-orange)

> Solução para otimização e controle operacional de obras externas, eliminando gargalos de liberação, reduzindo papéis em campo e fornecendo acompanhamento em tempo real para equipes de gestão e instalação[cite: 1].

---

## 📌 Links do Projeto

* 🎨 **Protótipo Figma:** [SA-Projetos no Figma](https://www.figma.com/design/QA09pjDhDUcLgIEaoSsTL2/SA-Projetos?t=L39DqTOoAVBBm24Y-1)
* 📄 **Documento de Visão:** [Documento no Google Docs](https://docs.google.com/document/d/1ZJ0EyygSE4PUboTAzvI3ybeYhsSrJMvQX3Dwmz2__kE/edit?tab=t.0)[cite: 1]
* 📊 **Fluxos e Arquitetura:** [Diagrama no Mermaid](https://mermaid.ai/app/projects/6a668f01-063b-4aab-8ec5-55f76dc9f5b2/diagrams/1a3906fd-9d10-4d18-8c99-d9701001ad2c/edit)

---

## 🚀 Arquitetura e Tecnologias

O ecossistema é dividido em três aplicações principais integradas por um backend robusto:


```

```
              ┌──────────────────────┐
              │    Painel Web Vue    │ (Gestores / Vendedores)
              └──────────┬───────────┘
                         │
                         ▼

```

┌─────────────────┐   ┌─────────────┐   ┌─────────────────┐
│ App Mobile MAUI │──►│ API .NET 10 │──►│ Banco de Dados  │
└─────────────────┘   └─────────────┘   └─────────────────┘
(Instaladores)

```

* **Backend (`/src/Backend`):** REST API construída com **.NET 10**, Clean Architecture / DDD, Entity Framework Core e autenticação JWT baseada em perfis de usuário (RBAC)[cite: 1].
* **Frontend Web (`/src/Frontend`):** Dashboard Web construído em **Vue.js 3** (Vite, Pinia, Vue Router, TailwindCSS/Componentes) para gestão de vendas, liberações e medições[cite: 1].
* **Mobile (`/src/Mobile`):** App Multiplataforma construído em **.NET MAUI** (Android / iOS) focado na jornada do instalador em campo (Modo Offline-first, envio de fotos e formulário pós-montagem)[cite: 1].

---

## 🎯 Funcionalidades Principais

### 💼 Vendedores / Gestores (Web Vue.js)
* **Gestão de Medições e Obras:** Cadastro e validação da medição final pelo vendedor[cite: 1].
* **Modalidades de Pagamento:** Vínculo de pagamentos (Cartão, Boleto, Cheque) para acionamento do prazo contratual[cite: 1].
* **Painel de Liberações:** Controle total ou parcial de liberação de itens para instalação, com registro de retenções/embargos[cite: 1].
* **Dashboards:** Visualização do progresso das obras e relatórios pós-montagem[cite: 1].

### 👷‍♂️ Instaladores / Campo (Mobile .NET MAUI)
* **Minha Agenda (Consulta):** Visualização de Ordens de Serviço (OS) atreladas, endereços e itens liberados com acesso a PDFs[cite: 1].
* **Check-out e Pós-Montagem:** Formulário digital de encerramento do atendimento[cite: 1].
* **Anexo de Evidências:** Captura de fotos e registro de observações do serviço diretamente da câmera do dispositivo[cite: 1].

---

## 🛠️ Estrutura do Repositório

```text
├── src/
│   ├── Backend/            # API .NET 10.0 (Controllers, Services, Infrastructure)
│   ├── Frontend/           # Aplicação Web em Vue.js 3
│   └── Mobile/             # Aplicação Mobile em .NET MAUI
├── docs/                   # Documentações e diagramas adicionais
└── README.md

```

---

## 💻 Pré-requisitos para Execução

* [.NET 10 SDK](https://dotnet.microsoft.com/)
* [Node.js](https://nodejs.org/) (v18.x ou superior) & npm/pnpm
* Workloads do **.NET MAUI** instalados (`dotnet workload install maui`)
* Android SDK / Emulador ou dispositivo iOS configurado para depuração

---

## ⚙️ Como Executar o Projeto

### 1. API (.NET 10)

```bash
cd src/Backend
dotnet restore
dotnet run --project API/API.csproj

```

> A API estará disponível em `https://localhost:7001` (com Swagger disponível em ambiente de Dev).

### 2. Frontend (Vue.js 3)

```bash
cd src/Frontend
npm install
npm run dev

```

> O painel web abrirá em `http://localhost:5173`.

### 3. Mobile (.NET MAUI)

```bash
cd src/Mobile
# Executar no Android
dotnet build -t:Run -f net10.0-android

```

*(Ou abra a solução no Visual Studio / VS Code com as extensões do .NET MAUI e selecione o dispositivo/emulador desejado).*

---

## 📝 Roadmap de Desenvolvimento

* [x] **Etapa 1:** Modelagem do banco de dados e prototipagem das telas no Figma.


* [ ] **Etapa 2:** Criação dos endpoints da API no .NET 10 e estrutura inicial do Vue.js e MAUI.


* [ ] **Etapa 3:** Integração dos fluxos de liberação parcial/total e travas de perfil.


* [ ] **Etapa 4:** Testes de campo, homologação e preparação do MVP.



---

## 📄 Licença

Este projeto é de propriedade privada e seu uso é restrito para fins de desenvolvimento interno.

```

```
