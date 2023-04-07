namespace IssueTracker.UseCases.Tests.Unit.Comment;

public class ViewCommentsByUserIdUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public ViewCommentsByUserIdUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private ViewCommentsByUserIdUseCase CreateUseCase()
	{
		return new ViewCommentsByUserIdUseCase(_commentRepositoryMock.Object);
	}

	[Fact]
	public async Task Execute_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var viewCommentsByUserIdUseCase = this.CreateUseCase();
		UserModel? user = null;

		// Act
		var result = await viewCommentsByUserIdUseCase.ExecuteAsync(
			user);

		// Assert
		Assert.True(false);
		//this.mockRepository.VerifyAll();
	}
}
