# TASK

Merge the following branches into the current branch:

{{BRANCHES}}

For each branch:

1. Run `git merge <branch> --no-edit`
2. If there are merge conflicts, resolve them intelligently by reading both sides and choosing the correct resolution
3. Run `.sandcastle/check.sh`. The sandbox has no Docker, so it skips the test projects that need it; the host's
   pre-push gate and CI run them
4. If it fails, fix the issues and run it again before proceeding to the next branch

After all branches are merged, make a single commit summarizing the merge. Its message follows
`.github/instructions/git-commit-instructions.md`: `<type>(<scope>): <Summary>`, imperative, capitalized, no closing
period.

# CLOSE ISSUES

For each branch that was merged, close its issue using the following command:

`gh issue close <ID> --comment "Completed by Sandcastle"`

Here are all the issues:

{{ISSUES}}

Once you've merged everything you can, output <promise>COMPLETE</promise>.
