namespace IssueTracker.CoreBusiness.Models;

[ExcludeFromCodeCoverage]
public class CommentModelTests
{
	[Fact]
	public void Test_GetId_ReturnsString()
	{
		// Arrange
		const string expected = "testId";
		CommentModel comment = new() { Id = expected };

		// Act
		string actual = comment.Id;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetTitle_ReturnsString()
	{
		// Arrange
		const string expected = "testTitle";
		CommentModel comment = new() { Title = expected };

		// Act
		string actual = comment.Title;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetDescription_ReturnsString()
	{
		// Arrange
		const string expected = "testDesc";
		CommentModel comment = new() { Description = expected };

		// Act
		string actual = comment.Description;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetDateCreated_ReturnsDateTime()
	{
		// Arrange
		DateTime expected = DateTime.UtcNow;
		CommentModel comment = new() { DateCreated = expected };

		// Act
		DateTime actual = comment.DateCreated;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetIssue_ReturnsIssueModel()
	{
		// Arrange
		BasicIssueModel expected = FakeIssue.GetBasicIssues(1).First();
		CommentModel comment = new() { Issue = expected };

		// Act
		BasicIssueModel? actual = comment.Issue;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetAuthor_ReturnsBasicUserModel()
	{
		// Arrange
		BasicUserModel expected = FakeUser.GetBasicUser(1).First();
		CommentModel comment = new() { Author = expected };

		// Act
		BasicUserModel actual = comment.Author;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetUserVotes_ReturnsHashSetString()
	{
		// Arrange
		BasicUserModel expectedUser = FakeUser.GetBasicUser(1).First();
		HashSet<string> expected = new HashSet<string> { expectedUser.Id };
		CommentModel comment = new() { UserVotes = expected };

		// Act
		HashSet<string> actual = comment.UserVotes;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void Test_GetArchived_ReturnsBoolean(bool expected)
	{
		// Arrange
		CommentModel comment = new() { Archived = expected };

		// Act
		bool actual = comment.Archived;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetArchivedBy_ReturnsBasicUserModel()
	{
		// Arrange
		BasicUserModel expected = FakeUser.GetBasicUser(1).First();
		CommentModel comment = new() { ArchivedBy = expected };

		// Act
		BasicUserModel actual = comment.ArchivedBy;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void Test_GetIsAnswer_ReturnsBoolean(bool expected)
	{
		// Arrange
		CommentModel comment = new() { IsAnswer = expected };

		// Act
		bool actual = comment.IsAnswer;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetAnswerSelectedBy_ReturnsBasicUserModel()
	{
		// Arrange
		BasicUserModel expected = FakeUser.GetBasicUser(1).First();
		CommentModel comment = new() { AnswerSelectedBy = expected };

		// Act
		BasicUserModel? actual = comment.AnswerSelectedBy;

		// Assert
		Assert.Equal(expected, actual);
	}
}