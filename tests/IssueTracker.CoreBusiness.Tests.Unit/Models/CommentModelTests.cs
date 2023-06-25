namespace IssueTracker.CoreBusiness.Models;

[ExcludeFromCodeCoverage]
public class CommentModelTests
{
	[Fact]
	public void Test_GetId_ReturnsString()
	{
		// Arrange
		const string expected = "testId";
		var comment = new CommentModel { Id = expected };

		// Act
		var actual = comment.Id;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetTitle_ReturnsString()
	{
		// Arrange
		const string expected = "testTitle";
		var comment = new CommentModel { Title = expected };

		// Act
		var actual = comment.Title;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetDescription_ReturnsString()
	{
		// Arrange
		const string expected = "testDesc";
		var comment = new CommentModel { Description = expected };

		// Act
		var actual = comment.Description;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetDateCreated_ReturnsDateTime()
	{
		// Arrange
		var expected = DateTime.UtcNow;
		var comment = new CommentModel { DateCreated = expected };

		// Act
		var actual = comment.DateCreated;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetCommentOnSource_ReturnsCommentOnSourceModel()
	{
		// Arrange
		var expected = FakeSource.GetSource(false);
		var comment = new CommentModel { CommentOnSource = expected };

		// Act
		var actual = comment.CommentOnSource;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetAuthor_ReturnsBasicUserModel()
	{
		// Arrange
		var expected = FakeUser.GetBasicUser(1).First();
		var comment = new CommentModel { Author = expected };

		// Act
		var actual = comment.Author;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetUserVotes_ReturnsHashSetString()
	{
		// Arrange
		BasicUserModel expectedUser = FakeUser.GetBasicUser(1).First();
		var expected = new HashSet<string>() { expectedUser.Id.ToString() };
		var comment = new CommentModel { UserVotes = expected };

		// Act
		var actual = comment.UserVotes;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void Test_GetArchived_ReturnsBoolean(bool expected)
	{
		// Arrange
		var comment = new CommentModel { Archived = expected };

		// Act
		var actual = comment.Archived;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetArchivedBy_ReturnsBasicUserModel()
	{
		// Arrange
		var expected = FakeUser.GetBasicUser(1).First();
		var comment = new CommentModel { ArchivedBy = expected };

		// Act
		var actual = comment.ArchivedBy;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void Test_GetIsAnswer_ReturnsBoolean(bool expected)
	{
		// Arrange
		var comment = new CommentModel { IsAnswer = expected };

		// Act
		var actual = comment.IsAnswer;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Test_GetAnswerSelectedBy_ReturnsBasicUserModel()
	{
		// Arrange
		var expected = FakeUser.GetBasicUser(1).First();
		var comment = new CommentModel { AnswerSelectedBy = expected };

		// Act
		var actual = comment.AnswerSelectedBy;

		// Assert
		Assert.Equal(expected, actual);
	}
}