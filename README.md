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
| [v0.0.3](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.3) | 2026-09-29 | docs: Write agent guidance for IssueTracker | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-29-pr-146-docs-write-agent-guidance-for-issuetracker.md) |
| [v0.0.2](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.2) | 2026-09-29 | ci: Gate Test Suite on coverage and let fork PRs pass it | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-29-pr-145-ci-gate-test-suite-on-coverage-and-let-fork-prs-pass-it.md) |
| [v0.0.1-1170](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.1-1170) | 2026-09-29 | v0.0.1-1170 | — |
| [v0.0.1-1169](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.1-1169) | 2026-09-29 | v0.0.1-1169 | — |
| [v0.0.1-1168](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.1-1168) | 2026-09-29 | v0.0.1-1168 | — |
| [v0.0.1-1167](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.1-1167) | 2026-09-29 | v0.0.1-1167 | — |
| [v0.0.1-1166](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.1-1166) | 2026-09-29 | v0.0.1-1166 | — |
| [v0.0.1-1165](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.1-1165) | 2026-09-29 | v0.0.1-1165 | — |
| [v0.0.1-1164](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.1-1164) | 2026-09-29 | v0.0.1-1164 | — |
| [v0.0.1](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.1) | 2026-09-29 | ci: Adopt atelier-store's CI, release and lint setup | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-29-pr-140-ci-adopt-atelier-store-s-ci-release-and-lint-setup.md) |

<!-- RELEASES_END -->

[All releases →](https://github.com/mpaulosky/IssueTracker/releases)
