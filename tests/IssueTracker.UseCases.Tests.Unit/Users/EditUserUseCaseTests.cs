namespace IssueTracker.UseCases.Tests.Unit.Users;

public class EditUserUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IUserRepository> mockUserRepository;

	public EditUserUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
	}

	private EditUserUseCase CreateEditUserUseCase()
	{
		return new EditUserUseCase(
				this.mockUserRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var editUserUseCase = this.CreateEditUserUseCase();
		UserModel? user = null;

		// Act
		await editUserUseCase.ExecuteAsync(
			user);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
