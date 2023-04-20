namespace IssueTracker.UseCases.Tests.Unit.Users;

public class ArchiveUserUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IUserRepository> mockUserRepository;

	public ArchiveUserUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
	}

	private ArchiveUserUseCase CreateArchiveUserUseCase()
	{
		return new ArchiveUserUseCase(
				this.mockUserRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var archiveUserUseCase = this.CreateArchiveUserUseCase();
		UserModel? user = null;

		// Act
		await archiveUserUseCase.ExecuteAsync(
			user);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
