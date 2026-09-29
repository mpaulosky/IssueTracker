// One-off data migration for #152: rename the "Answered" Status to "Accepted" (CONTEXT.md) and replace
// "suggestion" with "issue" in the seeded Status descriptions. Issues keep their own copy of their Status
// (IssueStatus), so those copies are updated too. Safe to run more than once.
//
// Run it with mongosh against the app's database, for example:
//   mongosh "mongodb://localhost:27017/devissuetracker?authSource=admin" scripts/migrations/152-rename-answered-status.js

const descriptions = {
	Accepted: "The issue was accepted and the corresponding item was created.",
	Watching: "The issue is interesting. We are watching to see how much interest there is in it.",
	Upcoming: "The issue was accepted and it will be released soon.",
	Dismissed: "The issue was not something that we are going to undertake."
};

const renamed = db.statuses.updateMany(
	{ status_name: "Answered" },
	{ $set: { status_name: "Accepted" } });
print(`statuses renamed Answered -> Accepted: ${renamed.modifiedCount}`);

const renamedCopies = db.issues.updateMany(
	{ "IssueStatus.StatusName": "Answered" },
	{ $set: { "IssueStatus.StatusName": "Accepted" } });
print(`issue Status copies renamed Answered -> Accepted: ${renamedCopies.modifiedCount}`);

// Only descriptions still worded with "suggestion" are replaced, so an Admin's own wording is kept.
for (const [name, description] of Object.entries(descriptions)) {
	const statuses = db.statuses.updateMany(
		{ status_name: name, status_description: /suggestion/ },
		{ $set: { status_description: description } });
	const copies = db.issues.updateMany(
		{ "IssueStatus.StatusName": name, "IssueStatus.StatusDescription": /suggestion/ },
		{ $set: { "IssueStatus.StatusDescription": description } });
	print(`${name} descriptions updated: ${statuses.modifiedCount} statuses, ${copies.modifiedCount} issue copies`);
}
