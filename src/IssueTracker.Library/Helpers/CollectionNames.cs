namespace IssueTracker.Library.Helpers;

public static class CollectionNames
{
	public static string GetCollectionName(string entityName)
	{
		return entityName switch
		{
			"CategoryModle" => "categories",
			"CommentModel" => "comments",
			"IssueModel" => "issues",
			"StatusModel" => "statuses",
			"UserModel" => "users",
			_ => ""
		};
	}
}