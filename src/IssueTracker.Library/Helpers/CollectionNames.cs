namespace IssueTracker.Library.Helpers;

public static class CollectionNames
{
	public static string GetCollectionName(string entityName)
	{
		return entityName switch
		{
			"Comment" => "comments",
			"Issue" => "issues",
			"Status" => "statuses",
			"User" => "users",
			_ => ""
		};
	}
}