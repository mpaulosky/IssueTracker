# TASK

Fix issue {{TASK_ID}}: {{ISSUE_TITLE}}

Pull in the issue using `gh issue view <ID>`. If it has a parent PRD, pull that in too.

Only work on the issue specified.

Work on branch {{BRANCH}}. Make commits and run tests.

# CONTEXT

Here are the last 10 commits:

<recent-commits>

!`git log -n 10 --format="%H%n%ad%n%B---" --date=short`

</recent-commits>

# EXPLORATION

Explore the repo and fill your context window with relevant information that will allow you to complete the task.

Pay extra attention to test files that touch the relevant parts of the code.

# EXECUTION

If applicable, use RGR to complete the task.

1. RED: write one test
2. GREEN: write the implementation to pass that test
3. REPEAT until done
4. REFACTOR the code

# FEEDBACK LOOPS

Before each commit, run `.sandcastle/check.sh` and fix whatever it reports until it exits 0. It lints the YAML and
shell files, builds `IssueTracker.slnx` in Release with warnings as errors, runs every test project that doesn't need
Docker, then the Sandcastle tests. While you work, run the test projects that cover your change one at a time with
`dotnet test tests/<Project> -c Release`.

The sandbox has no Docker, on purpose. `check.sh` skips the test projects that need it (those using Testcontainers,
`Aspire.Hosting.Testing` or Playwright, or marked `<RequiresDocker>true</RequiresDocker>`); the host's pre-push gate and
CI run them. Don't try to start Docker or containers. If a test fails because Docker is missing, don't delete, skip or
weaken it: say so in your commit body, and mark its project `<RequiresDocker>true</RequiresDocker>` only if it really
starts containers.

# COMMIT

Make a git commit that follows `.github/instructions/git-commit-instructions.md`:

1. A subject line `<type>(<scope>): <Summary>`, such as `fix(Services): Clear the Author's issue cache on update`:
   imperative, capitalized, no closing period, 72 characters or fewer
2. A body, wrapped at 72 characters, that gives the task completed, the key decisions made, and any blockers or notes
   for the next iteration
3. `Refs #{{TASK_ID}}` as the body's last line

Keep it concise. Never use `--no-verify`.

# THE ISSUE

If the task is not complete, leave a comment on the issue with what was done.

Do not close the issue - this will be done later.

Once complete, output <promise>COMPLETE</promise>.

# FINAL RULES

ONLY WORK ON A SINGLE TASK.
