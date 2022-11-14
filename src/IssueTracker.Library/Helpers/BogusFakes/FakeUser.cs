//-----------------------------------------------------------------------
// <copyright file="FakeUser.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Helpers.BogusFakes;

public static class FakeUser
{

	public static UserModel GetNewUser()
	{
		var userGenerator = new Faker<UserModel>()
			.RuleFor(x => x.ObjectIdentifier, Guid.NewGuid().ToString())
			.RuleFor(x => x.FirstName, f => f.Name.FirstName())
			.RuleFor(x => x.LastName, f => f.Name.LastName())
			.RuleFor(x => x.DisplayName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
			.RuleFor(x => x.EmailAddress, (f, u) => f.Internet.Email(u.FirstName, u.LastName));

		var user = userGenerator.Generate();

		return user;

	}

	public static IEnumerable<UserModel> GetUsers(int numberOfUsers)
	{

		var userGenerator = new Faker<UserModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString())
			.RuleFor(x => x.ObjectIdentifier, Guid.NewGuid().ToString())
			.RuleFor(x => x.FirstName, f => f.Name.FirstName())
			.RuleFor(x => x.LastName, f => f.Name.LastName())
			.RuleFor(x => x.DisplayName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
			.RuleFor(x => x.EmailAddress, (f, u) => f.Internet.Email(u.FirstName, u.LastName));

		var users = userGenerator.Generate(numberOfUsers);

		return users;

	}

	public static IEnumerable<BasicUserModel> GetBasicUser(int numberOfUsers)
	{

		var userGenerator = new Faker<BasicUserModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString())
		.RuleFor(x => x.DisplayName, f => f.Internet.UserName());

		var basicUsers = userGenerator.Generate(numberOfUsers);

		return basicUsers;

	}

}