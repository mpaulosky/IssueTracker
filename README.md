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
| [v0.0.50](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.50) | 2026-10-10 | chore: Re-apply the repo-ci-baseline Template | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-10-pr-250-chore-re-apply-the-repo-ci-baseline-template.md) |
| [v0.0.49](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.49) | 2026-10-10 | chore: Re-apply the repo-ci-baseline Template | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-10-pr-248-chore-re-apply-the-repo-ci-baseline-template.md) |
| [v0.0.48](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.48) | 2026-10-09 | chore: Re-apply the repo-ci-baseline Template | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-09-pr-246-chore-re-apply-the-repo-ci-baseline-template.md) |
| [v0.0.47](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.47) | 2026-10-09 | fix(sandcastle): Close the last gaps in the host's config guard and secret scan | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-09-pr-244-fix-sandcastle-close-the-last-gaps-in-the-host-s-config-guard-and-secret-scan.md) |
| [v0.0.46](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.46) | 2026-10-09 | fix(sandcastle): Close the gaps Claude Review found after #236 merged | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-09-pr-242-fix-sandcastle-close-the-gaps-claude-review-found-after-236-merged.md) |
| [v0.0.45](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.45) | 2026-10-09 | fix(sandcastle): Open one pull request per issue instead of merging locally | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-09-pr-236-fix-sandcastle-open-one-pull-request-per-issue-instead-of-merging-locally.md) |
| [v0.0.44](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.44) | 2026-10-09 | fix(sandcastle): Resolve references portably in needs-docker.sh | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-09-pr-239-fix-sandcastle-resolve-references-portably-in-needs-docker-sh.md) |
| [v0.0.43](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.43) | 2026-10-08 | chore(sandcastle): Run a Docker-free check.sh in the sandbox | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-231-chore-sandcastle-run-a-docker-free-check-sh-in-the-sandbox.md) |
| [v0.0.42](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.42) | 2026-10-08 | test(Integration.Tests): Check a cache hit by its reads, not a 5 ms timer | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-235-test-integration-tests-check-a-cache-hit-by-its-reads-not-a-5-ms-timer.md) |
| [v0.0.41](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.41) | 2026-10-08 | fix(sandcastle): Name bug branches fix/ instead of hotfix/ | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-08-pr-232-fix-sandcastle-name-bug-branches-fix-instead-of-hotfix.md) |

<!-- RELEASES_END -->

[All releases →](https://github.com/mpaulosky/IssueTracker/releases)
