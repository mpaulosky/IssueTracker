// Copyright (c) 2023. All rights reserved.
// File Name :     FakeUser.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
///   FakeUser class
/// </summary>
public static class FakeUser
{
	private static Faker<UserModel>? _userGenerator;

	private static void SetupGenerator()
	{
		Randomizer.Seed = new Random(123);

		_userGenerator = new Faker<UserModel>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(x => x.ObjectIdentifier, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(x => x.FirstName, f => f.Name.FirstName())
			.RuleFor(x => x.LastName, f => f.Name.LastName())
			.RuleFor(x => x.DisplayName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
			.RuleFor(x => x.EmailAddress, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
			.RuleFor(f => f.Archived, f => f.Random.Bool());
	}

	/// <summary>
	///   Gets a new user.
	/// </summary>
	/// <param name="keepId">bool whether to keep the generated Id</param>
	/// <returns>UserModel</returns>
	public static UserModel GetNewUser(bool keepId = false)
	{
		SetupGenerator();

		UserModel? user = _userGenerator!.Generate();

		if (!keepId)
		{
			user.Id = string.Empty;
		}

		user.Archived = false;

		return user;
	}

	/// <summary>
	///   Gets a list of users.
	/// </summary>
	/// <param name="numberOfUsers">The number of users.</param>
	/// <returns>IEnumerable List of UserModels</returns>
	public static IEnumerable<UserModel> GetUsers(int numberOfUsers)
	{
		SetupGenerator();

		List<UserModel>? users = _userGenerator!.Generate(numberOfUsers);

		foreach (UserModel? item in users.Where(x => x.Archived))
		{
			item.ArchivedBy = new BasicUserModel(GetNewUser());
		}

		return users;
	}

	/// <summary>
	///   Gets the basic user.
	/// </summary>
	/// <param name="numberOfUsers">The number of users.</param>
	/// <returns>IEnumerable List of BasicUserModels</returns>
	public static IEnumerable<BasicUserModel> GetBasicUser(int numberOfUsers)
	{
		SetupGenerator();

		IEnumerable<UserModel>? users = GetUsers(numberOfUsers);

		IEnumerable<BasicUserModel>? basicUsers =
			users.Select(c => new BasicUserModel(c));

		return basicUsers;
	}
}