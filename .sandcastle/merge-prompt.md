# TASK

Merge the following branches into the current branch:

{{BRANCHES}}

For each branch:

1. Run `git merge <branch> --no-edit`
2. If there are merge conflicts, resolve them intelligently by reading both sides and choosing the correct resolution
3. After resolving conflicts, run `dotnet build IssueTracker.slnx -c Release -warnaserror` and the affected test projects,
   one at a time, with `dotnet test tests/<Project> -c Release`. The sandbox has no Docker, so skip
   `tests/Integration.Tests`, `tests/IssueTracker.PlugIns.Tests.Integration` and `tests/AppHost.Tests`
4. If tests fail, fix the issues before proceeding to the next branch

After all branches are merged, make a single commit summarizing the merge. Its message follows
`.github/instructions/git-commit-instructions.md`: `<type>(<scope>): <Summary>`, imperative, capitalized, no closing
period.

# CLOSE ISSUES

For each branch that was merged, close its issue using the following command:

`gh issue close <ID> --comment "Completed by Sandcastle"`

Here are all the issues:

{{ISSUES}}

Once you've merged everything you can, output <promise>COMPLETE</promise>.
