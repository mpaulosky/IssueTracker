import assert from "node:assert/strict";
import { spawnSync } from "node:child_process";
import { mkdirSync, mkdtempSync, rmSync, writeFileSync } from "node:fs";
import { tmpdir } from "node:os";
import { dirname, join, resolve } from "node:path";
import { after, describe, it } from "node:test";

// .sandcastle/needs-docker.sh decides which test projects check.sh skips in
// the Docker-free sandbox. These run it against fixture projects in a temp repo.
const script = resolve(".sandcastle/needs-docker.sh");
const root = mkdtempSync(join(tmpdir(), "needs-docker-"));
after(() => rmSync(root, { recursive: true, force: true }));

const project = (path: string, body: string) => {
  const file = join(root, path);
  mkdirSync(dirname(file), { recursive: true });
  writeFileSync(file, `<Project Sdk="Microsoft.NET.Sdk">\n${body}\n</Project>\n`);
  return path;
};

const status = (path: string) => spawnSync("bash", [script, path], { cwd: root }).status;
const needsDocker = (path: string) => status(path) === 0;

describe("needs-docker.sh", () => {
  it("says yes to a project that references a Testcontainers package", () => {
    const path = project("tests/Direct/Direct.csproj", '<ItemGroup><PackageReference Include="Testcontainers.MongoDb" /></ItemGroup>');
    assert.equal(needsDocker(path), true);
  });

  it("says yes to Aspire.Hosting.Testing and Playwright", () => {
    const aspire = project("tests/Aspire/Aspire.csproj", '<ItemGroup><PackageReference Include="Aspire.Hosting.Testing" /></ItemGroup>');
    const playwright = project("tests/E2E/E2E.csproj", '<ItemGroup><PackageReference Include="Microsoft.Playwright" /></ItemGroup>');
    assert.equal(needsDocker(aspire), true);
    assert.equal(needsDocker(playwright), true);
  });

  it("matches a PackageReference split across lines", () => {
    const path = project("tests/Split/Split.csproj", '<ItemGroup>\n  <PackageReference\n    Include="Testcontainers" />\n</ItemGroup>');
    assert.equal(needsDocker(path), true);
  });

  it("says yes to a project marked RequiresDocker", () => {
    const path = project("tests/Marked/Marked.csproj", "<PropertyGroup>\n  <RequiresDocker>true</RequiresDocker>\n</PropertyGroup>");
    assert.equal(needsDocker(path), true);
  });

  it("follows tests/ project references at any depth, with Windows separators", () => {
    project("tests/Leaf/Leaf.csproj", '<ItemGroup><PackageReference Include="Testcontainers" /></ItemGroup>');
    project("tests/Middle/Middle.csproj", '<ItemGroup><ProjectReference Include="..\\Leaf\\Leaf.csproj" /></ItemGroup>');
    const path = project("tests/Top/Top.csproj", '<ItemGroup><ProjectReference Include="..\\Middle\\Middle.csproj" /></ItemGroup>');
    assert.equal(needsDocker(path), true);
  });

  it("ignores references outside tests/", () => {
    project("src/Tool/Tool.csproj", '<ItemGroup><PackageReference Include="Testcontainers" /></ItemGroup>');
    const path = project("tests/UsesSrc/UsesSrc.csproj", '<ItemGroup><ProjectReference Include="..\\..\\src\\Tool\\Tool.csproj" /></ItemGroup>');
    assert.equal(needsDocker(path), false);
  });

  it("ignores a commented-out reference", () => {
    const path = project("tests/Commented/Commented.csproj", '<ItemGroup>\n  <!-- <PackageReference Include="Testcontainers" /> -->\n  <PackageReference Include="xunit.v3" />\n</ItemGroup>');
    assert.equal(needsDocker(path), false);
  });

  it("terminates on a reference cycle and says no when nothing needs Docker", () => {
    project("tests/CycleA/CycleA.csproj", '<ItemGroup><ProjectReference Include="..\\CycleB\\CycleB.csproj" /></ItemGroup>');
    project("tests/CycleB/CycleB.csproj", '<ItemGroup><ProjectReference Include="..\\CycleA\\CycleA.csproj" /></ItemGroup>');
    assert.equal(status("tests/CycleA/CycleA.csproj"), 1);
  });

  it("says no to a plain unit test project", () => {
    const path = project("tests/Unit/Unit.csproj", '<ItemGroup><PackageReference Include="xunit.v3" /><PackageReference Include="NSubstitute" /></ItemGroup>');
    assert.equal(status(path), 1);
  });

  it("fails with 2 for a project that doesn't exist", () => {
    assert.equal(status("tests/Missing/Missing.csproj"), 2);
  });
});
