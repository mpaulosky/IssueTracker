# Issue Tracker

A modern issue tracking application built with Blazor and MongoDB, featuring comprehensive unit and integration testing with Docker-based test isolation.

## Statuses

[![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/mpaulosky/IssueTracker/blob/main/LICENSE)
[![xUnit Tests](https://img.shields.io/badge/Tests-xUnit-blueviolet?logo=github)](https://github.com/mpaulosky/IssueTracker/actions/workflows/ci.yml)
[![Latest release](https://img.shields.io/github/v/release/mpaulosky/IssueTracker)](https://github.com/mpaulosky/IssueTracker/releases/latest)
[![Codecov](https://img.shields.io/codecov/c/github/mpaulosky/IssueTracker?logo=codecov)](https://codecov.io/gh/mpaulosky/IssueTracker)
[![Stars](https://img.shields.io/github/stars/mpaulosky/IssueTracker)](https://github.com/mpaulosky/IssueTracker/stargazers)

[![Build and Test Suite](https://github.com/mpaulosky/IssueTracker/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/mpaulosky/IssueTracker/actions/workflows/ci.yml)
[![CodeQL](https://github.com/mpaulosky/IssueTracker/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/mpaulosky/IssueTracker/actions/workflows/codeql-analysis.yml)
[![.NET code metrics](https://github.com/mpaulosky/IssueTracker/actions/workflows/code-metrics.yml/badge.svg)](https://github.com/mpaulosky/IssueTracker/actions/workflows/code-metrics.yml)

[![Lint Markdown](https://github.com/mpaulosky/IssueTracker/actions/workflows/lint-markdown.yml/badge.svg?branch=main)](https://github.com/mpaulosky/IssueTracker/actions/workflows/lint-markdown.yml)
[![Lint YAML](https://github.com/mpaulosky/IssueTracker/actions/workflows/lint-yaml.yml/badge.svg?branch=main)](https://github.com/mpaulosky/IssueTracker/actions/workflows/lint-yaml.yml)
[![Lint Actions](https://github.com/mpaulosky/IssueTracker/actions/workflows/lint-actions.yml/badge.svg?branch=main)](https://github.com/mpaulosky/IssueTracker/actions/workflows/lint-actions.yml)

[![Open issues](https://img.shields.io/github/issues/mpaulosky/IssueTracker)](https://github.com/mpaulosky/IssueTracker/issues)
[![Closed issues](https://img.shields.io/github/issues-closed/mpaulosky/IssueTracker)](https://github.com/mpaulosky/IssueTracker/issues?q=is%3Aissue+is%3Aclosed)
[![Open PRs](https://img.shields.io/github/issues-pr/mpaulosky/IssueTracker)](https://github.com/mpaulosky/IssueTracker/pulls)
[![Closed PRs](https://img.shields.io/github/issues-pr-closed/mpaulosky/IssueTracker)](https://github.com/mpaulosky/IssueTracker/pulls?q=is%3Apr+is%3Aclosed)

## 🚀 Quick Start

```bash
# Clone the repository
git clone https://github.com/mpaulosky/IssueTracker.git
cd IssueTracker

# Restore dependencies
dotnet restore

# Run the application
dotnet run --project src/AppHost/AppHost.csproj
```text

## 📖 Documentation

For detailed documentation, architecture guides, and comprehensive references, visit our **[documentation site](https://mpaulosky.github.io/IssueTracker/)**.

- [Getting Started Guide](docs/getting-started.md)
- [Architecture Overview](docs/architecture.md)
- [Testing Guide](docs/testing.md)
- [Contributing Guide](docs/CONTRIBUTING.md)
- [Code Metrics](docs/CODE_METRICS.md)

## 🛠️ Tech Stack

- **.NET 10** - Modern C# framework
- **Blazor Server** - Interactive web UI
- **MongoDB** - Document database
- **Docker** - Containerization and test isolation
- **xUnit & bUnit** - Comprehensive testing

## 🤝 Contributing

Contributions are welcome! Please read our [Contributing Guide](docs/CONTRIBUTING.md) and [Code of Conduct](docs/CODE_OF_CONDUCT.md) before submitting pull requests.

After cloning, turn on the repository's git hooks once. They lint staged Markdown on commit and run `scripts/gate.sh` (lint, build and tests) on push:

```bash
git config core.hooksPath .github/hooks
```

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔒 Security

For security concerns, please review our [Security Policy](docs/SECURITY.md).

## Releases

<!-- RELEASES_START -->

| Version | Date | Title | Blog post |
| ------- | ---- | ----- | --------- |
| [v0.0.42](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.42) | 2026-10-08 | test(Integration.Tests): Check a cache hit by its reads, not a 5 ms timer | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-235-test-integration-tests-check-a-cache-hit-by-its-reads-not-a-5-ms-timer.md) |
| [v0.0.41](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.41) | 2026-10-08 | fix(sandcastle): Name bug branches fix/ instead of hotfix/ | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-232-fix-sandcastle-name-bug-branches-fix-instead-of-hotfix.md) |
| [v0.0.40](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.40) | 2026-10-08 | chore: Re-apply the repo-ci-baseline Template | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-227-chore-re-apply-the-repo-ci-baseline-template.md) |
| [v0.0.39](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.39) | 2026-10-08 | chore: Commit the Sandcastle setup | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-228-chore-commit-the-sandcastle-setup.md) |
| [v0.0.38](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.38) | 2026-10-08 | chore: Re-apply the repo-ci-baseline Template | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-225-chore-re-apply-the-repo-ci-baseline-template.md) |
| [v0.0.37](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.37) | 2026-10-08 | chore: Re-apply the repo-ci-baseline Template | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-223-chore-re-apply-the-repo-ci-baseline-template.md) |
| [v0.0.36](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.36) | 2026-10-08 | chore: Re-apply the repo-ci-baseline Template | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-221-chore-re-apply-the-repo-ci-baseline-template.md) |
| [v0.0.35](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.35) | 2026-10-08 | chore: Re-apply the repo-ci-baseline Template | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-219-chore-re-apply-the-repo-ci-baseline-template.md) |
| [v0.0.34](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.34) | 2026-10-08 | chore: Re-apply the repo-ci-baseline Template | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-217-chore-re-apply-the-repo-ci-baseline-template.md) |
| [v0.0.33](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.33) | 2026-10-08 | chore: Re-apply the repo-ci-baseline Template | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-215-chore-re-apply-the-repo-ci-baseline-template.md) |

<!-- RELEASES_END -->

[All releases →](https://github.com/mpaulosky/IssueTracker/releases)
