# Quiz app

## Description

ASP.NET based quiz app, which helps students and teachers to conduct quizes and make learning easier.

## Tech stack

- ASP.NET
- React

## Development setup

This repo contains two projects: the web api written using ASP.NET (`QuizAppApi`) and react frontend using Vite (`react-app`).
You first need to run web api and, after that, run the frontend.

### Backend setup

If you are using an ide, setup should be straightforward. You can also run it via cli:

```
dotnet run # or
dotnet watch
```

### Frontend setup

Please check the [react-app README](react-app/README.md)

## Table of contents

- [Quiz app](#quiz-app)
  - [Description](#description)
  - [Tech stack](#tech-stack)
  - [Table of contents](#table-of-contents)
  - [Branching strategy](#branching-strategy)
    - [Branches](#branches)
  - [Naming](#naming)
    - [Branch naming](#branch-naming)
      - [Example](#example)
    - [Commit naming](#commit-naming)
      - [Example](#example-1)
  - [Repository rules](#repository-rules)
  - [Tree](#tree)
    - [Notes](#notes)
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

## Tree

```bash
.
├── misc
└── README.md
```

### Notes

- Misc folder is used for storing images and miscellaneous items.

## Authors

- Aldas Vertelis
- Danielius Podbielski
- Kanstantinas Piatrashka
- Motiejus Šveikauskas
