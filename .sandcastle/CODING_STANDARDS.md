# Coding Standards

The reviewer loads this file through `@.sandcastle/CODING_STANDARDS.md`. It doesn't restate the repo's rules, which
would drift from them; it names where they live. Read each source that applies to the diff, and enforce it.

## Sources of truth

| Source | Covers |
| --- | --- |
| `CLAUDE.md` | Layout and layering, how the pieces fit, build and test commands, test conventions, code style |
| `.editorconfig` | Formatting and C# style; it is authoritative |
| `.github/instructions/dotnet-project.instructions.md` | .NET project and package rules |
| `.github/instructions/blazor.instructions.md` | Blazor pages and components |
| `.github/instructions/mongo-dba.instructions.md` | MongoDB access and repositories |
| `.github/instructions/markdown.instructions.md` | Markdown files |
| `.github/instructions/git-commit-instructions.md` | Commit messages |
| `docs/PROCESS.md` | Branches, commits, checks and merging |

Where an instructions file disagrees with `CLAUDE.md`, `CLAUDE.md` wins.

## What reviews most often catch

These are in the sources above; they are listed here because they are the easiest to miss.

- Dependencies point inward only: UI to Services to CoreBusiness, with PlugIns implementing the repository interfaces.
  Pages and components never touch MongoDB or a repository.
- Every write path removes each cache entry it affects, including `IssueService`'s per-Author list.
- Tabs, file-scoped namespaces, explicit types rather than `var`, and `System` usings first.
- Every `.cs` file starts with the repository's copyright header block, and public members have XML doc comments.
- Package versions live in `Directory.Packages.props`; a `PackageReference` has no `Version`.
- Warnings are errors. A `NoWarn` needs a comment saying why and when it goes away.
- New behaviour and bug fixes come with tests: xUnit v3, bUnit test classes deriving from `BunitContext`,
  `// Arrange`, `// Act` and `// Assert` markers, and names like `Method_With_Condition_Should_Result_Test`.
- No secrets in `appsettings*.json` or code.
- Commits are `<type>(<scope>): <Summary>`: imperative, capitalized, no closing period.
