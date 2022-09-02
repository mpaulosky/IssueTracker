namespace IssueTracker.Library.Models;

[Serializable]
public class BasicStatusModel
{
	public BasicStatusModel()
	{
	}

	public BasicStatusModel(StatusModel status)
	{
		StatusName = status?.StatusName;
		StatusDescription = status?.StatusDescription;
	}

	public string StatusName { get; init; }

	public string StatusDescription { get; init; }
}