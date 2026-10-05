# Contributing to This Project

Thank you for taking the time to consider contributing to our project.

The following is a set of guidelines for contributing to the project. These are mostly guidelines, not rules, and can be
changed in the future. Please submit your suggestions with a pull-request to this document.

- [Contributing to this Project](#contributing-to-this-project)
  - [Code of Conduct](#code-of-conduct)
  - [Quick Start](#quick-start)
  - [What should I know before I get started](#what-should-i-know-before-i-get-started)
    - [Code Style & Commit Messages](#code-style--commit-messages)
    - [Project Folder Structure](#project-folder-structure)
    - [Design Decisions](#design-decisions)
    - [How can I contribute](#how-can-i-contribute)
    - [Create an Issue](#create-an-issue)
    - [Respond to an Issue](#respond-to-an-issue)
    - [Review Process](#review-process)
    - [Write code](#write-code)
    - [Write documentation](#write-documentation)

## Code of Conduct

We have adopted a code of conduct from the Contributor Covenant. Contributors to this project are expected to adhere to
this code. Please report unwanted behavior to [Matthew Paulosky](mailto:matthew.paulosky@outlook.com)

## Quick Start

1. Fork the repository and clone your fork.
2. Follow [PROCESS.md](PROCESS.md): the one-time hook setup, a branch named to the standard in its own worktree,
   commit and PR title format, the PR description, and how checks, review, merging and releases work.
3. Make your changes, following the code style and guidelines below, with tests.
4. Push your branch and open a Pull Request to `main` using the template.

## What should I know before I get started

This project is an issue tracker built as a Blazor Server app on .NET 10, with MongoDB for storage and Redis for
caching, orchestrated by .NET Aspire.

### Code Style & Commit Messages

- `.editorconfig` is authoritative for formatting.
- Commits and PR titles follow [git-commit-instructions.md](../.github/instructions/git-commit-instructions.md)
  (`<type>(<scope>): <Summary>`); see [PROCESS.md](PROCESS.md#commits-and-pr-titles).

### Project Folder Structure

This project is designed to be built and run primarily with Visual Studio 2022 or JetBrains Rider. The folders are
configured so that they will support editing and working in other editors and on non-Windows operating systems. We
encourage you to develop with these other environments, because we would like to be able to support developers who use
those tools as well. The layout is described in [project-structure.md](project-structure.md) and
[architecture.md](architecture.md).

All official versions of the project are built and delivered with GitHub Actions and linked in the main README.md
and [releases tab in GitHub](https://github.com/mpaulosky/IssueTracker/releases).

### Design Decisions

Design for this project is ultimately decided by the project team
lead, [Matthew Paulosky](mailto:matthew.paulosky@outlook.com). The following project tenets are adhered to when making
decisions:

1. Use Blazor Server for the UI.

1. Follow Clean Architecture: the UI calls services, and services depend on repository interfaces.

1. Use MongoDB for data storage and Redis for caching.

1. Use .NET Aspire to orchestrate the app and its services.

### How can I contribute

We are always looking for help on this project. There are several ways that you can help:
This means one of several types of contributions:

1. [Create an Issue](#create-an-issue)

1. [Respond to an Issue](#respond-to-an-issue)

1. [Write code](#write-code)

1. [Write documentation](#write-documentation)

### Create an Issue

Create a [New Issue Here](https://github.com/mpaulosky/IssueTracker/issues/new/choose).

1. If you are reporting a `Bug` that you have found. Be sure to add the `Bug` label so that we can triage and track it.

1. If you are reporting an `Enhancement` that you think would improve the project. Be sure to add the `Enhancement`
   label so we can track it.

### Respond to an Issue

[Fork the Repository to your GitHub account](https://github.com/mpaulosky/IssueTracker/fork).

1. Create a branch in its own worktree, named for the existing Issue number (`feature/{issue}-{slug}` or
   `fix/{issue}-{slug}`); see [PROCESS.md](PROCESS.md#branches-and-worktrees).

1. Work on the issue.

1. Create Unit, Integration tests for any code that require them. We use xUnit v3 to test our code and
   [bUnit](https://www.nuget.org/packages/bunit/) to test our blazor components.

1. When you are done Create a Pull Request from your branch to `main`.

1. Submit the Pull Request.

Any code that is written to support a blazor component or new functionality are required to be accompanied with unit
tests at the time the pull request is submitted. Pull requests without unit tests will be delayed and asked for unit
tests to prove their functionality.

### Review Process

Every PR is reviewed by Copilot on each push, and merges once its required checks pass and every review thread is
resolved: a same-repo PR merges on its own, and the maintainer merges a fork's. The details are in
[PROCESS.md](PROCESS.md#checks-review-and-merging).

### Write code

All code should have an assigned issue that matches it. This way we can prevent contributors from working on the same
feature at the same time.

Code for components' features should also include some definition in the `/docs` folder so that our users can
identify and understand which feature is supported.

### Write documentation

The documentation for the project is always needed. We are always looking for help to add content to the `/docs`
section of the repository with proper links back through to the main `/README.md`.
