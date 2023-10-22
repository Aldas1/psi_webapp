# Quiz app

![Github Actions](https://github.com/Aldas1/psi_webapp/actions/workflows/dotnet.yml/badge.svg)
![Code Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/Aldas1/f04a7acd2cbaa647c6515030113c444b/raw/code-coverage.json)

## Description

ASP.NET based quiz app, which helps students and teachers to conduct quizes and make learning easier.

## Tech stack

![Dotnet](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![Typescript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)
![Vite](https://img.shields.io/badge/Vite-B73BFE?style=for-the-badge&logo=vite&logoColor=FFD62E)

## Development setup

This repo contains two projects: the web api written using ASP.NET (`QuizAppApi`) and react frontend using Vite (`better-frontend`).
You first need to run web api and, after that, run the frontend.

### DB setup

We use mssql server dbms. Make sure to install it, a viable option is to use [Docker image](https://hub.docker.com/_/microsoft-mssql-server).
No matter how you setup your dbms, you need to get the connection string, it may look like this:
```
Server=127.0.0.1,1433; Database=Master; User Id=SA; Password=REALLY_SECURE_PASSWORD; Encrypt=False; TrustServerCertificate=True;
```
[This video](https://www.youtube.com/watch?v=EmV_IBYIlyo&list=PL82C6-O4XrHdiS10BLh23x71ve9mQCln0&index=5) may help when setting up the db.

Connection string is added via secret:
```bash
cd QuizAppApi/QuizAppApi
dotnet user-secrets init
dotnet user-secrets set "ConnectionString" "YOUR_CONNECTION_STRING"
```

Make sure to [apply migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli) (You can google on how to do that)

### Backend setup

If you are using an ide, setup should be straightforward. You can also run it via cli:

``` bash
dotnet run # or
dotnet watch
```

### Backend tests

To run .NET tests use this command in QuizAppApi.sln directory:

``` bash
dotnet test
```

### Frontend setup

```bash
cd better-frontend/
npm install
npm run dev
```

### OpenAI API setup

1) Create ```.openai``` file in ```QuizAppApi/QuizAppApi``` directory
2) Edit ```.openai``` file to look like this: ```OPENAI_API_KEY='YOUR_API_KEY'```

## Table of contents

- [Quiz app](#quiz-app)
  - [Description](#description)
  - [Tech stack](#tech-stack)
  - [Development setup](#development-setup)
    - [DB setup](#db-setup)
    - [Backend setup](#backend-setup)
    - [Backend tests](#backend-tests)
    - [Frontend setup](#frontend-setup)
  - [Table of contents](#table-of-contents)
  - [Branching strategy](#branching-strategy)
    - [Branches](#branches)
  - [Naming](#naming)
    - [Branch naming](#branch-naming)
      - [Example](#example)
    - [Commit naming](#commit-naming)
      - [Example](#example-1)
  - [Repository rules](#repository-rules)
  - [Authors](#authors)

## Branching strategy

### Branches

- **Main branch** - used as a branch for all feature branches to merge and in this environment the app is tested.
- **Feat branch** - these branches are branched from _main branch_ and are used for developing.
  - Each feature branch is used for a different task and each feature branch belongs to it's owner and should be worked on by the owner.

## Naming

### Branch naming

- Branch names should begin with _feat_
- Feat branch name should give information about the task
- Words in branch name are seperated by dashes (-)
- If the feat branch is a branch of another feat branch, the name should have a number at the end

#### Example

- "feat-pipeline"
- "feat-pipeline-2"

### Commit naming

Commit messages should begin with one of the following:

- Add
- Delete
- Change
- Fix

#### Example

- "Add ASP.NET template"
- "Change README.md"
- "Delete build files"

## Repository rules

- Approver count
  - Main: 1

## Authors

- Aldas Vertelis
- Danielius Podbielski
- Kanstantinas Piatrashka
- Motiejus Šveikauskas
