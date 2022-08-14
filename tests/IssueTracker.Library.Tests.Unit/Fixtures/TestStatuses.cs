namespace IssueTracker.Library.Tests.Unit.Fixtures;

[ExcludeFromCodeCoverage]
public static class TestStatuses
{
	public static StatusModel GetKnownStatus()
	{
		var status = new StatusModel
		{
			Id = "5dc1039a1521eaa36835e541", StatusDescription = "New Status", StatusName = "New"
		};

		return status;
	}

	public static StatusModel GetStatus(string id, string statusDescription, string statusName)
	{
		var status = new StatusModel { Id = id, StatusDescription = statusDescription, StatusName = statusName };

		return status;
	}

	public static IEnumerable<StatusModel> GetStatuses()
	{
		var statuses = new List<StatusModel>
		{
			new() { Id = Guid.NewGuid().ToString(), StatusDescription = "New Status", StatusName = "New" },
			new() { Id = Guid.NewGuid().ToString(), StatusDescription = "New Status", StatusName = "New" },
			new() { Id = Guid.NewGuid().ToString(), StatusDescription = "New Status", StatusName = "New" }
		};

		return statuses;
	}

	public static StatusModel GetNewStatus()
	{
		var status = new StatusModel { StatusDescription = "New Status", StatusName = "New" };

		return status;
	}

	public static StatusModel GetUpdatedStatus()
	{
		var status = new StatusModel
		{
			Id = "5dc1039a1521eaa36835e541", StatusDescription = "Updated New Status", StatusName = "New"
		};

		return status;
	}
}