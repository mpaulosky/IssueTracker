// The planner's plan, and the issues the host takes from it.

import { z } from "zod";

// The planner emits its plan as JSON inside <plan> tags; Output.object extracts
// and validates it against this schema. There's no branch field: the host
// names branches, so a re-plan can't move an issue's work to a new branch.
export const planSchema = z.object({
  issues: z.array(z.object({ id: z.string(), title: z.string() })),
});

// The ready issues the plan picks, in plan order, each once. An id that isn't
// a ready issue (hallucinated, stale, or held back for an open PR) is skipped,
// so the planner can't start work on an issue it wasn't offered. A repeated
// id is skipped too: two pipelines on one branch would share a worktree.
export function readyPicks<T extends { number: number }>(
  planned: readonly { id: string }[],
  ready: readonly T[],
  warn: (line: string) => void = console.warn,
): T[] {
  const picks: T[] = [];
  for (const { id } of planned) {
    const issue = ready.find((open) => String(open.number) === id.trim().replace(/^#/, ""));
    if (!issue) {
      warn(`  Skipping ${id}: it isn't one of the ready issues.`);
    } else if (picks.includes(issue)) {
      warn(`  Skipping ${id}: the plan already picked it.`);
    } else {
      picks.push(issue);
    }
  }
  return picks;
}
