// What a push of an issue branch would publish, for the secret scan: every
// commit as stored (its message and its headers: author and committer names
// and emails, encoding, signature and any extra header), and every line the
// commits add. Removed lines and context
// aren't scanned: each was added by an earlier commit, already on main or in
// this range, so scanning them would only stop branches for tokens main holds
// on purpose (the redaction tests' fake tokens).

import { git } from "./shell.mts";

// What `git log -p --cc -U0` output publishes: each file header line whole
// (the paths are published too), and the content of each added line. The
// output is read with state, not by prefix, since an added line can itself
// start with "++ " and look like a "+++ b/path" header. A hunk header has one
// "@" per column plus one ("@@" for a commit with one parent, "@@@" for a
// merge of two), and in a hunk each line starts with that many columns: a
// line is added when some column is "+" and none is "-". A hunk lasts until
// the next "diff " line.
export function addedLines(patch: string): string[] {
  const published: string[] = [];
  let columns = 0;
  for (const line of patch.split("\n")) {
    if (line.startsWith("diff ")) {
      columns = 0;
      published.push(line);
    } else if (columns === 0) {
      const hunk = /^(@{2,}) /.exec(line);
      if (hunk) columns = hunk[1]!.length - 1;
      else if (line) published.push(line);
    } else if (/^@{2,} /.test(line)) {
      continue;
    } else {
      const marks = line.slice(0, columns);
      if (marks.includes("+") && !marks.includes("-")) published.push(line.slice(columns));
    }
  }
  return published;
}

// The raw commits and added lines from `base` to `commit`. --format=raw
// prints each commit object's headers and message, not just the message: an
// agent can put a token in its author name or email (git -c user.name=...,
// GIT_AUTHOR_EMAIL) without touching .git/config, and the push publishes
// those too. --cc reads merge commits too, so content written while resolving
// a merge, or amended into one, is scanned; without it git log shows merges
// with no diff.
export function publishedText(cwd: string, base: string, commit: string): string {
  const range = `${base}..${commit}`;
  const commits = git(cwd, "log", "--no-color", "--format=raw", range);
  // --no-color: with color.diff=always in the user's config, every line would
  // start with an escape code, and the scan would see no added lines at all.
  const patch = git(cwd, "log", "--no-color", "--format=", "-p", "--cc", "-U0", "--no-ext-diff", "--no-textconv", "--text", range);
  return [commits, ...addedLines(patch)].join("\n");
}
