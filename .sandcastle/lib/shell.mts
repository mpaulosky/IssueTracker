import { execFileSync } from "node:child_process";

// Run a command on the host in `cwd` and return trimmed stdout. Throws on failure.
export const sh = (cwd: string, cmd: string, ...args: string[]) =>
  execFileSync(cmd, args, { cwd, encoding: "utf8", stdio: ["ignore", "pipe", "inherit"] }).trim();

// Git with hooks switched off. core.hooksPath is .github/hooks, a path inside
// the checkout, so in an issue worktree it names hooks the agents can edit;
// running them on the host would run the agents' code outside the sandbox.
export const git = (cwd: string, ...args: string[]) => sh(cwd, "git", "-c", "core.hooksPath=/dev/null", ...args);
