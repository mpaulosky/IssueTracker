//namespace TestingSupport.Library.Fixtures;

//[ExcludeFromCodeCoverage]
//public static class TestStatuses
//{
//	public static StatusModel GetKnownStatus()
//	{
//		var status = new StatusModel
//		{
//			Id = "5dc1039a1521eaa36835e541",
//			StatusDescription = "New Status",
//			StatusName = "New"
//		};

//		return status;
//	}

//	public static StatusModel GetStatus(string id, string statusDescription, string statusName)
//	{
//		var status =
//			new StatusModel { Id = id, StatusDescription = statusDescription, StatusName = statusName };

//		return status;
//	}

//	public static IEnumerable<StatusModel> GetStatuses()
//	{
//		var statuses = new List<StatusModel>
//		{
//			new()
//			{
//				Id = Guid.NewGuid().ToString(),
//				StatusName = "Answered",
//				StatusDescription = "The suggestion was accepted and the corresponding item was created."
//			},
//			new()
//			{
//				Id = Guid.NewGuid().ToString(),
//				StatusName = "Watching",
//				StatusDescription =
//					"The suggestion is interesting. We are watching to see how much interest there is in it."
//			},
//			new()
//			{
//				Id = Guid.NewGuid().ToString(),
//				StatusName = "In Work",
//				StatusDescription = "The suggestion was accepted and it will be released soon."
//			},
//			new()
//			{
//				Id = Guid.NewGuid().ToString(),
//				StatusName = "Dismissed",
//				StatusDescription = "The suggestion was not something that we are going to undertake."
//			}
//		};

//		return statuses;
//	}

//	public static StatusModel GetNewStatus()
//	{
//		var status = new StatusModel { StatusDescription = "New Status", StatusName = "New" };

//		return status;
//	}

//	public static StatusModel GetUpdatedStatus()
//	{
//		var status = new StatusModel
//		{
//			Id = "5dc1039a1521eaa36835e541",
//			StatusDescription = "Updated New Status",
//			StatusName = "New"
//		};

//		return status;
//	}
//}
