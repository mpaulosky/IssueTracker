namespace IssueTrackerLibrary.Models;

public static class CollectionNames
{
	public static string GetCollectionName(string entityName)
	{
		switch (entityName)
		{
			case "Comment":
				return "comments";
				break;
			case "Issue":
				return "issues";
				break;
			case "Status":
				return "statuses";
				break;
			case "User":
				return "users";
				break;
			default:
				return "";
		}
	}
}