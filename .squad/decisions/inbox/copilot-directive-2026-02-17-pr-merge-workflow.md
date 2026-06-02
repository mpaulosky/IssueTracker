### 2026-02-17: PR merge workflow — sync main before next branch

**By:** mpaulosky (via Copilot)

**What:** After a PR is merged/closed, always:
1. Checkout main branch (`git checkout main`)
2. Sync with origin/main (`git pull origin main`)
3. Resolve any merge conflicts or issues
4. THEN create new feature branches for next work

Never start new feature branches from stale/diverged main.

**Why:** Prevents codebase from diverging and getting "completely out of control." Ensures all new work is based on the latest stable main branch, avoiding cascading conflicts and rework when merging PRs downstream.

**Implementation:**
- Required workflow after every PR merge
- Apply before creating ANY new feature branch (squad/*, etc.)
- If conflicts found during sync, resolve before proceeding
- This maintains main as single source of truth
