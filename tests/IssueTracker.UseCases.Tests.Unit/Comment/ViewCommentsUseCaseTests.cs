namespace IssueTracker.UseCases.Tests.Unit.Comment;

public class ViewCommentsUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public ViewCommentsUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private ViewCommentsUseCase CreateUseCase()
	{
		return new ViewCommentsUseCase(_commentRepositoryMock.Object);
	}

	[Fact]
	public async Task Execute_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var viewCommentsUseCase = this.CreateUseCase();

		// Act
		var result = await viewCommentsUseCase.ExecuteAsync();

		// Assert
		Assert.True(false);
		//this.mockRepository.VerifyAll();
	}
}
