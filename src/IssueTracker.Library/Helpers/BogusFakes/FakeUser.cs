namespace IssueTracker.Library.Helpers.BogusFakes;

public static class FakeUser
{

	public static UserModel GetNewUser()
	{
		var userGenerator = new Faker<UserModel>()
		.RuleFor(x => x.FirstName, f => f.Name.FirstName())
		.RuleFor(x => x.LastName, f => f.Name.LastName())
		.RuleFor(x => x.DisplayName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
		.RuleFor(x => x.EmailAddress, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
		.RuleFor(x => x.AuthoredComments, FakeComment.GetBasicComments(1).ToList())
		.RuleFor(x => x.AuthoredIssues, FakeIssue.GetBasicIssues(2).ToList());

		var user = userGenerator.Generate();

		return user;

	}

	public static IEnumerable<UserModel> GetUsers(int numberOfUsers)
	{

		var userGenerator = new Faker<UserModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(x => x.FirstName, f => f.Name.FirstName())
		.RuleFor(x => x.LastName, f => f.Name.LastName())
		.RuleFor(x => x.DisplayName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
		.RuleFor(x => x.EmailAddress, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
		.RuleFor(x => x.AuthoredComments, FakeComment.GetBasicComments(1).ToList())
		.RuleFor(x => x.AuthoredIssues, FakeIssue.GetBasicIssues(2).ToList());

		var users = userGenerator.Generate(numberOfUsers);

		return users;

	}

	public static IEnumerable<BasicUserModel> GetBasicUser(int numberOfUsers)
	{
		var userGenerator = new Faker<BasicUserModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(x => x.DisplayName, f => f.Internet.UserName());

		var basicUsers = userGenerator.Generate(numberOfUsers);

		return basicUsers;

	}

}