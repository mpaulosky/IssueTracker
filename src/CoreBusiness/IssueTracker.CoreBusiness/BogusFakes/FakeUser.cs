//-----------------------------------------------------------------------
// <copyright>
//	File:		FakeUser.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
/// FakeUser class
/// </summary>
public static class FakeUser
{

	private static Faker<UserModel>? _userGenerator;

	private static void SetupGenerator()
	{

		Randomizer.Seed = new Random(123);

		_userGenerator = new Faker<UserModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString())
			.RuleFor(x => x.ObjectIdentifier, Guid.NewGuid().ToString())
			.RuleFor(x => x.FirstName, f => f.Name.FirstName())
			.RuleFor(x => x.LastName, f => f.Name.LastName())
			.RuleFor(x => x.DisplayName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
			.RuleFor(x => x.EmailAddress, (f, u) => f.Internet.Email(u.FirstName, u.LastName));

	}

	/// <summary>
	/// Gets a new user.
	/// </summary>
	/// <returns>UserModel</returns>
	public static UserModel GetNewUser()
	{

		SetupGenerator();

		var user = _userGenerator!.Generate();

		user.Id = string.Empty;

		return user;

	}

	/// <summary>
	/// Gets a list of users.
	/// </summary>
	/// <param name="numberOfUsers">The number of users.</param>
	/// <returns>IEnumerable List of UserModels</returns>
	public static IEnumerable<UserModel> GetUsers(int numberOfUsers)
	{

		SetupGenerator();

		var users = _userGenerator!.Generate(numberOfUsers);

		return users;

	}

	/// <summary>
	/// Gets the basic user.
	/// </summary>
	/// <param name="numberOfUsers">The number of users.</param>
	/// <returns>IEnumerable List of BasicUserModels</returns>
	public static IEnumerable<BasicUserModel> GetBasicUser(int numberOfUsers)
	{

		SetupGenerator();

		var users = _userGenerator!.Generate(numberOfUsers);

		var basicUsers = users.Select(c => new BasicUserModel(c));

		return basicUsers;

	}

}