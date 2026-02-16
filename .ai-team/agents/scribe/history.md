# History — Scribe

## Project Learnings (from init)

**Project:** IssueTracker  
**Team Root:** E:\github\IssueTracker  
**Team:** Milo (Lead), Stansfield (Frontend), Wolinski (Backend), Hooper (Tester), Scribe, Ralph  
**Owner:** mpaulosky  

**Role:** Silent recorder — logs sessions, merges decisions, syncs context, commits `.ai-team/` state  

**Files Maintained:**
- `.ai-team/log/{YYYY-MM-DD}-{topic}.md` — Session logs
- `.ai-team/decisions.md` — Canonical merged decisions
- `.ai-team/agents/{name}/history.md` — Individual agent learnings + team updates
- `.ai-team/agents/{name}/history-archive.md` — Archived old history (if needed)
- `.ai-team/decisions/inbox/{name}-{slug}.md` — Temporary inbox (cleared after merge)

**Deduplication Logic:**
- Exact duplicates: Keep first, remove rest
- Overlapping (same topic, different dates): Consolidate with `(consolidated)` tag
- Never delete unique reasoning — merge and preserve all insights

**History Archival:**
- Threshold: ~3,000 tokens (~12KB file size)
- Action: Summarize entries >2 weeks old into "## Core Context", archive originals to `history-archive.md`
- Keep recent entries (<2 weeks) in "## Learnings"
- Preserve "## Project Learnings (from import)" section as-is

**Git Workflow (Windows PowerShell):**
- `cd {team_root}`, then `git add .ai-team/`
- Check: `git diff --cached --quiet` (exit code 0 = no changes)
- Write message to temp file, commit with `-F`, clean up
- Verify: `git log --oneline -1`

## Learnings

(None yet — first session)
