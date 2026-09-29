# IssueTracker

A place where signed-in people raise Issues about a product, discuss them in Comments, and vote on the most useful
answers, while Admins decide which Issues everyone gets to see.

## Language

### People

**User**:
A person who has signed in. Every Issue, Comment and Upvote belongs to one.
_Avoid_: Account, member, customer

**Author**:
The User who wrote a particular Issue or Comment.
_Avoid_: Owner, creator, poster

**Admin**:
A User allowed to moderate: approve or reject Issues, archive records, and manage Categories and Statuses.
_Avoid_: Moderator, administrator, superuser

### Issues

**Issue**:
A question, problem or request a User raises about the product, with a title and a description.
_Avoid_: Suggestion, ticket, bug, request

**Category**:
The kind of Issue, such as Design, Documentation or Implementation, chosen by the Author when raising it.
_Avoid_: Type, tag, label

**Status**:
Where an Admin says an approved Issue stands: Accepted, Watching, Upcoming or Dismissed. An Accepted Issue has been
taken on; that says nothing about whether any Comment is its Answer.
_Avoid_: State, stage, resolution, "Answered" (for the Accepted Status)

**Pending Issue**:
An Issue no Admin has approved or rejected yet. It appears only in its Author's and the Admins' lists.
_Avoid_: Draft, unapproved, waiting

**Approval**:
An Admin's decision that an Issue may be shown to every User.
_Avoid_: Release, publish, accept

**Rejection**:
An Admin's decision that an Issue will never be shown to other Users. It is final.
_Avoid_: Decline, deny, dismissal

### Comments

**Comment**:
A User's reply on an Issue.
_Avoid_: Reply, post, response

**Upvote**:
A User's endorsement of someone else's Comment. A User can upvote a Comment once and withdraw it; an Author can't
upvote their own.
_Avoid_: Like, vote, thumbs-up

**Answer**:
The Comment picked as resolving its Issue, by the Issue's Author or an Admin.
_Avoid_: Solution, accepted comment, resolution

### Record keeping

**Archive**:
Taking a record out of view while keeping it, along with who archived it. It applies to Users, Issues, Comments,
Categories and Statuses.
_Avoid_: Delete, remove, hide
