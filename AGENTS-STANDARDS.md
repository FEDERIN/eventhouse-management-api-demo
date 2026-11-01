# Engineering Standards

## 1. Purpose

Define the common engineering standards for all
repositories and NuGet libraries.

All repository-specific AGENTS.md files must follow these
standards unless an explicit project-specific exception exists.

## 2. Technology Standards

- .NET SDK: 9.0.320
- SDK rollForward: latestFeature
- Test runner: Microsoft.NET.Test.Sdk + xunit.runner.visualstudio
- Target frameworks: net8.0;net9.0
- Nullable Reference Types: enabled
- Implicit Usings: enabled
- C# language features: follow the selected SDK and project TFM.

All projects must use the shared SDK configuration.

## 3. Dependency Management

- Use Central Package Management.
- Manage package versions in Directory.Packages.props.
- Never specify Version directly in PackageReference.
- Commit packages.lock.json files.
- Use RestoreLockedMode in CI.
- Regenerate lock files after dependency changes.
- Never edit lock files manually.
- Prefer explicit and intentional dependency references.
- Do not replace PackageReference with ProjectReference
  without verifying the package architecture and release strategy.

## 4. Code Quality

- Nullable must remain enabled.
- TreatWarningsAsErrors must be enabled.
- EnforceCodeStyleInBuild must be enabled.
- AnalysisLevel must use latest-recommended.
- Deterministic builds must be enabled.
- Suppressions must be targeted and justified.
- Avoid unnecessary public API exposure.
- Follow existing project conventions.

## 5. Public API

- Every public type and member must have XML documentation.
- Public API changes must be intentional.
- Breaking changes must follow Semantic Versioning.
- Update package README, documentation and `PackageReleaseNotes`
  when applicable.
- Published package versions are immutable.

## 6. Testing

- Use xUnit v3.
- Use FluentAssertions and NSubstitute according to existing conventions.
- Unit tests must validate isolated behavior.
- Integration tests must validate real infrastructure
  when the functionality requires it.
- Use Testcontainers when testing external infrastructure.
- Do not replace integration tests with mocks when
  real infrastructure behavior is relevant.

## 7. Security and Dependencies

- Enable NuGet vulnerability auditing.
- Run the CI audit step (NuGet vulnerability audit of the
  solution including transitive dependencies) on every PR.
- Do not rely on `dotnet list package --vulnerable` as the only
  security gate.
- Never commit secrets, credentials or environment files.

## 8. Documentation

- Use MkDocs for repository documentation.
- Keep documentation synchronized with public APIs.
- Update package README files when public behavior changes.
- Documentation and published repository artifacts
  must be written in English.

## 9. Git and Pull Requests

- Never push directly to main.
- Use feature branches.
- Submit changes through Pull Requests.
- Keep changes focused and reviewable.
- Use conventional commit messages.
- Write commits and PR descriptions in English.

## 10. Definition of Done

Before considering a task complete:

1. Build succeeds in Release.
2. Relevant tests pass.
3. Formatting and code quality checks pass.
4. Lock files are updated when dependencies change.
5. Public API documentation is updated.
6. README and CHANGELOG are updated when applicable.
7. No unrelated files or changes are included.
