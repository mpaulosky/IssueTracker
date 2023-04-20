namespace IssueTracker.UseCases.Tests.Unit.Users;

public class ViewUserFromAuthenticationUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IUserRepository> mockUserRepository;

	public ViewUserFromAuthenticationUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
	}

	private ViewUserFromAuthenticationUseCase CreateViewUserFromAuthenticationUseCase()
	{
		return new ViewUserFromAuthenticationUseCase(
				this.mockUserRepository.Object);
	}

	[Fact]
	public async Task Execute_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var viewUserFromAuthenticationUseCase = this.CreateViewUserFromAuthenticationUseCase();
		string? userObjectIdentifierId = null;

		// Act
		var result = await viewUserFromAuthenticationUseCase.Execute(
			userObjectIdentifierId);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
