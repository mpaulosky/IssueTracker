﻿namespace IssueTracker.Library.Services.UserServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateUserTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserService _sut;
	private string _cleanupValue;

	public CreateUserTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));
		_sut = new UserService(repo);

	}

	[Fact]
	public async Task CreateUser_With_ValidData_Should_CreateAUser_TestAsync()
	{

		// Arrange
		_cleanupValue = "users";
		var expected = FakeUser.GetNewUser();

		// Act
		await _sut.CreateUser(expected);

		// Assert
		expected.Id.Should().NotBeNull();

	}

	[Fact]
	public async Task CreateUser_With_InValidData_Should_FailToCreateAUser_TestAsync()
	{

		// Arrange
		_cleanupValue = "";
		UserModel expected = null;

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateUser(expected));

	}

	public Task InitializeAsync() => Task.CompletedTask;

	public async Task DisposeAsync()
	{

		await _factory.ResetDatabaseAsync(_cleanupValue);

	}

}