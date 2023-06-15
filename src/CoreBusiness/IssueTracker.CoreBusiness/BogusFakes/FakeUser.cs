// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeUser.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
///   FakeUser class
/// </summary>
public static class FakeUser
{
	/// <summary>
	///   Gets a new user.
	/// </summary>
	/// <param name="keepId">bool whether to keep the generated Id</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>UserModel</returns>
	public static UserModel GetNewUser(bool keepId = false, bool useNewSeed = false)
	{
		var user = GenerateFake(useNewSeed).Generate();

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
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of UserModels</returns>
	public static List<UserModel> GetUsers(int numberOfUsers, bool useNewSeed = false)
	{
		var users = GenerateFake(useNewSeed).Generate(numberOfUsers);

		foreach (var user in users.Where(x => x.Archived))
		{
			user.ArchivedBy = new BasicUserModel(GetNewUser());
		}

		return users;
	}

	/// <summary>
	///   Gets the basic user.
	/// </summary>
	/// <param name="numberOfUsers">The number of users.</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of BasicUserModels</returns>
	public static List<BasicUserModel> GetBasicUser(int numberOfUsers, bool useNewSeed = false)
	{
		var users = GenerateFake(useNewSeed).Generate(numberOfUsers);

		return users.Select(c => new BasicUserModel(c)).ToList();
	}

	/// <summary>
	///  Generates a fake user.
	/// </summary>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A Faker UserModel</returns>
	private static Faker<UserModel> GenerateFake(bool useNewSeed = false)
	{
		var seed = 0;
		if (useNewSeed)
		{
			seed = Random.Shared.Next(10, int.MaxValue);
		}

		return new Faker<UserModel>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(x => x.ObjectIdentifier, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(x => x.FirstName, f => f.Name.FirstName())
			.RuleFor(x => x.LastName, f => f.Name.LastName())
			.RuleFor(x => x.DisplayName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
			.RuleFor(x => x.EmailAddress, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
			.RuleFor(f => f.Archived, f => f.Random.Bool())
			.UseSeed(seed);
	}
}