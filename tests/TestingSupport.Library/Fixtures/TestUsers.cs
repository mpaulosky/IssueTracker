//namespace TestingSupport.Library.Fixtures;

//[ExcludeFromCodeCoverage]
//public static class TestUsers
//{
//	public static IEnumerable<UserModel> GetUsers()
//	{
//		var expected = new List<UserModel>
//		{
//			new()
//			{
//				Id = Guid.NewGuid().ToString(),
//				ObjectIdentifier = Guid.NewGuid().ToString(),
//				FirstName = "Jim",
//				LastName = "Test",
//				DisplayName = "jim test",
//				EmailAddress = "jim.test@test.com"

//			},
//			new()
//			{
//				Id = Guid.NewGuid().ToString(),
//				ObjectIdentifier = Guid.NewGuid().ToString(),
//				FirstName = "Sam",
//				LastName = "Test",
//				DisplayName = "sam test",
//				EmailAddress = "sam.test@test.com"
//			},
//			new()
//			{
//				Id = Guid.NewGuid().ToString(),
//				ObjectIdentifier = Guid.NewGuid().ToString(),
//				FirstName = "Tim",
//				LastName = "Test",
//				DisplayName = "tim test",
//				EmailAddress = "tim.test@test.com"
//			}
//		};

//		return expected;
//	}

//	public static UserModel GetKnownUser()
//	{
//		var user = new UserModel
//		{
//			Id = "5dc1039a1521eaa36835e545",
//			ObjectIdentifier = "5dc1039a1521eaa36835e542",
//			FirstName = "Jim",
//			LastName = "Test",
//			DisplayName = "jim test",
//			EmailAddress = "jim.test@test.com"
//		};

//		return user;
//	}


//	public static UserModel GetKnownUserWithNoVotedOn()
//	{
//		var user = new UserModel
//		{
//			Id = "5dc1039a1521eaa36835e541",
//			ObjectIdentifier = "5dc1039a1521eaa36835e542",
//			FirstName = "Jim",
//			LastName = "Test",
//			DisplayName = "jim test",
//			EmailAddress = "jim.test@test.com"
//		};

//		return user;
//	}

//	public static UserModel GetUser(
//		string userId,
//		string objectIdentifier,
//		string firstName,
//		string lastName,
//		string displayName,
//		string email)
//	{
//		var expected = new UserModel
//		{
//			Id = userId,
//			ObjectIdentifier = objectIdentifier,
//			FirstName = firstName,
//			LastName = lastName,
//			DisplayName = "jim test",
//			EmailAddress = "jim.test@test.com"
//		};

//		return expected;
//	}

//	public static UserModel GetNewUser()
//	{
//		var user = new UserModel
//		{
//			ObjectIdentifier = "5dc1039a1521eaa36835e542",
//			FirstName = "Jim",
//			LastName = "Test",
//			DisplayName = "jim test",
//			EmailAddress = "jim.test@test.com"
//		};

//		return user;
//	}

//	public static UserModel GetUpdatedUser()
//	{
//		var user = new UserModel
//		{
//			Id = "5dc1039a1521eaa36835e545",
//			ObjectIdentifier = "5dc1039a1521eaa36835e542",
//			FirstName = "Jim",
//			LastName = "Test",
//			DisplayName = "jim test Update",
//			EmailAddress = "jim.test@test.com"
//		};

//		return user;
//	}
//}
