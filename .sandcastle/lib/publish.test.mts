import assert from "node:assert/strict";
import { execFileSync } from "node:child_process";
import { describe, it } from "node:test";
import { prBody, prTitle, withoutClosingKeywords } from "./publish.mts";

// The PR title standard, from the script the required PR title check runs.
const passesTitleCheck = (title: string) => {
  try {
    execFileSync("bash", ["scripts/check-pr-title.sh", title], { stdio: "ignore" });
    return true;
  } catch {
    return false;
  }
};

describe("prTitle", () => {
  it("keeps a title already in commit format", () => {
    assert.equal(prTitle({ title: "feat(UI): Let users sort by date", labels: [] }), "feat(UI): Let users sort by date");
  });

  it("prefixes fix: for a bug and feat: otherwise", () => {
    assert.equal(prTitle({ title: "Crash on empty list", labels: ["bug"] }), "fix: Crash on empty list");
    assert.equal(prTitle({ title: "Sort by date", labels: ["Sandcastle"] }), "feat: Sort by date");
  });

  it("capitalizes the summary and drops a closing period", () => {
    assert.equal(prTitle({ title: "fix: stop the crash.", labels: [] }), "fix: Stop the crash");
    assert.equal(prTitle({ title: "add search...", labels: [] }), "feat: Add search");
  });

  it("leaves no trailing space where a closing period was", () => {
    assert.equal(prTitle({ title: "Add search .", labels: [] }), "feat: Add search");
    assert.equal(prTitle({ title: "fix(UI): stop the crash . . ", labels: [] }), "fix(UI): Stop the crash");
  });

  it("only produces titles that pass the PR title check", () => {
    for (const title of ["feat: Add search", "fix(Web): stop the crash.", "add search", "  Odd   spacing ", "chore!: drop it", "...", "Add search .", "feat: x . "]) {
      for (const labels of [[], ["bug"]]) {
        const pr = prTitle({ title, labels });
        assert.ok(passesTitleCheck(pr), `${pr} fails scripts/check-pr-title.sh`);
      }
    }
  });
});

describe("prBody", () => {
  it("uses the template's headings, quotes the review and closes the issue", () => {
    const body = prBody({ number: 42, title: "Add search" }, "Clean.\nTested.");
    for (const heading of ["## Why", "## What changed", "## Verification"]) assert.ok(body.includes(heading), heading);
    assert.ok(body.includes("> Clean.\n> Tested."));
    assert.ok(body.endsWith("Fixes #42"));
  });
});

describe("prBody's check warning", () => {
  it("names the check files the branch changed, first", () => {
    const body = prBody({ number: 42, title: "Add search" }, "Clean.", [".sandcastle/check.sh", "tests/A/A.csproj"]);
    assert.ok(body.startsWith("> [!WARNING]"));
    assert.ok(body.includes("> - `.sandcastle/check.sh`\n> - `tests/A/A.csproj`"));
  });

  it("has no warning when the branch leaves them alone", () => {
    assert.ok(!prBody({ number: 42, title: "Add search" }, "Clean.", []).includes("[!WARNING]"));
  });
});

describe("withoutClosingKeywords", () => {
  it("rewrites issue references after a closing keyword", () => {
    assert.equal(
      withoutClosingKeywords("This also fixes #12, Resolves: #40 and closes o/r#3."),
      "This also fixes issue 12, Resolves: issue 40 and closes o/r issue 3.",
    );
  });

  it("leaves other references alone", () => {
    assert.equal(withoutClosingKeywords("Builds on #12; see #40."), "Builds on #12; see #40.");
  });

  it("keeps the body's own Fixes line, and only that one, closing an issue", () => {
    const body = prBody({ number: 42, title: "Add search" }, "Also fixes #7.");
    assert.ok(body.includes("> Also fixes issue 7."));
    assert.deepEqual(body.match(/fixes #\d+/gi), ["Fixes #42"]);
  });
});
