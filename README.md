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
| [v0.0.8](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.8) | 2026-09-29 | fix(Services): Hide Pending and Rejected Issues from other Users | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-29-pr-162-fix-services-hide-pending-and-rejected-issues-from-other-users.md) |
| [v0.0.7](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.7) | 2026-09-29 | test(UI): Move bUnit tests to BunitContext on bUnit 2.11.3 | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-29-pr-159-test-ui-move-bunit-tests-to-bunitcontext-on-bunit-2-11-3.md) |
| [v0.0.6](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.6) | 2026-09-29 | ci(release): Drop featured_image from release blog posts | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-29-pr-157-ci-release-drop-featured-image-from-release-blog-posts.md) |
| [v0.0.5](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.5) | 2026-09-29 | docs: Add a CONTEXT.md glossary of the domain language | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-29-pr-153-docs-add-a-context-md-glossary-of-the-domain-language.md) |
| [v0.0.4](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.4) | 2026-09-29 | ci: Run code metrics by hand and drop unused Auth0 settings | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-29-pr-148-ci-run-code-metrics-by-hand-and-drop-unused-auth0-settings.md) |
| [v0.0.3](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.3) | 2026-09-29 | docs: Write agent guidance for IssueTracker | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-29-pr-146-docs-write-agent-guidance-for-issuetracker.md) |
| [v0.0.2](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.2) | 2026-09-29 | ci: Gate Test Suite on coverage and let fork PRs pass it | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-29-pr-145-ci-gate-test-suite-on-coverage-and-let-fork-prs-pass-it.md) |
| [v0.0.1-1170](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.1-1170) | 2026-09-29 | v0.0.1-1170 | — |
| [v0.0.1-1169](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.1-1169) | 2026-09-29 | v0.0.1-1169 | — |
| [v0.0.1-1168](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.1-1168) | 2026-09-29 | v0.0.1-1168 | — |

<!-- RELEASES_END -->

[All releases →](https://github.com/mpaulosky/IssueTracker/releases)
