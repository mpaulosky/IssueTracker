using System;
using System.Collections.Generic;

namespace IssueTracker.Library.UnitTests.Fixtures;

[ExcludeFromCodeCoverage]
public static class TestStatuses
{
	public static Status GetKnownStatus()
	{
		var status = new Status() { Id = "5dc1039a1521eaa36835e541", StatusDescription = "New Status", StatusName = "New", };
		
		return status;
	}

	public static Status GetStatus(string id, string statusDescription, string statusName)
	{
		var status = new Status() { Id = id, StatusDescription = statusDescription, StatusName = statusName };

		return status;
	}

	public static IEnumerable<Status> GetStatuses()
	{
		var statuses = new List<Status>
		{
			new() { Id = Guid.NewGuid().ToString(), StatusDescription = "New Status", StatusName = "New",},
			new() { Id = Guid.NewGuid().ToString(), StatusDescription = "New Status", StatusName = "New",},
			new() { Id = Guid.NewGuid().ToString(), StatusDescription = "New Status", StatusName = "New",}
		};
		
		return statuses;
	}
}