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

	/// <summary>
	/// Gets the new user.
	/// </summary>
	/// <returns>UserModel</returns>
	public static UserModel GetNewUser()
	{
		Faker<UserModel> userGenerator = new Faker<UserModel>()
			.RuleFor(x => x.ObjectIdentifier, Guid.NewGuid().ToString())
			.RuleFor(x => x.FirstName, f => f.Name.FirstName())
			.RuleFor(x => x.LastName, f => f.Name.LastName())
			.RuleFor(x => x.DisplayName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
			.RuleFor(x => x.EmailAddress, (f, u) => f.Internet.Email(u.FirstName, u.LastName));

		UserModel user = userGenerator.Generate();

		return user;

	}

	/// <summary>
	/// Gets the users.
	/// </summary>
	/// <param name="numberOfUsers">The number of users.</param>
	/// <returns>IEnumerable List of UserModels</returns>
	public static IEnumerable<UserModel> GetUsers(int numberOfUsers)
	{

		Faker<UserModel> userGenerator = new Faker<UserModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString())
			.RuleFor(x => x.ObjectIdentifier, Guid.NewGuid().ToString())
			.RuleFor(x => x.FirstName, f => f.Name.FirstName())
			.RuleFor(x => x.LastName, f => f.Name.LastName())
			.RuleFor(x => x.DisplayName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
			.RuleFor(x => x.EmailAddress, (f, u) => f.Internet.Email(u.FirstName, u.LastName));

		List<UserModel> users = userGenerator.Generate(numberOfUsers);

		return users;

	}

	/// <summary>
	/// Gets the basic user.
	/// </summary>
	/// <param name="numberOfUsers">The number of users.</param>
	/// <returns>IEnumerable List of BasicUserModels</returns>
	public static IEnumerable<BasicUserModel> GetBasicUser(int numberOfUsers)
	{

		Faker<BasicUserModel> userGenerator = new Faker<BasicUserModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString())
		.RuleFor(x => x.DisplayName, f => f.Internet.UserName());

		List<BasicUserModel> basicUsers = userGenerator.Generate(numberOfUsers);

		return basicUsers;

	}

}