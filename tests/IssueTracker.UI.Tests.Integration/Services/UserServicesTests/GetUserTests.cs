
namespace IssueTracker.UI.Tests.Integration.Services.UserServicesTests;

public class GetUserTests : IClassFixture<IssueTrackerUIFactory>
{

	private readonly IssueTrackerUIFactory _factory;
	private readonly UserService _sut;

	public GetUserTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		var repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));
		_sut = new UserService(repo);

	}

	[Fact]
	public async Task GetUser_With_WithData_Should_ReturnAValidUser_TestAsync()
	{

		// Arrange
		var expected = FakeUser.GetNewUser();
		await _sut.CreateUser(expected);

		// Act
		var result = await _sut.GetUser(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Fact]
	public async Task GetUser_With_WithoutData_Should_ReturnNothing_TestAsync()
	{
		// Arrange
		var id = "62cf2ad6326e99d665759e5a";

		// Act
		var result = await _sut.GetUser(id);

		// Assert
		result.Should().BeNull();

	}

	[Fact]
	public async Task GetUser_With_NullId_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		string? id = null;

		// Act
		var act = async () => await _sut.GetUser(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

	[Fact]
	public async Task GetUser_With_EmptyStringId_Should_ThrowArgumentException_Test()
	{

		// Arrange
		string? id = "";

		// Act
		var act = async () => await _sut.GetUser(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentException>();

	}

}
