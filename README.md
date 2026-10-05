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
| [v0.0.27](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.27) | 2026-10-05 | chore: Re-apply the repo-ci-baseline Template | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-05-pr-203-chore-re-apply-the-repo-ci-baseline-template.md) |
| [v0.0.26](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.26) | 2026-10-04 | ci(automerge): Re-check a PR when a review is submitted | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-04-pr-201-ci-automerge-re-check-a-pr-when-a-review-is-submitted.md) |
| [v0.0.25](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.25) | 2026-10-04 | fix(AppHost): Start the AppHost again on Aspire 13.5 | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-04-pr-197-fix-apphost-start-the-apphost-again-on-aspire-13-5.md) |
| [v0.0.24](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.24) | 2026-10-04 | chore(deps): bump the all-actions group with 2 updates | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-04-pr-198-chore-deps-bump-the-all-actions-group-with-2-updates.md) |
| [v0.0.23](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.23) | 2026-10-04 | chore: standardize on the repo-ci-baseline Template | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-10-04-pr-194-chore-standardize-on-the-repo-ci-baseline-template.md) |
| [v0.0.22](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.22) | 2026-09-30 | ci(release): List again every 2s, for up to 30s | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-30-pr-192-ci-release-list-again-every-2s-for-up-to-30s.md) |
| [v0.0.21](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.21) | 2026-09-30 | fix(release): Wait for a just-published Release to be listed | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-30-pr-190-fix-release-wait-for-a-just-published-release-to-be-listed.md) |
| [v0.0.20](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.20) | 2026-09-30 | ci: Classify a PR's changed files in a tested script | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-30-pr-188-ci-classify-a-pr-s-changed-files-in-a-tested-script.md) |
| [v0.0.19](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.19) | 2026-09-30 | test(hooks): Test uppercase slugs apart from the chore/ digit rule | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-30-pr-186-test-hooks-test-uppercase-slugs-apart-from-the-chore-digit-rule.md) |
| [v0.0.18](https://github.com/mpaulosky/IssueTracker/releases/tag/v0.0.18) | 2026-09-30 | fix(release): Skip a named PR merged elsewhere; mark a published draft | [Post](https://github.com/mpaulosky/IssueTracker/blob/main/docs/blogs/2026-09-30-pr-184-fix-release-skip-a-named-pr-merged-elsewhere-mark-a-published-draft.md) |

<!-- RELEASES_END -->

[All releases →](https://github.com/mpaulosky/IssueTracker/releases)
