import { execFileSync } from "node:child_process";

// Run a command on the host in `cwd` and return trimmed stdout. Throws on failure.
export const sh = (cwd: string, cmd: string, ...args: string[]) =>
  execFileSync(cmd, args, { cwd, encoding: "utf8", stdio: ["ignore", "pipe", "inherit"] }).trim();

// Sandcastle mounts this clone's .git into every sandbox, writable, so the
// agents can commit. That also lets them write .git/config, and git runs
// commands named there: credential helpers, core.sshCommand, core.fsmonitor,
// filter and diff drivers, and remotes or url.*.insteadOf rewrites that send a
// push elsewhere. So the host records the clone's own config before any
// sandbox starts, and refuses to run git once it has changed.
//
// Hooks are a second way in: core.hooksPath is .github/hooks, a path inside
// the checkout, so in an issue worktree it names hooks the agents can edit.
// The host's git runs with hooks off.

// The clone's own config: .git/config, and the include and worktree-config
// switches it holds, as `git config --local --list` prints it. Reading config
// runs nothing.
const localConfig = () => {
  try {
    return execFileSync("git", ["config", "--local", "--list"], {
      cwd: process.cwd(),
      encoding: "utf8",
      stdio: ["ignore", "pipe", "ignore"],
    });
  } catch {
    // Exit 1 with no output: the file has no entries.
    return "";
  }
};

let recordedConfig: string | undefined;

// Record the clone's config. main.mts calls this once, before any sandbox.
export function recordGitConfig(): void {
  recordedConfig = localConfig();
}

// The config lines added or removed between two `git config --list` outputs.
export function configChanges(before: string, after: string): string[] {
  const lines = (text: string) => new Set(text.split("\n").filter(Boolean));
  const was = lines(before);
  const now = lines(after);
  return [
    ...[...was].filter((line) => !now.has(line)).map((line) => `- ${line}`),
    ...[...now].filter((line) => !was.has(line)).map((line) => `+ ${line}`),
  ];
}

export class GitConfigChangedError extends Error {}

// Throw if the clone's config changed since recordGitConfig.
export function assertGitConfigUnchanged(): void {
  if (recordedConfig === undefined) return;
  const changes = configChanges(recordedConfig, localConfig());
  if (changes.length > 0) {
    throw new GitConfigChangedError(
      ".git/config changed while Sandcastle ran, and only the agents' sandboxes could have changed it. " +
        "Sandcastle stopped before running git on the host. Inspect .git/config before running any git command " +
        `in this clone:\n${changes.join("\n")}`,
    );
  }
}

// Git on the host: refused once the clone's config has changed, and with
// hooks off.
export const git = (cwd: string, ...args: string[]) => {
  assertGitConfigUnchanged();
  return sh(cwd, "git", "-c", "core.hooksPath=/dev/null", ...args);
};
