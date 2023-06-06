﻿namespace IssueTracker.UseCases.Issue;

[ExcludeFromCodeCoverage]
public class UpdateIssueUseCaseTests
{

	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	public UpdateIssueUseCaseTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();

	}

	private UpdateIssueUseCase CreateUseCase()
	{

		return new UpdateIssueUseCase(_issueRepositoryMock.Object);

	}

	[Fact(DisplayName = "UpdateIssueUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_EditIssue_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		IssueModel issue = FakeIssue.GetIssues(1).First();
		issue.Title = "New Issue";

		// Act
		await sut.ExecuteAsync(issue);

		// Assert
		_issueRepositoryMock.Verify(x =>
				x.UpdateAsync(It.IsAny<IssueModel>()), Times.Once);

	}

	[Fact(DisplayName = "UpdateIssueUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_ReturnArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		const string expectedParamName = "issue";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

}