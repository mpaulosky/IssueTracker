// Sandcastle passes every key in .sandcastle/.env into the sandbox, taking the
// value from the host's environment when the file leaves it blank. So a
// GH_TOKEN line there, even an empty one, hands the host's GitHub token to the
// agents. main.mts refuses to start while one is present.
//
// The keys that are there (the Claude token or API key) reach the sandbox, so
// anything printed in it could hold them. The host removes their values from
// every issue comment and pull request it writes.

import { existsSync, readFileSync } from "node:fs";

export const ENV_FILE = ".sandcastle/.env";

const githubTokens = new Set(["GH_TOKEN", "GITHUB_TOKEN"]);

// GitHub token keys the env file content defines, in file order.
export function githubTokensIn(envFile: string): string[] {
  const found: string[] = [];
  for (const line of envFile.split("\n")) {
    const trimmed = line.trim();
    if (!trimmed || trimmed.startsWith("#")) continue;
    const eq = trimmed.indexOf("=");
    if (eq === -1) continue;
    const key = trimmed.slice(0, eq).trim().replace(/^export\s+/, "");
    if (githubTokens.has(key)) found.push(key);
  }
  return found;
}

// The keys the env file content defines, in file order, with the value the
// sandbox gets: the file's, or the host environment's when the file's is blank.
export function sandboxEnv(envFile: string, hostEnv: Record<string, string | undefined>): Map<string, string> {
  const values = new Map<string, string>();
  for (const line of envFile.split("\n")) {
    const trimmed = line.trim();
    if (!trimmed || trimmed.startsWith("#")) continue;
    const eq = trimmed.indexOf("=");
    if (eq === -1) continue;
    const key = trimmed.slice(0, eq).trim().replace(/^export\s+/, "");
    const value = trimmed.slice(eq + 1).trim().replace(/^(["'])(.*)\1$/, "$2");
    const effective = value || hostEnv[key] || "";
    if (effective) values.set(key, effective);
  }
  return values;
}

// Token shapes worth removing even when they didn't come from the env file.
const tokenPatterns = [/sk-ant-[A-Za-z0-9_-]{16,}/g, /\bgh[pousr]_[A-Za-z0-9]{20,}\b/g, /\bgithub_pat_[A-Za-z0-9_]{20,}\b/g];

// Shorter values are too likely to be ordinary words to remove everywhere.
const minimumSecretLength = 8;

// The text with every secret value, and anything shaped like a Claude or
// GitHub token, replaced by ***.
export function redact(text: string, secrets: Iterable<string>): string {
  let result = text;
  const longestFirst = [...secrets].filter((secret) => secret.length >= minimumSecretLength).sort((a, b) => b.length - a.length);
  for (const secret of longestFirst) result = result.split(secret).join("***");
  for (const pattern of tokenPatterns) result = result.replace(pattern, "***");
  return result;
}

// The values .sandcastle/.env gives the sandbox.
function sandboxSecrets(): Iterable<string> {
  const envFile = existsSync(ENV_FILE) ? readFileSync(ENV_FILE, "utf8") : "";
  return sandboxEnv(envFile, process.env).values();
}

// redact with the values .sandcastle/.env gives the sandbox.
export function redactSandboxSecrets(text: string): string {
  return redact(text, sandboxSecrets());
}

// Whether the text holds a secret or token that redact would replace.
export function containsSecret(text: string, secrets: Iterable<string>): boolean {
  return redact(text, secrets) !== text;
}

// containsSecret with the values .sandcastle/.env gives the sandbox.
export function containsSandboxSecret(text: string): boolean {
  return containsSecret(text, sandboxSecrets());
}
