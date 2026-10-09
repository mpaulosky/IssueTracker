import assert from "node:assert/strict";
import { describe, it } from "node:test";
import { containsSecret, githubTokensIn, redact, sandboxEnv } from "./sandbox-env.mts";

describe("githubTokensIn", () => {
  it("finds GH_TOKEN and GITHUB_TOKEN, even when blank or exported", () => {
    const env = "CLAUDE_CODE_OAUTH_TOKEN=abc\nGH_TOKEN=\nexport GITHUB_TOKEN=xyz\n";
    assert.deepEqual(githubTokensIn(env), ["GH_TOKEN", "GITHUB_TOKEN"]);
  });

  it("ignores comments and other keys", () => {
    const env = "# GH_TOKEN=old\nANTHROPIC_API_KEY=k\nNOT_GH_TOKEN=1\n";
    assert.deepEqual(githubTokensIn(env), []);
  });
});

describe("sandboxEnv", () => {
  it("takes the file's value, or the host's when the file leaves it blank", () => {
    const env = sandboxEnv('CLAUDE_CODE_OAUTH_TOKEN=\nANTHROPIC_API_KEY="from-file-key"\n# OTHER=x\nEMPTY=\n', {
      CLAUDE_CODE_OAUTH_TOKEN: "from-host-token",
    });
    assert.deepEqual([...env], [
      ["CLAUDE_CODE_OAUTH_TOKEN", "from-host-token"],
      ["ANTHROPIC_API_KEY", "from-file-key"],
    ]);
  });
});

describe("redact", () => {
  it("replaces each secret value, however often it appears", () => {
    assert.equal(redact("a s3cr3t-value b s3cr3t-value", ["s3cr3t-value"]), "a *** b ***");
  });

  it("leaves short values alone", () => {
    assert.equal(redact("yes and no", ["no"]), "yes and no");
  });

  it("replaces token shapes that didn't come from the env file", () => {
    const text = "key sk-ant-oat01-abcdefghijklmnopqrst and ghp_abcdefghijklmnopqrstuvwxyz0123";
    assert.equal(redact(text, []), "key *** and ***");
  });
});

describe("containsSecret", () => {
  it("finds a secret value or a token shape, and nothing else", () => {
    assert.ok(containsSecret("+ const key = 's3cr3t-value';", ["s3cr3t-value"]));
    assert.ok(containsSecret("+ token: sk-ant-api03-abcdefghijklmnopqrstuv", []));
    assert.ok(!containsSecret("+ const key = 'placeholder';", ["s3cr3t-value"]));
  });
});
